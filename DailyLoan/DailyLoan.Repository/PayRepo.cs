
using DailyLoan.Repository.Interfaces;
using DailyLoan.Library;
using DailyLoan.Library.Status;
using DailyLoan.Model.Entities.DailyLoan;
using DailyLoan.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using DailyLoan.Model.Resoinse.Pay;
using DailyLoan.Model.AppSettings;
using Microsoft.Extensions.Options;

namespace DailyLoan.Repository
{
    public class PayRepo : IPayRepo
    {
        private readonly DailyLoanContext _DailyLoanContext;
        private readonly AppsettingModel _appsettingModel;
        public PayRepo(DailyLoanContext dailyLoanContext,
            IOptions<AppsettingModel> appsettingModel)
        {
            _DailyLoanContext = dailyLoanContext;
            _appsettingModel = appsettingModel.Value;
        }
        #region getValue
        public async Task<List<int>> GetAlert(int uid, int useraccess)
        {
            var hid = GetHouseIdByUserId(uid);
            List<int> rtn = new List<int>();
            var tmp = await _DailyLoanContext.Notification.Where(x => x.Status == Notification_Status.Alert).ToListAsync();

            List<ManagementContract> tmp1 = await (from c in _DailyLoanContext.Contract.Where(x => x.Status == ContractStatus.StatusContract_WaitConfirm)
                                                  //join cu in _DailyLoanContext.User on c.ApproverId equals cu.Id
                                                  join us in _DailyLoanContext.StatusContract on c.Status equals us.Id
                                                  select new ManagementContract()
                                                  {
                                                      Id = c.Id,
                                                      ContractId = c.ContractId,
                                                      CustomerId = c.CustomerId,
                                                      GuarantorId = c.GuarantorId,
                                                      ApproverId = c.ApproverId,
                                                      Remark = c.Remark,
                                                      TotalAmount = c.TotalAmount,
                                                      TotalPay = c.TotalPay,
                                                      Status = c.Status,
                                                      SpecialRateCount = c.SpecialRateCount,
                                                      CutCount = c.CutCount,
                                                      Type = c.Type,
                                                      ExContractPay = c.ExContractPay,
                                                      CreateBy = c.CreateBy,
                                                      CreateDate = c.CreateDate,
                                                      UpdateBy = c.UpdateBy,
                                                      UpdateDate = c.UpdateDate,
                                                      StatusText = us.Status,
                                                      //ApproverName = cu.Firstname + " " + cu.Lastname
                                                  }).ToListAsync();

            for (int i = 0; i < tmp1.Count(); i++)
            {
                tmp1[i].Customer = GetCustomer(tmp1[i].CustomerId);
            }
            if (useraccess == StatusUserAccess.UserAccess_Admin || useraccess == StatusUserAccess.UserAccess_Audit)
            {
                tmp = tmp.Where(x => x.HouseId == hid).ToList();
                tmp1 = tmp1.Where(x => x.Customer.HouseId == hid).ToList();
            }
            rtn.Add(tmp.Count);
            rtn.Add(tmp1.Count);
            return rtn;
        }
        public async Task<List<User>> GetAllUserByCustomerLine(int cid)
        {
            List<User> rtn = await (from u in _DailyLoanContext.User
                                              join up in _DailyLoanContext.UserPermission on u.Id equals up.UserId
                                              where up.CustomerLineId == cid
                                              select new User()
                                              {
                                                  Id = u.Id,
                                                  Firstname = u.Firstname,
                                                  Lastname = u.Lastname
                                              }).ToListAsync();
            return rtn;
        }
        public int GetHouseIdByUserId(int uid)
        {
            return (_DailyLoanContext.User.Where(x => x.Id == uid).FirstOrDefault().HouseId);
        }
        public int GetUseraccessByUserId(int uid)
        {
            return (_DailyLoanContext.User.Where(x => x.Id == uid).FirstOrDefault().UserAccess);
        }
        public int GetCustomerLineIdByUserId(int uid)
        {
            var rtn = _DailyLoanContext.UserPermission.Where(x => x.UserId == uid).FirstOrDefault();
            return (rtn == null ? 0 : rtn.CustomerLineId);
        }
        public string GetIdcardByCustomerId(int cid)
        {
            var rtn = _DailyLoanContext.Customer.Where(x => x.Id == cid).FirstOrDefault();
            return rtn.Idcard;
        }
        public string GetContractIDByIdcard(string idcard)
        {
            var rtn = (from c in _DailyLoanContext.Contract
                       join cc in _DailyLoanContext.Customer on c.CustomerId equals cc.Id
                       where cc.Idcard == idcard && c.Status != ContractStatus.StatusContract_Closed
                       select new Contract() { ContractId = c.ContractId }).FirstOrDefault();
            return rtn.ContractId;
        }
        public async Task<List<ManagementCustomer>> SearchCustomer(int uid,string idcard,string name,string firstname,string lastname,string address)
        {
            List<ManagementCustomer> rtn = await (from c in _DailyLoanContext.Customer
                                      join us in _DailyLoanContext.StatusCustomer on c.Status equals us.Id
                                      join cl in _DailyLoanContext.CustomerLine on c.CustomerLineId equals cl.Id
                                      join h in _DailyLoanContext.House on cl.HouseId equals h.Id
                                      select new ManagementCustomer()
                                      {
                                          Id = c.Id,
                                          Firstname = c.Firstname,
                                          Lastname = c.Lastname,
                                          Nickname = c.Nickname,
                                          Phone1 = c.Phone1,
                                          Phone2 = c.Phone2,
                                          Status = c.Status,
                                          Idcard = c.Idcard,
                                          Address = c.Address,
                                          ShortAddress = c.ShortAddress,
                                          CustomerLineId = c.CustomerLineId,
                                          DailyCollect = c.DailyCollect,
                                          Installment = c.Installment,
                                          CreateBy = c.CreateBy,
                                          CreateDate = c.CreateDate,
                                          UpdateBy = c.UpdateBy,
                                          UpdateDate = c.UpdateDate,
                                          StatusText = us.Status,
                                          HouseId = h.Id,
                                          HouseText = h.HouseName,
                                          CustomerLineText = cl.CustomerLineName
                                      }).ToListAsync();
            int ua = GetUseraccessByUserId(uid);
            if (ua == StatusUserAccess.UserAccess_Admin || ua == StatusUserAccess.UserAccess_Audit)
            {
                var hid = GetHouseIdByUserId(uid);
                rtn = rtn.Where(x => x.HouseId == hid).ToList();
            }
            else if (ua == StatusUserAccess.UserAccess_Agent)
            {
                var clid = GetCustomerLineIdByUserId(uid);
                rtn = rtn.Where(x => x.CustomerLineId == clid).ToList();
            }
            if (!String.IsNullOrEmpty(idcard)) rtn = rtn.Where(x => x.Idcard.Contains(idcard)).ToList();
            if (!String.IsNullOrEmpty(name)) rtn = rtn.Where(x => x.Firstname.Contains(name)|| x.Lastname.Contains(name)).ToList();
            if (!String.IsNullOrEmpty(firstname)) rtn = rtn.Where(x => x.Firstname.Contains(firstname)).ToList();
            if (!String.IsNullOrEmpty(lastname)) rtn = rtn.Where(x => x.Lastname.Contains(lastname)).ToList();
            if (!String.IsNullOrEmpty(address)) rtn = rtn.Where(x => x.Address.Contains(address)).ToList();
            rtn = rtn.OrderBy(x => x.Address).ThenBy(x => x.Firstname).ThenBy(x => x.Lastname).ToList();
            return rtn;
        }
        public async Task<List<ManagementContract>> SearchContract(int uid, string idcard, string firstname, string lastname, string address)
        {
            List<ManagementContract> rtn = await (from c in _DailyLoanContext.Contract
                                                  //join cu in _DailyLoanContext.User on c.ApproverId equals cu.Id
                                                  join us in _DailyLoanContext.StatusContract on c.Status equals us.Id
                                                  select new ManagementContract()
                                                  {
                                                      Id = c.Id,
                                                      ContractId = c.ContractId,
                                                      CustomerId = c.CustomerId,
                                                      GuarantorId = c.GuarantorId,
                                                      ApproverId = c.ApproverId,
                                                      TotalAmount = c.TotalAmount,
                                                      Remark = c.Remark,
                                                      TotalPay = c.TotalPay,
                                                      Status = c.Status,
                                                      SpecialRateCount = c.SpecialRateCount,
                                                      CutCount = c.CutCount,
                                                      Type = c.Type,
                                                      ExContractPay = c.ExContractPay,
                                                      CreateBy = c.CreateBy,
                                                      CreateDate = c.CreateDate,
                                                      UpdateBy = c.UpdateBy,
                                                      UpdateDate = c.UpdateDate,
                                                      StatusText = us.Status,
                                                      //ApproverName = cu.Firstname + " " + cu.Lastname
                                                  }).ToListAsync();
            int ua = GetUseraccessByUserId(uid); 
            for (int i = 0; i < rtn.Count(); i++)
            {
                rtn[i].Guaruntor = GetCustomer(rtn[i].GuarantorId);
                rtn[i].Customer = GetCustomer(rtn[i].CustomerId);
            }
            if (ua == StatusUserAccess.UserAccess_Admin || ua == StatusUserAccess.UserAccess_Audit)
            {
                var hid = GetHouseIdByUserId(uid);
                rtn = rtn.Where(x => x.Customer.HouseId == hid).ToList();
            }
            else if (ua == StatusUserAccess.UserAccess_Agent)
            {
                var clid = GetCustomerLineIdByUserId(uid);
                rtn = rtn.Where(x => x.Customer.CustomerLineId == clid).ToList();
            }
            if (!String.IsNullOrEmpty(idcard)) rtn = rtn.Where(x => x.Customer.Idcard.Contains(idcard)).ToList();
            if (!String.IsNullOrEmpty(firstname)) rtn = rtn.Where(x => x.Customer.Firstname.Contains(firstname)).ToList();
            if (!String.IsNullOrEmpty(lastname)) rtn = rtn.Where(x => x.Customer.Lastname.Contains(lastname)).ToList();
            if (!String.IsNullOrEmpty(address)) rtn = rtn.Where(x => x.Customer.Address.Contains(address)).ToList();
            rtn = rtn.OrderBy(x => x.Status).ThenBy(x => x.Customer.Address).ThenBy(x => x.Customer.Firstname).ThenBy(x => x.Customer.Lastname).ToList();
            return rtn;
        }
        public bool isExistContract(int cid)
        {
            var rtn = _DailyLoanContext.Contract.Where(x => x.CustomerId == cid&&x.Status != ContractStatus.StatusContract_Closed).FirstOrDefault();
            return (rtn != null);
        }
        public string getConfig(string name,int uid)
        {
            var rtn = _DailyLoanContext.Config.Where(x => x.Name == name && x.HouseId == GetHouseIdByUserId(uid)).FirstOrDefault().Value;
            return rtn;
        }
        public bool CheckSpecialTime(int uid,DateTime date)
        {
            uid = 1;
            var rtn = _DailyLoanContext.SpecialRate.Where(x => x.HouseId == GetHouseIdByUserId(uid) && x.StartDate<=date &&x.EndDate >= date).FirstOrDefault();
            return rtn != null;
        }
        public SpecialRate GetSpecialRate(int uid, DateTime date)
        {
            uid = 1;
            var rtn = _DailyLoanContext.SpecialRate.Where(x => x.HouseId == GetHouseIdByUserId(uid) && x.StartDate <= date && x.EndDate >= date).FirstOrDefault();
            return rtn;
        }
        public string getHouseRate(int uid,DateTime date)
        {
            bool isSpecialTime = CheckSpecialTime(uid,date);
            string rtn = null;
            if (isSpecialTime)
            {
                rtn = GetSpecialRate(uid,date).HouseRate.ToString();
            }
            else
            {
                rtn = _DailyLoanContext.Config.Where(x => x.Name == "HouseRate" && x.HouseId == GetHouseIdByUserId(uid)).FirstOrDefault().Value;
            }
            return rtn;
        }
        #endregion
        #region Customer
        public async Task<List<ManagementCustomer>> GetAllCustomer(int uid, int useraccess)
        {
            List<ManagementCustomer> rtn = await (from c in _DailyLoanContext.Customer
                                                  join us in _DailyLoanContext.StatusCustomer on c.Status equals us.Id
                                                  join cl in _DailyLoanContext.CustomerLine on c.CustomerLineId equals cl.Id
                                                  join h in _DailyLoanContext.House on cl.HouseId equals h.Id
                                                  select new ManagementCustomer()
                                                  {
                                                      Id = c.Id,
                                                      Firstname = c.Firstname,
                                                      Lastname = c.Lastname,
                                                      Nickname = c.Nickname,
                                                      Phone1 = c.Phone1,
                                                      Phone2 = c.Phone2,
                                                      Status = c.Status,
                                                      Idcard = c.Idcard,
                                                      Address = c.Address,
                                                      ShortAddress = c.ShortAddress,
                                                      CustomerLineId = c.CustomerLineId,
                                                      DailyCollect = c.DailyCollect,
                                                      Installment = c.Installment,
                                                      CreateBy = c.CreateBy,
                                                      CreateDate = c.CreateDate,
                                                      UpdateBy = c.UpdateBy,
                                                      UpdateDate = c.UpdateDate,
                                                      StatusText = us.Status,
                                                      HouseId = h.Id,
                                                      HouseText = h.HouseName,
                                                      CustomerLineText = cl.CustomerLineName
                                                  }).ToListAsync();
            if (useraccess == StatusUserAccess.UserAccess_Admin || useraccess == StatusUserAccess.UserAccess_Audit)
            {
                var hid = GetHouseIdByUserId(uid);
                rtn = rtn.Where(x => x.HouseId == hid).ToList();
            }
            else if (useraccess == StatusUserAccess.UserAccess_Agent)
            {
                var clid = GetCustomerLineIdByUserId(uid);
                rtn = rtn.Where(x => x.CustomerLineId == clid).ToList();
            }
            rtn = rtn.OrderBy(x => x.Address).ThenBy(x => x.Firstname).ThenBy(x => x.Lastname).ToList();
            return rtn;
        }
        public ManagementCustomer GetCustomer(int cid)
        {
            ManagementCustomer rtn = (from c in _DailyLoanContext.Customer
                                      join us in _DailyLoanContext.StatusCustomer on c.Status equals us.Id
                                      join cl in _DailyLoanContext.CustomerLine on c.CustomerLineId equals cl.Id
                                      join h in _DailyLoanContext.House on cl.HouseId equals h.Id
                                      where c.Id == cid
                                      select new ManagementCustomer()
                                      {
                                          Id = c.Id,
                                          Firstname = c.Firstname,
                                          Lastname = c.Lastname,
                                          Nickname = c.Nickname,
                                          Phone1 = c.Phone1,
                                          Phone2 = c.Phone2,
                                          Status = c.Status,
                                          Idcard = c.Idcard,
                                          Address = c.Address,
                                          ShortAddress = c.ShortAddress,
                                          CustomerLineId = c.CustomerLineId,
                                          DailyCollect = c.DailyCollect,
                                          Installment = c.Installment,
                                          CreateBy = c.CreateBy,
                                          CreateDate = c.CreateDate,
                                          UpdateBy = c.UpdateBy,
                                          UpdateDate = c.UpdateDate,
                                          StatusText = us.Status,
                                          HouseId = h.Id,
                                          HouseText = h.HouseName,
                                          CustomerLineText = cl.CustomerLineName
                                      }).FirstOrDefault();
            return rtn;
        }
        public async Task<bool> EditCustomer(Customer req)
        {
            if (req.Id == 0)
            {
                if (req.CustomerLineId == 0) req.CustomerLineId = GetCustomerLineIdByUserId(req.CreateBy);
                 _DailyLoanContext.Customer.Add(req);
            }
            else
            {
                var cus = await _DailyLoanContext.Customer.FindAsync(req.Id);
                if (cus == null)
                {
                    return await Task.FromResult(false);
                }
                cus.Idcard = req.Idcard;
                cus.Firstname = req.Firstname;
                cus.Lastname = req.Lastname;
                cus.Nickname = req.Nickname;
                cus.Address = req.Address;
                cus.ShortAddress = req.ShortAddress;
                cus.Phone1 = req.Phone1;
                cus.Phone2 = req.Phone2;
                cus.Status = req.Status;
                cus.CustomerLineId = req.CustomerLineId;
                cus.UpdateBy = req.UpdateBy;
                cus.UpdateDate = req.UpdateDate;
            }
            return (await _DailyLoanContext.SaveChangesAsync()) > 0;
        }
        public async Task<bool> MoveCustomer(int cid, int clid, int uid)
        {
            Customer req = await _DailyLoanContext.Customer.FindAsync(cid);
            if (req.CustomerLineId == clid) return false;
            Customer newcus = new Customer()
            {
                CustomerLineId = clid,
                Firstname = req.Firstname,
                Lastname = req.Lastname,
                Nickname = req.Nickname,
                Idcard = req.Idcard,
                Phone1 = req.Phone1,
                Phone2 = req.Phone2,
                Address = req.Address,
                ShortAddress = req.ShortAddress,
                Status = req.Status,
                CreateBy = uid,
                CreateDate = DateTime.Now,
                DailyCollect = 0,
                Installment = 0
            };
            _DailyLoanContext.Customer.Add(newcus);
            return (await _DailyLoanContext.SaveChangesAsync()) > 0;
        }
        public async Task<bool> DeleteCustomer(int cid)
        {
            _DailyLoanContext.Customer.Remove(_DailyLoanContext.Customer.Where(x => x.Id == cid).FirstOrDefault());
            var allcon = await _DailyLoanContext.Contract.Where(x => x.CustomerId == cid).ToListAsync();
            for(int i = 0; i < allcon.Count; i++)
            {
                var alltran = await _DailyLoanContext.Transaction.Where(x => x.ContractId == allcon[i].Id).ToListAsync();
                if (alltran.Count > 0) _DailyLoanContext.Transaction.RemoveRange(alltran);
                var allnot = await _DailyLoanContext.Notification.Where(x => x.ContractId == allcon[i].Id).ToListAsync();
                if (allnot.Count > 0) _DailyLoanContext.Notification.RemoveRange(allnot);
            }
            if (allcon.Count > 0) _DailyLoanContext.Contract.RemoveRange(allcon);
            return (await _DailyLoanContext.SaveChangesAsync()) > 0;
        }
        #endregion
        #region Contract
        public async Task<List<ManagementContract>> GetAllContract(int uid, int useraccess)
        {
            List<ManagementContract> rtn = await (from c in _DailyLoanContext.Contract
                                                  //join cu in _DailyLoanContext.User on c.ApproverId equals cu.Id
                                                  join us in _DailyLoanContext.StatusContract on c.Status equals us.Id
                                                  join ts in _DailyLoanContext.RequestType on c.Type equals ts.Id
                                                  select new ManagementContract()
                                                  {
                                                      Id = c.Id,
                                                      ContractId = c.ContractId,
                                                      CustomerId = c.CustomerId,
                                                      GuarantorId = c.GuarantorId,
                                                      ApproverId = c.ApproverId,
                                                      TotalAmount = c.TotalAmount,
                                                      Remark = c.Remark,
                                                      TotalPay = c.TotalPay,
                                                      Status = c.Status,
                                                      SpecialRateCount = c.SpecialRateCount,
                                                      CutCount = c.CutCount,
                                                      Type = c.Type,
                                                      ExContractPay = c.ExContractPay,
                                                      CreateBy = c.CreateBy,
                                                      CreateDate = c.CreateDate,
                                                      UpdateBy = c.UpdateBy,
                                                      UpdateDate = c.UpdateDate,
                                                      StatusText = us.Status,
                                                      TypeText = ts.Name
                                                      //ApproverName = cu.Firstname + " " + cu.Lastname
                                                  }).ToListAsync();
            for(int i = 0;i < rtn.Count();i++)
            {
                rtn[i].Guaruntor = GetCustomer(rtn[i].GuarantorId);
                rtn[i].Customer = GetCustomer(rtn[i].CustomerId);
            }
            if (useraccess == StatusUserAccess.UserAccess_Admin || useraccess == StatusUserAccess.UserAccess_Audit)
            {
                var hid = GetHouseIdByUserId(uid);
                rtn = rtn.Where(x => x.Customer.HouseId == hid).ToList();
            }
            else if (useraccess == StatusUserAccess.UserAccess_Agent)
            {
                var clid = GetCustomerLineIdByUserId(uid);
                rtn = rtn.Where(x => x.Customer.CustomerLineId == clid).ToList();
            }
            rtn = rtn.OrderBy(x => x.Status).ThenBy(x => x.Customer.Address).ThenBy(x => x.Customer.Firstname).ThenBy(x => x.Customer.Lastname).ToList();
            return rtn;
        }
        public ManagementContract GetContract(int cid)
        {
            ManagementContract rtn = (from c in _DailyLoanContext.Contract
                                                 //join cu in _DailyLoanContext.User on c.ApproverId equals cu.Id
                                                 join us in _DailyLoanContext.StatusContract on c.Status equals us.Id
                                                 join ts in _DailyLoanContext.RequestType on c.Type equals ts.Id
                                                   where c.Id == cid
                                                 select new ManagementContract()
                                                 {
                                                     Id = c.Id,
                                                     ContractId = c.ContractId,
                                                     CustomerId = c.CustomerId,
                                                     GuarantorId = c.GuarantorId,
                                                     ApproverId = c.ApproverId,
                                                     TotalAmount = c.TotalAmount,
                                                     Remark = c.Remark,
                                                     TotalPay = c.TotalPay,
                                                     Status = c.Status,
                                                     SpecialRateCount = c.SpecialRateCount,
                                                     CutCount = c.CutCount,
                                                     Type = c.Type,
                                                     ExContractPay = c.ExContractPay,
                                                     CreateBy = c.CreateBy,
                                                     CreateDate = c.CreateDate,
                                                     UpdateBy = c.UpdateBy,
                                                     UpdateDate = c.UpdateDate,
                                                     StatusText = us.Status,
                                                     TypeText = ts.Name
                                                     //ApproverName = cu.Firstname + " " + cu.Lastname,
                                                 }).FirstOrDefault();

            var hrateins = Convert.ToDouble(getConfig("HouseRate", rtn.CreateBy));
            var hrate = Convert.ToDouble(getHouseRate(rtn.CreateBy, DateTime.Now));
            var arate = Convert.ToDouble(getConfig("AgentRate", rtn.CreateBy));
            rtn.Bounty = (double)(arate * rtn.TotalAmount / 100);
            rtn.Collect = (double)(hrateins * rtn.TotalAmount / 100);
            bool isSpecial = CheckSpecialTime(rtn.CreateBy, DateTime.Now) && rtn.SpecialRateCount.Value < 3;
            rtn.Special = (double)(isSpecial? ((hrate - hrateins) * rtn.TotalAmount / 100): 0);

            var customer =  _DailyLoanContext.Customer.Find(rtn.CustomerId);
            var profit = Convert.ToDouble(getConfig("TotalProfit", rtn.CreateBy));
            var oldcontract = _DailyLoanContext.Contract.Where(x => x.CustomerId==rtn.CustomerId&&x.Id<cid
                                                                &&x.Status!=ContractStatus.StatusContract_NotApprove
                                                                &&x.Status!=ContractStatus.StatusContract_WaitConfirm)
                                                            .OrderByDescending(x => x.Id).FirstOrDefault();
            //double oldamount = (double)customer.Installment * 100 / hrateins;
            //double oldprofit = (double)customer.Installment * profit / hrateins;
            double oldamount = oldcontract==null ? 0 : (double)oldcontract.TotalAmount;
            double oldprofit = oldcontract == null ? 0 : (double)oldcontract.TotalAmount * profit / 100;

            var tran = _DailyLoanContext.Transaction.Where(x => x.Type == TransactionType_Status.PayToCustomer && x.FromContractId == cid).ToList();
            rtn.PaytoCustomer = tran.Count==0? (double)rtn.TotalAmount - (oldamount - (double)rtn.ExContractPay) - oldprofit - rtn.Collect - rtn.Special - rtn.Bounty : (double)tran.Select(x => x.Amount).Sum(); 
            rtn.Guaruntor = GetCustomer(rtn.GuarantorId);
            rtn.Customer = GetCustomer(rtn.CustomerId);
            return rtn;
        }
        public async Task<bool> EditContract(Contract req)
        {
            if (req.Id == 0)
            {
                req.Type = Request_Type.NewContract;
                req.ExContractPay = 0;
                _DailyLoanContext.Contract.Add(req);
                var customer = await _DailyLoanContext.Customer.FindAsync(req.CustomerId);
                customer.Installment = 0;
            }
            else
            {
                var cus = await _DailyLoanContext.Contract.FindAsync(req.Id);
                if (cus == null)
                {
                    return await Task.FromResult(false);
                }

                //cus.ContractId = req.ContractId;
                cus.CustomerId = req.CustomerId;
                cus.GuarantorId = req.GuarantorId;
                //cus.ApproverId = req.ApproverId;
                cus.TotalAmount = req.TotalAmount;
                //cus.TotalPay = req.TotalPay;
                cus.Status = req.Status;
                //cus.SpecialRateCount = req.SpecialRateCount;
                //cus.CutCount = req.CutCount;
                cus.UpdateBy = req.UpdateBy;
                cus.UpdateDate = req.UpdateDate;
            }
            return (await _DailyLoanContext.SaveChangesAsync()) > 0;
        }
        public async Task<bool> Approve(int cid,int uid)
        {
            var req = await _DailyLoanContext.Contract.FindAsync(cid);
            req.Status = ContractStatus.StatusContract_Normal;
            req.UpdateBy = uid;
            req.UpdateDate = DateTime.Now;
            var customer = await _DailyLoanContext.Customer.FindAsync(req.CustomerId);
            var hrateins = Convert.ToDouble(getConfig("HouseRate", req.CreateBy));
            var arate = Convert.ToDouble(getConfig("AgentRate", req.CreateBy));
            var hrate = Convert.ToDouble(getHouseRate(req.CreateBy,DateTime.Now));
            bool isSpecial = CheckSpecialTime(uid, DateTime.Now) && req.SpecialRateCount.Value < 3;
            int specialCount = (isSpecial ? req.SpecialRateCount.Value + 1 : req.SpecialRateCount.Value);
            req.SpecialRateCount = specialCount;

            var profit = Convert.ToDouble(getConfig("TotalProfit", req.CreateBy));
            var oldcontract = _DailyLoanContext.Contract.Where(x => x.CustomerId == req.CustomerId && x.Id < cid
                                                                && x.Status != ContractStatus.StatusContract_NotApprove
                                                                && x.Status != ContractStatus.StatusContract_WaitConfirm)
                                                            .OrderByDescending(x => x.Id).FirstOrDefault();
            //double oldamount = (double)customer.Installment * 100 / hrateins;
            //double oldprofit = (double)customer.Installment * profit / hrateins;
            double oldamount = oldcontract == null ? 0 : (double)oldcontract.TotalAmount;
            double oldprofit = oldcontract == null ? 0 : (double)oldcontract.TotalAmount * profit / 100;

            customer.Installment = hrateins * req.TotalAmount / 100;
            customer.UpdateBy = uid;
            customer.UpdateDate = DateTime.Now;
            double collect = (double)(hrateins * req.TotalAmount / 100), bounty = (double)(arate * req.TotalAmount / 100);
            double savecollect = (double)(isSpecial?(hrate * req.TotalAmount / 100)- collect:0);
            double payto = (double)req.TotalAmount - (oldamount - (double)req.ExContractPay) - oldprofit - collect - savecollect - bounty;
            Transaction agent = new Transaction()
            {
                ContractId = req.Id,
                CustomerLineId = customer.CustomerLineId,
                Amount = bounty,
                Type = TransactionType_Status.Bounty,
                Remark = "",
                CreateBy = req.CreateBy,
                CreateDate = DateTime.Now,
                PayDate = DateTime.Now.Date,
                FromContractId = req.Id,
                CustomerId = req.CustomerId
            };
            Transaction house = new Transaction()
            {
                ContractId = req.Id,
                CustomerLineId = customer.CustomerLineId,
                Amount = collect,
                Type = TransactionType_Status.CollectFromCustomerCut,
                Remark = "งวด1 เปิดสัญญา",
                CreateBy = req.CreateBy,
                CreateDate = DateTime.Now,
                PayDate = DateTime.Now.Date,
                FromContractId = req.Id,
                CustomerId = req.CustomerId
            };
            Transaction cust = new Transaction()
            {
                ContractId = req.Id,
                CustomerLineId = customer.CustomerLineId,
                Amount = payto,
                Type = TransactionType_Status.PayToCustomer,
                Remark = "",
                CreateBy = req.CreateBy,
                CreateDate = DateTime.Now,
                PayDate = DateTime.Now.Date,
                FromContractId = req.Id,
                CustomerId = req.CustomerId
            };
            if (isSpecial)
            {
                Transaction savehouse = new Transaction()
                {
                    ContractId = 0,
                    CustomerLineId = customer.CustomerLineId,
                    Amount = savecollect,
                    Type = TransactionType_Status.SaveCollectFromCustomer,
                    Remark = "ดอกพิเศษของลูกค้าครั้งที่"+specialCount.ToString(),
                    CreateBy = req.CustomerId,
                    CreateDate = DateTime.Now,
                    PayDate = GetSpecialRate(uid, DateTime.Now).OpenDate,
                    FromContractId = req.Id,
                    CustomerId = req.CustomerId
                };
                _DailyLoanContext.Transaction.Add(savehouse);
            }
            req.TotalPay = collect;
            _DailyLoanContext.Transaction.Add(agent);
            _DailyLoanContext.Transaction.Add(house);
            _DailyLoanContext.Transaction.Add(cust);
            
            var allopen = await _DailyLoanContext.Contract.Where(x=>x.Id!=cid&&x.CustomerId==req.CustomerId&&x.Status!=ContractStatus.StatusContract_Closed && x.Status != ContractStatus.StatusContract_NotApprove).ToListAsync();
            for(int i = 0; i < allopen.Count; i++)
            {
                allopen[i].Status = ContractStatus.StatusContract_Closed;
            }

            return (await _DailyLoanContext.SaveChangesAsync()) > 0;
        }
        public async Task<bool> Deny(int cid, string remark)
        {
            var req = await _DailyLoanContext.Contract.FindAsync(cid);
            req.Remark = remark;
            req.Status = ContractStatus.StatusContract_NotApprove;
            //_DailyLoanContext.Contract.Remove(req);
            return (await _DailyLoanContext.SaveChangesAsync()) > 0;
        }
        public async Task<bool> DeleteContract(int cid)
        {
            _DailyLoanContext.Contract.Remove(_DailyLoanContext.Contract.Where(x => x.Id == cid).FirstOrDefault());
            var alltran = await _DailyLoanContext.Transaction.Where(x => x.ContractId==cid).ToListAsync();
            if (alltran.Count > 0) _DailyLoanContext.Transaction.RemoveRange(alltran);
            var allnot = await _DailyLoanContext.Notification.Where(x => x.ContractId == cid).ToListAsync();
            if (allnot.Count > 0) _DailyLoanContext.Notification.RemoveRange(allnot);
            return (await _DailyLoanContext.SaveChangesAsync()) > 0;
        }
        #endregion
        #region DailyCost
        public DailyCost GetDailyCost(int clid,DateTime date)
        {
            var req = _DailyLoanContext.DailyCost.Where(x => x.CustomerLineId==clid&&x.Date.Value.Date==date.Date).FirstOrDefault();
            if (req == null) req = new DailyCost();
            if (req.Police1 == null) req.Police1 = 0;
            if (req.PoliceRemark1 == null) req.PoliceRemark1 = "";
            if (req.Police2 == null) req.Police2 = 0;
            if (req.PoliceRemark2 == null) req.PoliceRemark2 = "";
            if (req.Police3 == null) req.Police3 = 0;
            if (req.PoliceRemark3 == null) req.PoliceRemark3 = "";
            if (req.Gas == null) req.Gas = 0;
            if (req.Topup == null) req.Topup = 0;
            if (req.Caught == null) req.Caught = 0;
            if (req.BikeMaintenance == null) req.BikeMaintenance = 0;
            if (req.Other == null) req.Other = 0;
            if (req.OtherDetail == null) req.OtherDetail = "";
            if (req.Remark == null) req.Remark = "";
            if (req.OtherIncome == null) req.OtherIncome = 0;
            if (req.OtherIncomeRemark == null) req.OtherIncomeRemark = "";
            if (req.PayOut == null) req.PayOut = 0;
            if (req.Receive == null) req.Receive = 0;
            if (req.Allowance == null) req.Allowance = 0;
            return req;
        }
        public async Task<DailyReportResponse> GetMustReturn(int clid, DateTime date)
        {
            var cost = _DailyLoanContext.DailyCost.Where(x => x.CustomerLineId == clid && x.Date.Value.Date == date.Date).FirstOrDefault();

            var result = new DailyReportResponse();
            var data = await (from tran in _DailyLoanContext.Transaction.Where(x => x.CreateDate.Date == date.Date && x.CustomerLineId == clid)
                              select tran).ToListAsync();
            double total = 0, sumexpense = 0;
            if (cost != null) 
            { 
                total = (double)(cost.PayOut==null?0:cost.PayOut);
                sumexpense += (double)(cost.Police1 == null ? 0 : cost.Police1);
                sumexpense += (double)(cost.Police2 == null ? 0 : cost.Police2);
                sumexpense += (double)(cost.Police3 == null ? 0 : cost.Police3);
                sumexpense += (double)(cost.Other == null ? 0 : cost.Other);
                sumexpense += (double)(cost.Caught == null ? 0 : cost.Caught);
                sumexpense += (double)(cost.Topup == null ? 0 : cost.Topup);
                sumexpense += (double)(cost.Gas == null ? 0 : cost.Gas);
                sumexpense += (double)(cost.BikeMaintenance == null ? 0 : cost.BikeMaintenance);
                total -= sumexpense;
            }

            var collect = await (from con in _DailyLoanContext.Contract
                                 join cus in _DailyLoanContext.Customer on con.CustomerId equals cus.Id
                                 where cus.CustomerLineId == clid
                                 && con.Status != ContractStatus.StatusContract_Closed
                                 && con.Status != ContractStatus.StatusContract_WaitConfirm
                                 && con.Status != ContractStatus.StatusContract_NotApprove
                                 select cus).ToListAsync();
            if(data != null)
            {
                double bounty = (double)data.Where(x => x.Type == TransactionType_Status.Bounty).Select(x => x.Amount).Sum();
                double allowance = bounty >= _appsettingModel.Allowance ? 0 : (_appsettingModel.Allowance - bounty);
                double paytocustomer = (double)data.Where(x => x.Type == TransactionType_Status.PayToCustomer).Select(x => x.Amount).Sum();
                double collectfrom = (double)data.Where(x => x.Type == TransactionType_Status.CollectFromCustomer).Select(x => x.Amount).Sum();
                total -= paytocustomer;
                total -= bounty;
                total -= allowance;
                total += collectfrom;
                result = new DailyReportResponse()
                {
                    bounty = bounty,
                    allowance = allowance,
                    housepayout = cost == null ? 0 : cost.PayOut,
                    paytocustomer = paytocustomer,
                    collect = collectfrom,
                    collectall = data.Where(x => x.Type == TransactionType_Status.CollectFromCustomer || x.Type == TransactionType_Status.CollectFromCustomerCut || x.Type == TransactionType_Status.SaveCollectFromCustomer).Select(x => x.Amount).Sum(),
                    mustcollect = collect.Select(x => x.Installment).Sum(),
                    mustreturn = total,
                    sumexpense = sumexpense
                };
            }
            return result;
        }
        public async Task<double> GetPayToCustomer(int clid, DateTime date)
        {
            var alltran = await _DailyLoanContext.Transaction.Where(x => x.CreateDate.Date == date.Date && x.CustomerLineId == clid && x.Type == TransactionType_Status.PayToCustomer).ToListAsync();
            return (double)alltran.Select(x => x.Amount).Sum();
        }
        public async Task<bool> SaveDailyCost(DailyCost req,int uid)
        {
            if (req.Police1 == null) req.Police1 = 0;
            if (req.PoliceRemark1 == null) req.PoliceRemark1 = "";
            if (req.Police2 == null) req.Police2 = 0;
            if (req.PoliceRemark2 == null) req.PoliceRemark2 = "";
            if (req.Police3 == null) req.Police3 = 0;
            if (req.PoliceRemark3 == null) req.PoliceRemark3 ="";
            if (req.Gas == null) req.Gas = 0;
            if (req.Topup == null) req.Topup = 0;
            if (req.Caught == null) req.Caught = 0;
            if (req.BikeMaintenance == null) req.BikeMaintenance = 0;
            if (req.Other == null) req.Other = 0;
            if (req.OtherDetail == null) req.OtherDetail = "";
            if (req.Remark == null) req.Remark = "";
            if (req.OtherIncome == null) req.OtherIncome = 0;
            if (req.OtherIncomeRemark == null|| req.OtherIncomeRemark == "undefined" ) req.OtherIncomeRemark = "";
            if (req.PayOut == null) req.PayOut = 0;
            if (req.Receive == null) req.Receive = 0;
            if (req.Allowance == null) req.Allowance = 0;
            var tmp = _DailyLoanContext.DailyCost.Where(x => x.CustomerLineId == req.CustomerLineId && x.Date.Value.Date == req.Date.Value.Date).FirstOrDefault();
            if (tmp == null)
            {
                req.CreateBy = uid;
                req.CreateDate = DateTime.Now;
                _DailyLoanContext.DailyCost.Add(req);

            }
            else
            {
                tmp.UpdateBy = uid;
                tmp.UpdateDate = DateTime.Now;
                if(req.PayOut!=null)tmp.PayOut = req.PayOut;
                if (req.Receive != null) tmp.Receive = req.Receive;
                if (req.Allowance != null) tmp.Allowance = req.Allowance;
                /*
                if (req.SalaryReceived1 != null) tmp.SalaryReceived1 = req.SalaryReceived1;
                if (req.Salary1 != null) tmp.Salary1 = req.Salary1;
                if (req.SalaryReceived2 != null) tmp.SalaryReceived2 = req.SalaryReceived2;
                if (req.Salary2 != null) tmp.Salary2 = req.Salary2;
                if (req.SalaryReceived3 != null) tmp.SalaryReceived3 = req.SalaryReceived3;
                if (req.Salary3 != null) tmp.Salary3 = req.Salary3;
                if (req.SalaryReceived4 != null) tmp.SalaryReceived4 = req.SalaryReceived4;
                if (req.Salary4 != null) tmp.Salary4 = req.Salary4;
                if (req.SalaryReceived5 != null) tmp.SalaryReceived5 = req.SalaryReceived5;
                if (req.Salary5 != null) tmp.Salary5 = req.Salary5;
                if (req.PaperInk != null) tmp.PaperInk = req.PaperInk;
                */
                if (req.Police1 != null) tmp.Police1 = req.Police1;
                if (req.PoliceRemark1 != null) tmp.PoliceRemark1 = req.PoliceRemark1;
                if (req.Police2 != null) tmp.Police2 = req.Police2;
                if (req.PoliceRemark2 != null) tmp.PoliceRemark2 = req.PoliceRemark2;
                if (req.Police3 != null) tmp.Police3 = req.Police3;
                if (req.PoliceRemark3 != null) tmp.PoliceRemark3 = req.PoliceRemark3;
                if (req.Gas != null) tmp.Gas = req.Gas;
                if (req.Topup != null) tmp.Topup = req.Topup;
                if (req.Caught != null) tmp.Caught = req.Caught;
                if (req.BikeMaintenance != null) tmp.BikeMaintenance = req.BikeMaintenance;
                if (req.Other != null) tmp.Other = req.Other;
                if (req.OtherDetail != null) tmp.OtherDetail = req.OtherDetail;
                if (req.Remark != null) tmp.Remark = req.Remark;
                if (req.OtherIncome != null) tmp.OtherIncome = req.OtherIncome;
                if (req.OtherIncomeRemark != null) tmp.OtherIncomeRemark = req.OtherIncomeRemark;
            }
            return (await _DailyLoanContext.SaveChangesAsync()) > 0;
        }
        #endregion
        #region DailyReport
        public async Task<DailyReportResponse> GetDailyReport(int uid, DateTime date)
        {
            var cost = _DailyLoanContext.DailyCost.Where(x => x.CustomerLineId == uid && x.Date.Value.Date == date.Date).FirstOrDefault();
            var result = new DailyReportResponse();
            var data = await (from tran in _DailyLoanContext.Transaction.Where(x => x.CreateDate.Date == date.Date && x.CustomerLineId == uid)
                                select tran ).ToListAsync();
            double total = 0, sumexpense = 0;
            if (cost != null)
            {
                total = (double)(cost.PayOut == null ? 0 : cost.PayOut);
                sumexpense += (double)(cost.Police1 == null ? 0 : cost.Police1);
                sumexpense += (double)(cost.Police2 == null ? 0 : cost.Police2);
                sumexpense += (double)(cost.Police3 == null ? 0 : cost.Police3);
                sumexpense += (double)(cost.Other == null ? 0 : cost.Other);
                sumexpense += (double)(cost.Caught == null ? 0 : cost.Caught);
                sumexpense += (double)(cost.Topup == null ? 0 : cost.Topup);
                sumexpense += (double)(cost.Gas == null ? 0 : cost.Gas);
                sumexpense += (double)(cost.BikeMaintenance == null ? 0 : cost.BikeMaintenance);
                total -= sumexpense;
            }
            var collect = await (from con in _DailyLoanContext.Contract
                                 join cus in _DailyLoanContext.Customer on con.CustomerId equals cus.Id
                                    where cus.CustomerLineId == uid 
                                    && con.Status != ContractStatus.StatusContract_Closed 
                                    && con.Status != ContractStatus.StatusContract_WaitConfirm
                                    && con.Status != ContractStatus.StatusContract_NotApprove
                                 select cus).ToListAsync();
            if(data != null)
            {
                double bounty = (double)data.Where(x => x.Type == TransactionType_Status.Bounty).Select(x => x.Amount).Sum();
                double allowance = bounty >= _appsettingModel.Allowance ? 0 : (_appsettingModel.Allowance - bounty);
                double paytocustomer = (double)data.Where(x => x.Type == TransactionType_Status.PayToCustomer).Select(x => x.Amount).Sum();
                double collectfrom = (double)data.Where(x => x.Type == TransactionType_Status.CollectFromCustomer).Select(x => x.Amount).Sum();
                total -= paytocustomer;
                total -= bounty;
                total -= allowance;
                total += collectfrom;
                result = new DailyReportResponse()
                {
                    bounty = bounty,
                    allowance = allowance,
                    housepayout = cost == null?0: cost.PayOut,
                    paytocustomer = paytocustomer,
                    collect = collectfrom,
                    collectall = data.Where(x => x.Type == TransactionType_Status.CollectFromCustomer|| x.Type == TransactionType_Status.CollectFromCustomerCut || x.Type == TransactionType_Status.SaveCollectFromCustomer).Select(x => x.Amount).Sum(),
                    mustcollect = collect.Select(x => x.Installment).Sum(),
                    mustreturn = total,
                    sumexpense = sumexpense
                };
            }
            return result;
        }
        #endregion
        #region History
        public async Task<ManagementCut> GetCutDetail(int cid)
        {
            var data = await (from con in _DailyLoanContext.Contract
                             join c in _DailyLoanContext.Customer on con.CustomerId equals c.Id
                             join cl in _DailyLoanContext.CustomerLine on c.CustomerLineId equals cl.Id
                             where con.Id == cid
                            select new ManagementCut() {
                                hid = cl.HouseId.Value,
                                installment = c.Installment.Value,
                                totalamount = con.TotalAmount.Value,
                                totalpay = con.TotalPay.Value,
                                cutcount = con.CutCount.Value,
                            }).FirstOrDefaultAsync();
            if (data != null) data.profitrate = Convert.ToDouble(_DailyLoanContext.Config.Where(x=>x.Name=="TotalProfit"&&x.HouseId==data.hid).FirstOrDefault().Value);
            return data;
        }
        public async Task<bool> CutRequest(int cid,double amount,int uid,int type)
        {
            var contract = await _DailyLoanContext.Contract.FindAsync(cid);
            var customer = await _DailyLoanContext.Customer.FindAsync(contract.CustomerId);
            var data = await (from tran in _DailyLoanContext.Transaction.Where(x => (x.Type == TransactionType_Status.CollectFromCustomer|| x.Type == TransactionType_Status.CollectFromCustomerCut) && x.ContractId == cid)
                              select tran).ToListAsync();
            int cutcount = type == Request_Type.CutIncrease ? 0 : contract.CutCount.Value + 1;
            bool isSpecial = CheckSpecialTime(uid, DateTime.Now) && contract.SpecialRateCount.Value < 3;
            int specialCount = contract.SpecialRateCount.Value;
            Contract req = new Contract()
            {
                ContractId = customer.Idcard + "-" + DateTime.Now.ToString("yyyyMMddhhmmssfff") ,
                CustomerId = contract.CustomerId,
                GuarantorId = contract.GuarantorId,
                TotalAmount = amount,
                TotalPay = 0,
                SpecialRateCount = specialCount,
                CutCount = cutcount,
                Type = type,
                ExContractPay = data.Select(x => x.Amount).Sum(),
                Status = Request_Status.Pending,
                CreateBy = uid,
                CreateDate = DateTime.Now
            };
            _DailyLoanContext.Contract.Add(req);
            return (await _DailyLoanContext.SaveChangesAsync()) > 0;
        }
        public bool isExistRequest(int cid,int type)
        {
            return _DailyLoanContext.Request.Where(x => x.ContractId==cid&&x.Type==type&&(x.Status==Request_Status.Pending|| x.Status == Request_Status.Approve)).Count()>0;
        }
        #endregion
        #region Collector
        public async Task<List<Transaction>> GetPayHistory(int cid)
        {
            return await _DailyLoanContext.Transaction.Where(x => (x.FromContractId == cid&&(x.Type == TransactionType_Status.CollectFromCustomer
            || x.Type == TransactionType_Status.SaveCollectFromCustomer 
            || x.Type == TransactionType_Status.CollectFromCustomerCut))
            || (x.ContractId == cid && x.Type == TransactionType_Status.SaveCollectFromCustomer)).ToListAsync();
        }
        public async Task<bool> CollectCustomer(int uid,int cid,double amount,string remark)
        {
            var contract = await _DailyLoanContext.Contract.FindAsync(cid);
            Transaction t = new Transaction()
            {
                ContractId = cid,
                CustomerLineId = GetCustomerLineIdByUserId(uid),
                Amount = amount,
                Type = TransactionType_Status.CollectFromCustomer,
                Remark = String.IsNullOrEmpty(remark)||remark=="null"?"":remark,
                CreateBy = uid,
                CreateDate = DateTime.Now,
                PayDate = DateTime.Now.Date,
                FromContractId = cid,
                CustomerId = contract.CustomerId
            };
            _DailyLoanContext.Transaction.Add(t);
            contract.TotalPay = contract.TotalPay + amount;
            contract.UpdateBy = uid;
            contract.UpdateDate = DateTime.Now;
            var isDone = await _DailyLoanContext.SaveChangesAsync()>0;
            var customer = await _DailyLoanContext.Customer.FindAsync(contract.CustomerId);
            var alltran = await _DailyLoanContext.Transaction.Where(x => x.ContractId == cid&&x.Type==TransactionType_Status.CollectFromCustomer).OrderByDescending(x=>x.CreateDate).ToListAsync();
            int notpay = 0, paypartial = 0;
            if (alltran.Count >= 3)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (alltran[i].Amount < customer.Installment / 2) paypartial++;
                    if (alltran[i].Amount == 0) notpay++;
                }
                if (notpay == 3 && (contract.Status == ContractStatus.StatusContract_Normal || contract.Status == ContractStatus.StatusContract_Loss))
                {
                    Notification n = new Notification()
                    {
                        Message = Notification_Message.NotPay,
                        Status = Notification_Status.Alert,
                        Type = Notification_Type.NotPay,
                        CreateBy = uid,
                        CreateDate = DateTime.Now,
                        ContractId = cid,
                        CustomerLineId = customer.CustomerLineId,
                        HouseId = GetHouseIdByUserId(uid)
                    };
                    _DailyLoanContext.Notification.Add(n);
                    contract.Status = ContractStatus.StatusContract_Alert;
                    customer.Status = CustomerStatus.StatusCustomer_Bad;
                    return await _DailyLoanContext.SaveChangesAsync() > 0;
                }
                else if (paypartial == 3 && (contract.Status == ContractStatus.StatusContract_Normal || contract.Status == ContractStatus.StatusContract_Loss))
                {
                    Notification n = new Notification()
                    {
                        Message = Notification_Message.NotPay,
                        Status = Notification_Status.Alert,
                        Type = Notification_Type.NotPay,
                        CreateBy = uid,
                        CreateDate = DateTime.Now,
                        ContractId = cid,
                        CustomerLineId = customer.CustomerLineId,
                        HouseId = GetHouseIdByUserId(uid)
                    };
                    _DailyLoanContext.Notification.Add(n);
                    contract.Status = ContractStatus.StatusContract_Alert;
                    customer.Status = CustomerStatus.StatusCustomer_Bad;
                    return await _DailyLoanContext.SaveChangesAsync() > 0;
                }
                else
                    return isDone;
            }
            else
                return isDone;
        }
        #endregion
        #region Warn
        public async Task<List<ManagementWarn>> GetAllWarn(int uid, int useraccess)
        {
            List<ManagementWarn> rtn = await (from n in _DailyLoanContext.Notification
                                                  join us in _DailyLoanContext.StatusNotification on n.Status equals us.Id
                                              //where n.Status != Notification_Status.Checked
                                              select new ManagementWarn()
                                                  {
                                                      Id = n.Id,
                                                      ContractId = n.ContractId,
                                                      CustomerLineId = n.CustomerLineId,
                                                      HouseId = n.HouseId,
                                                      Message = n.Message,
                                                      Status = n.Status,
                                                      Type = n.Type,
                                                      Remark = n.Remark,
                                                      CreateBy = n.CreateBy,
                                                      CreateDate = n.CreateDate,
                                                      UpdateBy = n.UpdateBy,
                                                      UpdateDate = n.UpdateDate,
                                                      StatusText = us.Status,
                                                  }).ToListAsync();
            for (int i = 0; i < rtn.Count(); i++)
            {
                ManagementHistory tt = new ManagementHistory(GetContract(rtn[i].ContractId.Value));
                tt.History = await GetPayHistory(tt.Id);
                rtn[i].Contract = tt;
            }
            var hid = GetHouseIdByUserId(uid);
            if (useraccess == StatusUserAccess.UserAccess_Admin)
            {
                rtn = rtn.Where(x => x.Contract.Customer.HouseId == hid).ToList();
            }
            else if (useraccess == StatusUserAccess.UserAccess_Audit)
            {
                rtn = rtn.Where(x => x.Contract.Customer.HouseId == hid&&x.Status != ContractStatus.StatusContract_Checked).ToList();
            }
            return rtn;
        }
        public async Task<ManagementWarn> GetWarn(int nid)
        {
            ManagementWarn rtn = await (from n in _DailyLoanContext.Notification
                                              join us in _DailyLoanContext.StatusNotification on n.Status equals us.Id
                                              where n.Id == nid
                                              select new ManagementWarn()
                                              {
                                                  Id = n.Id,
                                                  ContractId = n.ContractId,
                                                  CustomerLineId = n.CustomerLineId,
                                                  HouseId = n.HouseId,
                                                  Message = n.Message,
                                                  Status = n.Status,
                                                  Type = n.Type,
                                                  Remark = n.Remark,
                                                  CreateBy = n.CreateBy,
                                                  CreateDate = n.CreateDate,
                                                  UpdateBy = n.UpdateBy,
                                                  UpdateDate = n.UpdateDate,
                                                  StatusText = us.Status,
                                              }).FirstOrDefaultAsync();

            ManagementHistory tt = new ManagementHistory(GetContract(rtn.ContractId.Value));
            tt.History = await GetPayHistory(tt.Id);
            rtn.Contract = tt;
            return rtn;
        }
        public async Task<bool> DeleteNotification(int nid)
        {
            _DailyLoanContext.Notification.Remove(_DailyLoanContext.Notification.Where(x => x.Id == nid).FirstOrDefault());
            return (await _DailyLoanContext.SaveChangesAsync()) > 0;
        }
        public async Task<bool> EditNotification(int id,string remark,int status,int uid)
        {
            int notstatus = status == ContractStatus.StatusContract_Normal || status == ContractStatus.StatusContract_Loss || status == ContractStatus.StatusContract_Dead ? ContractStatus.StatusContract_Checked : status;
            var notice = await _DailyLoanContext.Notification.FindAsync(id);
            if (notice == null)
            {
                return await Task.FromResult(false);
            }
            notice.Remark = remark;
            notice.Status = notstatus;
            notice.UpdateBy = uid;
            notice.UpdateDate = DateTime.Now;

            var cus = await _DailyLoanContext.Contract.FindAsync(notice.ContractId);
            cus.Status = status;

            return (await _DailyLoanContext.SaveChangesAsync()) > 0;
        }
        #endregion
    }
}
