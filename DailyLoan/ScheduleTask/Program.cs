using DailyLoan.Model.Entities.DailyLoan;
using DailyLoan.Library.Status;
using DailyLoan.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DailyLoan
{
    public class ScheduleTask
    {
        static void Main(string[] args)
        {
            Console.WriteLine("...Read Config...");
            string json = File.ReadAllText("appsettings.json");
            appsettings settings = JsonSerializer.Deserialize<appsettings>(json);
            DailyLoanContext dailyContext = new DailyLoanContext(settings.ConnectionStrings.DailyLoanConnection);
            Console.WriteLine("...Load data...");
            DateTime now = DateTime.Now;
            if(now.Hour == 23)
            {
                #region pay 0 and alert
                Console.WriteLine("...Check Pay 0...");
                var allcon = dailyContext.Contract.Where(x => x.Status != ContractStatus.StatusContract_Closed
                                                        && x.Status != ContractStatus.StatusContract_WaitConfirm
                                                        && x.Status != ContractStatus.StatusContract_NotApprove).ToList();
                bool isChange = false; 
                if (allcon.Count > 0)
                {
                    for (int i = 0; i < allcon.Count; i++)
                    {
                        var tran = dailyContext.Transaction.Where(x => x.FromContractId == allcon[i].Id && x.PayDate == now.Date).FirstOrDefault();
                        if (tran == null)
                        {
                            var cus = dailyContext.Customer.Find(allcon[i].CustomerId);
                            int clid = cus != null ? cus.CustomerLineId : 0;
                            Transaction tmp = new Transaction()
                            {
                                ContractId = allcon[i].Id,
                                CustomerLineId = clid,
                                Amount = 0,
                                Type = TransactionType_Status.CollectFromCustomer,
                                Remark = "",
                                CreateBy = 0,
                                CreateDate = now,
                                PayDate = now.Date,
                                FromContractId = allcon[i].Id,
                                CustomerId = allcon[i].CustomerId
                            };
                            dailyContext.Transaction.Add(tmp);
                            var customer = dailyContext.Customer.Find(allcon[i].CustomerId);
                            var alltran = dailyContext.Transaction.Where(x => x.ContractId == allcon[i].Id && x.Type == TransactionType_Status.CollectFromCustomer).OrderByDescending(x => x.CreateDate).ToList();
                            int notpay = 0, paypartial = 0;
                            if (alltran.Count >= 3)
                            {
                                for (int j = 0; j < 3; j++)
                                {
                                    if (alltran[j].Amount < customer.Installment / 2) paypartial++;
                                    if (alltran[j].Amount == 0) notpay++;
                                }
                                if (notpay == 3 && (allcon[i].Status == ContractStatus.StatusContract_Normal || allcon[i].Status == ContractStatus.StatusContract_Loss))
                                {
                                    Notification n = new Notification()
                                    {
                                        Message = Notification_Message.NotPay,
                                        Status = Notification_Status.Alert,
                                        Type = Notification_Type.NotPay,
                                        CreateBy = allcon[i].CreateBy,
                                        CreateDate = DateTime.Now,
                                        ContractId = allcon[i].Id,
                                        CustomerLineId = customer.CustomerLineId,
                                        HouseId = (dailyContext.User.Where(x => x.Id == allcon[i].CreateBy).FirstOrDefault().HouseId)
                                    };
                                    dailyContext.Notification.Add(n);
                                    if(allcon[i].Status != ContractStatus.StatusContract_Dead)
                                    {
                                        allcon[i].Status = ContractStatus.StatusContract_Alert;
                                        customer.Status = CustomerStatus.StatusCustomer_Bad;
                                    }
                                }
                                else if (paypartial == 3 && (allcon[i].Status == ContractStatus.StatusContract_Normal || allcon[i].Status == ContractStatus.StatusContract_Loss))
                                {
                                    Notification n = new Notification()
                                    {
                                        Message = Notification_Message.NotPay,
                                        Status = Notification_Status.Alert,
                                        Type = Notification_Type.NotPay,
                                        CreateBy = allcon[i].CreateBy,
                                        CreateDate = DateTime.Now,
                                        ContractId = allcon[i].Id,
                                        CustomerLineId = customer.CustomerLineId,
                                        HouseId = (dailyContext.User.Where(x => x.Id == allcon[i].CreateBy).FirstOrDefault().HouseId)
                                    };
                                    dailyContext.Notification.Add(n);
                                    if (allcon[i].Status != ContractStatus.StatusContract_Dead)
                                    {
                                        allcon[i].Status = ContractStatus.StatusContract_Alert;
                                        customer.Status = CustomerStatus.StatusCustomer_Bad;
                                    }
                                }
                            }
                            isChange = true;
                        }
                    }
                    if (isChange) dailyContext.SaveChanges();
                    Console.WriteLine("...Finish...");
                }
                #endregion
            }
            if (now.Hour == 1)
            {
                #region dead 100
                Console.WriteLine("...Check Dead 100...");
                bool isChange = false;
                var allcon = dailyContext.Contract.Where(x => x.Status == ContractStatus.StatusContract_Dead).ToList();
                if(allcon.Count > 0)
                {
                    for (int i = 0; i < allcon.Count; i++)
                    {
                        if((now.Date-allcon[i].UpdateDate.Value.Date).TotalDays >= 100)
                        {
                            allcon[i].Status = ContractStatus.StatusContract_Closed;
                            isChange = true;
                        }
                    }
                    if (isChange) dailyContext.SaveChanges();
                }
                #endregion
                #region pay special
                Console.WriteLine("...Check Pay Special...");
                var alltran = dailyContext.Transaction.Where(x => x.Type == TransactionType_Status.SaveCollectFromCustomer&&x.PayDate == now.Date).ToList();
                if (alltran.Count > 0)
                {
                    for (int i = 0; i < alltran.Count; i++)
                    {
                        var con = dailyContext.Contract.Where(x => x.CustomerId == alltran[i].CustomerId 
                                                            && x.Status != ContractStatus.StatusContract_Closed
                                                            && x.Status != ContractStatus.StatusContract_NotApprove
                                                            && x.Status != ContractStatus.StatusContract_WaitConfirm).FirstOrDefault();
                        if(con != null)
                        {
                            alltran[i].ContractId = con.Id;
                            con.TotalPay += alltran[i].Amount;
                            if (con.TotalAmount <= con.TotalPay) con.Status = ContractStatus.StatusContract_Closed;
                        }
                    }
                    dailyContext.SaveChanges();
                }
                #endregion
                var spe = dailyContext.SpecialRate.Where(x => x.OpenDate == now.Date).FirstOrDefault();
                if(spe != null)
                {
                    dailyContext.Contract.ToList().ForEach(x => x.SpecialRateCount = 0);
                    dailyContext.SaveChanges();
                }
                Console.WriteLine("...Finish...");
            }
        }
    }
    class appsettings
    {
        public ConnectionStrings ConnectionStrings { get; set; }
        public object AppsettingModel { get; set; }
        public object Logging { get; set; }
        public string AllowedHosts { get; set; }
    }
    class ConnectionStrings
    {
        public string DailyLoanConnection { get; set; }
    }
}
