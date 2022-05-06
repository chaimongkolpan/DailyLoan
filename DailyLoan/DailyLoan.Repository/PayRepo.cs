
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
                var hid = GetHouseIdByUserId(uid);
                tmp = tmp.Where(x => x.HouseId == hid).ToList();
                tmp1 = tmp1.Where(x => x.Customer.HouseId == hid).ToList();
            }
            if (useraccess == StatusUserAccess.UserAccess_Agent)
            {
                var clid = GetCustomerLineIdByUserId(uid);
                tmp = tmp.Where(x => x.CustomerLineId == clid).ToList();
                tmp1 = tmp1.Where(x => x.Customer.CustomerLineId == clid).ToList();
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
            var rtn = _DailyLoanContext.User.Where(x => x.Id == uid).FirstOrDefault();
            return rtn==null?1:rtn.HouseId;
        }
        public int GetHouseIdByCustomerLine(int clid)
        {
            return ((int)_DailyLoanContext.CustomerLine.Where(x => x.Id == clid).FirstOrDefault().HouseId);
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
        public async Task<double> GetTotalCollectByCustomerLine(int clid,int m,int y)
        {
            var rtn = await _DailyLoanContext.Transaction.Where(x => x.CustomerLineId == clid
                                                                && x.PayDate.Value.Month == m
                                                                && x.PayDate.Value.Year == y
                                    &&(x.Type==TransactionType_Status.CollectFromCustomer
                                    || x.Type == TransactionType_Status.CollectFromCustomerCut 
                                    || x.Type == TransactionType_Status.SaveCollectFromCustomer)).ToListAsync();
            return (rtn.Count == 0 ? 0 : (double)rtn.Select(x => x.Amount).Sum());
        }
        public async Task<int> GetTotalWorkdayByUserId(int uid, int m, int y)
        {
            int clid = GetCustomerLineIdByUserId(uid);
            var rtn = await _DailyLoanContext.DailyCost.Where(x => x.CustomerLineId == clid
                                                                &&(x.Agent1==uid||x.Agent2==uid)
                                                                &&x.Date.Value.Month==m
                                                                &&x.Date.Value.Year==y).ToListAsync();
            return rtn.Count;
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
            var rtn = _DailyLoanContext.Contract.Where(x => x.CustomerId == cid&&x.Status != ContractStatus.StatusContract_Closed
            && x.Status != ContractStatus.StatusContract_NotApprove).FirstOrDefault();
            return (rtn != null);
        }
        public string getConfig(string name,int uid)
        {
            var rtn = _DailyLoanContext.Config.Where(x => x.Name == name && x.HouseId == GetHouseIdByUserId(uid)).FirstOrDefault().Value;
            return rtn;
        }
        public string getConfigByHouse(string name, int hid)
        {
            var rtn = _DailyLoanContext.Config.Where(x => x.Name == name && x.HouseId == hid).FirstOrDefault().Value;
            return rtn;
        }
        public bool CheckSpecialTime(int uid,DateTime date)
        {
            uid = 1;
            var rtn = _DailyLoanContext.SpecialRate.Where(x => x.HouseId == GetHouseIdByUserId(uid) && x.StartDate<=date &&x.EndDate >= date).FirstOrDefault();
            return rtn != null;
        }
        public bool IsCollectToday(int cid)
        {
            var rtn = _DailyLoanContext.Transaction.Where(x => x.ContractId == cid && x.PayDate.Value == DateTime.Now.Date ).FirstOrDefault();
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
                                                                && x.Status!=ContractStatus.StatusContract_NotApprove
                                                                && x.Status!=ContractStatus.StatusContract_Closed
                                                                && x.Status!=ContractStatus.StatusContract_WaitConfirm)
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
                //var customer = await _DailyLoanContext.Customer.FindAsync(req.CustomerId);
                //customer.Installment = 0;
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
                cus.Remark = req.Remark;
                //cus.SpecialRateCount = req.SpecialRateCount;
                //cus.CutCount = req.CutCount;
                cus.UpdateBy = req.UpdateBy;
                cus.UpdateDate = req.UpdateDate;
                if (req.Status == ContractStatus.StatusContract_Closed)
                {
                    var allnoti = await _DailyLoanContext.Notification.Where(x => x.ContractId == req.Id).ToListAsync();
                    cus.CloseDate = DateTime.Now;
                    for (int i = 0; i < allnoti.Count; i++)
                    {
                        allnoti[i].Status = Notification_Status.Checked;
                        allnoti[i].Remark = allnoti[i].Remark + "(ปิดสัญญา)";
                        allnoti[i].Status = Notification_Status.Checked;
                        allnoti[i].UpdateDate = DateTime.Now;
                        allnoti[i].UpdateBy = req.UpdateBy;
                    }
                }
            }
            return (await _DailyLoanContext.SaveChangesAsync()) > 0;
        }
        public async Task<bool> Approve(int cid,int uid)
        {
            var checktran = await _DailyLoanContext.Transaction.Where(x=>x.ContractId == cid).ToListAsync();
            if(checktran.Count == 0)
            {
                var req = await _DailyLoanContext.Contract.FindAsync(cid);
                req.Status = ContractStatus.StatusContract_Normal;
                req.ApproverId = uid;
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
                                                                    && x.Status != ContractStatus.StatusContract_Closed
                                                                    && x.Status != ContractStatus.StatusContract_WaitConfirm)
                                                                .OrderByDescending(x => x.Id).FirstOrDefault();
                //double oldamount = (double)customer.Installment * 100 / hrateins;
                //double oldprofit = (double)customer.Installment * profit / hrateins;
                double oldamount = oldcontract == null ? 0 : (double)oldcontract.TotalAmount;
                double oldprofit = oldcontract == null ? 0 : (double)oldcontract.TotalAmount * profit / 100;

                customer.Installment = hrateins * req.TotalAmount / 100;
                req.Installment = hrateins * req.TotalAmount / 100;
                req.CustomerLineId = customer.CustomerLineId;
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
                var allnoti = new List<Notification>();
                for(int i = 0; i < allopen.Count; i++)
                {
                    allopen[i].Status = ContractStatus.StatusContract_Closed;
                    allopen[i].CloseDate = DateTime.Now;
                    allopen[i].UpdateDate = DateTime.Now;
                    allopen[i].UpdateBy = uid;
                    var tmp = await _DailyLoanContext.Notification.Where(x => x.ContractId == allopen[i].Id).ToListAsync();
                    if(tmp.Count > 0)
                        allnoti.AddRange(tmp);
                }
                for (int j = 0; j < allnoti.Count; j++)
                {
                    allnoti[j].Remark = allnoti[j].Remark + "(ปิดสัญญา)";
                    allnoti[j].Status = Notification_Status.Checked;
                    allnoti[j].UpdateDate = DateTime.Now;
                    allnoti[j].UpdateBy = uid;
                }
                return (await _DailyLoanContext.SaveChangesAsync()) > 0;
            }
            else
            {
                return true;
            }
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
                total += (double)(cost.OtherIncome == null ? 0 : cost.OtherIncome);
            }

            var collect = await (from con in _DailyLoanContext.Contract
                                 where con.CustomerLineId == clid
                                 && con.Status != ContractStatus.StatusContract_Closed
                                 && con.Status != ContractStatus.StatusContract_WaitConfirm
                                 && con.Status != ContractStatus.StatusContract_NotApprove
                                 select con).ToListAsync();
            if(data != null)
            {
                double bounty = (double)data.Where(x => x.Type == TransactionType_Status.Bounty).Select(x => x.Amount).Sum();
                double allowance = bounty >= _appsettingModel.Allowance ? 0 : (_appsettingModel.Allowance - bounty);
                allowance = cost==null?0:cost.Allowance==null?0:(double)cost.Allowance;
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
                    housepayout = cost == null ? 0 : cost.PayOut == null ? 0 : cost.PayOut,
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
        public async Task<DailyReportResponse> GetMustReturnDaily(int clid, DateTime date)
        {
            var cost = _DailyLoanContext.DailyCost.Where(x => x.CustomerLineId == clid && x.Date.Value.Date == date.Date).FirstOrDefault();

            var result = new DailyReportResponse();
            var data = await (from tran in _DailyLoanContext.Transaction.Where(x => x.CreateDate.Date == date.Date && x.CustomerLineId == clid)
                              select tran).ToListAsync();
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
                total += (double)(cost.OtherIncome == null ? 0 : cost.OtherIncome);
            }

            var collect = await (from con in _DailyLoanContext.Contract
                                 where con.CustomerLineId == clid
                                 && con.Status != ContractStatus.StatusContract_Closed
                                 && con.Status != ContractStatus.StatusContract_WaitConfirm
                                 && con.Status != ContractStatus.StatusContract_NotApprove
                                 select con).ToListAsync();
            if (data != null)
            {
                double bounty = (double)data.Where(x => x.Type == TransactionType_Status.Bounty).Select(x => x.Amount).Sum();
                double allowance = bounty >= _appsettingModel.Allowance ? 0 : (_appsettingModel.Allowance - bounty);
                allowance = cost==null?0:cost.Allowance==null?0:(double)cost.Allowance;
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
                    housepayout = cost == null ? 0 : cost.PayOut == null ? 0 : cost.PayOut,
                    paytocustomer = paytocustomer,
                    collect = collectfrom,
                    collectall = data.Where(x => x.Type == TransactionType_Status.CollectFromCustomer || x.Type == TransactionType_Status.CollectFromCustomerCut || x.Type == TransactionType_Status.SaveCollectFromCustomer).Select(x => x.Amount).Sum(),
                    mustcollect = collect.Select(x => x.Installment).Sum(),
                    mustreturn = total,
                    sumexpense = sumexpense
                };
                result.transactions = new List<collectTransaction>();
                var more = new List<collectTransaction>();
                var zero = new List<collectTransaction>();
                int cutcount = 0, opencount = 0;
                double cutamount = 0.0, openamount = 0.0;
                var hrateins = Convert.ToDouble(getConfigByHouse("HouseRate", GetHouseIdByCustomerLine(clid)));
                for (int i = 0; i < data.Count; i++)
                {
                    if(data[i].Type == TransactionType_Status.CollectFromCustomerCut|| data[i].Type == TransactionType_Status.CollectFromCustomer)
                    {
                        collectTransaction tmp = new collectTransaction();
                        var cont = GetContract(data[i].ContractId);
                        var cus = GetCustomer(cont.CustomerId);
                        var gua = GetCustomer(cont.GuarantorId);
                        tmp.customername = cus.Firstname + " " + cus.Lastname;
                        tmp.guarantorname = gua.Firstname + " " + gua.Lastname;
                        tmp.address = cus.Address;
                        tmp.mustcollect = cont.TotalAmount*hrateins/100;
                        tmp.type = data[i].Type==TransactionType_Status.CollectFromCustomerCut?(int)cont.Type:0;
                        tmp.collect = data[i].Amount;
                        if (data[i].Amount > 0) more.Add(tmp);
                        else zero.Add(tmp);
                        //result.transactions.Add(tmp);
                        /*
                        if(data[i].Type == TransactionType_Status.CollectFromCustomerCut&& (int)cont.Type != 1)
                        {
                            cutcount++;
                            cutamount += (double)data[i].Amount;
                        }
                        if (data[i].Type == TransactionType_Status.CollectFromCustomerCut && (int)cont.Type == 1)
                        {
                            opencount++;
                            openamount += (double)data[i].Amount;
                        }*/
                    }
                    if (data[i].Type == TransactionType_Status.PayToCustomer)
                    {
                        var cont = GetContract(data[i].ContractId);
                        if ((int)cont.Type != 1)
                        {
                            cutcount++;
                            cutamount += (double)data[i].Amount;
                        }
                        if ((int)cont.Type == 1)
                        {
                            opencount++;
                            openamount += (double)data[i].Amount;
                        }
                    }
                }

                if (more.Count > 0) result.transactions.AddRange(more.OrderBy(x => x.address).ThenBy(x => x.customername).ToList());
                if (zero.Count > 0) result.transactions.AddRange(zero.OrderBy(x => x.address).ThenBy(x => x.customername).ToList());
                //result.transactions = result.transactions.OrderBy(x => x.address).ThenBy(x => x.customername).ToList();
                result.cutcount = cutcount;
                result.cutamount = cutamount;
                result.opencount = opencount;
                result.openamount = openamount;
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
            if (req.MustCollect == null) req.MustCollect = 0;
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
                if (req.MustCollect != null) tmp.MustCollect = req.MustCollect;
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
        public async Task<bool> SaveDailyCostAgent(DailyCost req, int uid)
        {
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
            if (req.MustCollect == null) req.MustCollect = 0;
            if (req.Other == null) req.Other = 0;
            if (req.OtherDetail == null) req.OtherDetail = "";
            if (req.Remark == null) req.Remark = "";
            if (req.OtherIncome == null) req.OtherIncome = 0;
            if (req.OtherIncomeRemark == null || req.OtherIncomeRemark == "undefined") req.OtherIncomeRemark = "";
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
                //if (req.PayOut != null) tmp.PayOut = req.PayOut;
                //if (req.Receive != null) tmp.Receive = req.Receive;
                //if (req.Allowance != null) tmp.Allowance = req.Allowance;
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
                if (req.MustCollect != null) tmp.MustCollect = req.MustCollect;
                if (req.Other != null) tmp.Other = req.Other;
                if (req.OtherDetail != null) tmp.OtherDetail = req.OtherDetail;
                if (req.Remark != null) tmp.Remark = req.Remark;
                if (req.OtherIncome != null) tmp.OtherIncome = req.OtherIncome;
                if (req.OtherIncomeRemark != null) tmp.OtherIncomeRemark = req.OtherIncomeRemark;
            }
            return (await _DailyLoanContext.SaveChangesAsync()) > 0;
        }
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
                total += (double)(cost.OtherIncome == null ? 0 : cost.OtherIncome);
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
                allowance = cost==null?0:cost.Allowance==null?0:(double)cost.Allowance;
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
                    housepayout = cost == null?0: cost.PayOut == null ? 0 : cost.PayOut,
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
            var data = await (from tran in _DailyLoanContext.Transaction.Where(x => (x.Type == TransactionType_Status.CollectFromCustomer|| x.Type == TransactionType_Status.CollectFromCustomerCut || x.Type == TransactionType_Status.SaveCollectFromCustomer) && x.ContractId == cid)
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
        public async Task<List<ManagementContract>> SearchContractCollect(int uid, string idcard, string firstname, string lastname, string address)
        {
            List<ManagementContract> rtn = await (from c in _DailyLoanContext.Contract.Where(x=>
                                                            x.Status != ContractStatus.StatusContract_Closed
                                                            && x.Status != ContractStatus.StatusContract_WaitConfirm
                                                            && x.Status != ContractStatus.StatusContract_NotApprove)
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
                rtn[i].iscollect = IsCollectToday(rtn[i].Id);
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
            var profit = Convert.ToDouble(getConfig("TotalProfit", uid));
            contract.TotalPay = contract.TotalPay + amount;
            contract.Status = contract.TotalPay >= contract.TotalAmount * ((100 + profit) / 100) ? ContractStatus.StatusContract_Closed : contract.Status;
            contract.UpdateBy = uid;
            contract.UpdateDate = DateTime.Now;
            if(contract.Status == ContractStatus.StatusContract_Closed)
            {
                contract.CloseDate = DateTime.Now;
                var allnoti = await _DailyLoanContext.Notification.Where(x => x.ContractId == cid).ToListAsync();
                for(int i = 0; i < allnoti.Count; i++)
                {
                    allnoti[i].Status = Notification_Status.Checked;
                }
            }
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
                //if (notpay == 3 && (contract.Status == ContractStatus.StatusContract_Normal || contract.Status == ContractStatus.StatusContract_Loss))
                if (notpay == 3 && (contract.Status == ContractStatus.StatusContract_Normal))
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
                //else if (paypartial == 3 && (contract.Status == ContractStatus.StatusContract_Normal || contract.Status == ContractStatus.StatusContract_Loss))
                else if (paypartial == 3 && (contract.Status == ContractStatus.StatusContract_Normal))
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
            rtn = rtn.OrderBy(x => x.Status).ThenBy(x => x.Contract.Customer.Address).ThenBy(x => x.Contract.Customer.Firstname).ThenBy(x => x.Contract.Customer.Lastname).ToList();
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
            if(cus.Status != ContractStatus.StatusContract_Closed&& cus.Status != ContractStatus.StatusContract_Dead)
                cus.Status = status;
            return (await _DailyLoanContext.SaveChangesAsync()) > 0;
        }
        #endregion
        #region monthly
        public async Task<MonthlyInput> GetMonthlyCost(int m, int y,int hid,int uid)
        {
            if(hid == 0) { hid = GetHouseIdByUserId(uid); }
            MonthlyInput rtn = await (from n in _DailyLoanContext.MonthlyCost.Where(x => x.Month==m&&x.Year==y&&x.HouseId==hid)
                                        select new MonthlyInput()
                                        {
                                            Id = n.Id,
                                            HouseId = n.HouseId,
                                            Month = n.Month,
                                            Year = n.Year,
                                            HouseRent = n.HouseRent,
                                            Water = n.Water,
                                            Electric = n.Electric,
                                            Internet = n.Internet,
                                            PaperInk = n.PaperInk,
                                            GodFee = n.GodFee,
                                            Banquet = n.Banquet,
                                            BanquetRemark = n.BanquetRemark,
                                            VehicleCost = n.VehicleCost,
                                            VehicleRemark = n.VehicleRemark,
                                            Other = n.Other,
                                            OtherRemark = n.OtherRemark,
                                            Remark = n.Remark,
                                            CreateBy = n.CreateBy,
                                            CreateDate = n.CreateDate,
                                            UpdateBy = n.UpdateBy,
                                            UpdateDate = n.UpdateDate
                                        }).FirstOrDefaultAsync();
            if(rtn == null)
            {
                rtn = new MonthlyInput()
                {
                    HouseId = hid,
                    Month = m,
                    Year = y,
                    HouseRent = 0,
                    Water = 0,
                    Electric = 0,
                    Internet = 0,
                    PaperInk = 0,
                    GodFee = 0,
                    Banquet = 0,
                    BanquetRemark = "",
                    VehicleCost = 0,
                    VehicleRemark = "",
                    Other = 0,
                    OtherRemark = "",
                    Remark = "",
                    Users = new List<UserSalary>()
                };
            }
            List<UserSalary> users = await (from s in _DailyLoanContext.Salary.Where(x => x.Month == m && x.Year == y && x.HouseId == hid)
                                            join u in _DailyLoanContext.User on s.UserId equals u.Id
                                            join ua in _DailyLoanContext.UserAccess on u.UserAccess equals ua.Id
                                            select new UserSalary()
                                            {
                                                HouseId = s.HouseId,
                                                UserId = s.UserId,
                                                Name = u.Firstname + " " + u.Lastname + (String.IsNullOrEmpty(u.Nickname)?"":"("+u.Nickname+")"),
                                                Useraccess = u.UserAccess,
                                                UseraccessText = ua.UserAccess1,
                                                Salary = s.Salary1.Value==0?0: s.Salary1.Value,
                                                Performance = s.Performance.Value
                                            }).ToListAsync();
            if(users.Count == 0)
            {
                users = await (from u in _DailyLoanContext.User.Where(x => x.HouseId == hid)
                                join ua in _DailyLoanContext.UserAccess on u.UserAccess equals ua.Id
                                select new UserSalary()
                                {
                                    HouseId = u.HouseId,
                                    UserId = u.Id,
                                    Name = u.Firstname + " " + u.Lastname + (String.IsNullOrEmpty(u.Nickname) ? "" : "(" + u.Nickname + ")"),
                                    Useraccess = u.UserAccess,
                                    UseraccessText = ua.UserAccess1,
                                    Salary = 0,
                                    Performance = 0
                                }).ToListAsync();
            }
            var data = await (from tran in _DailyLoanContext.Transaction.Where(x => x.CustomerLineId == 1
                                            && x.CreateDate.Date == DateTime.Now.Date)
                                select tran).ToListAsync();
            for(int i = 0;i < users.Count; i++)
            {
                if(users[i].Useraccess == StatusUserAccess.UserAccess_Agent)
                {
                    users[i].Salary = users[i].Salary == 0 ? (double)data.Where(x => x.Type == TransactionType_Status.CollectFromCustomer
                                                    || x.Type == TransactionType_Status.CollectFromCustomerCut
                                                    || x.Type == TransactionType_Status.SaveCollectFromCustomer).Select(x => x.Amount).Sum() : users[i].Salary;
                    int wd = await GetTotalWorkdayByUserId(users[i].UserId,m,y);
                    users[i].Workdays = wd==0?DateTime.DaysInMonth(y, m):wd;
                    users[i].TotalCollect = await GetTotalCollectByCustomerLine(GetCustomerLineIdByUserId(users[i].UserId),m,y);
                }
            }
            rtn.Users = users;
            return rtn;
        }
        public async Task<bool> SaveMonthlyCost(MonthlyCost req)
        {
            if (req.HouseId == 0) { req.HouseId = GetHouseIdByUserId(req.CreateBy); }
            if (req.HouseRent == null) req.HouseRent = 0;
            if (req.Water == null) req.Water = 0;
            if (req.Electric == null) req.Electric = 0;
            if (req.Internet == null) req.Internet = 0;

            if (req.PaperInk == null) req.PaperInk = 0;
            if (req.GodFee == null) req.GodFee = 0;

            if (req.Banquet == null) req.Banquet = 0;
            if (req.BanquetRemark == null || req.BanquetRemark == "undefined") req.BanquetRemark = "";
            if (req.VehicleCost == null) req.VehicleCost = 0;
            if (req.VehicleRemark == null || req.VehicleRemark == "undefined") req.VehicleRemark = "";

            if (req.Other == null) req.Other = 0;
            if (req.OtherRemark == null || req.OtherRemark == "undefined") req.OtherRemark = "";
            if (req.Remark == null || req.Remark == "undefined") req.Remark = "";
            var tmp = _DailyLoanContext.MonthlyCost.Where(x => x.HouseId == req.HouseId && x.Month == req.Month && x.Year == req.Year).FirstOrDefault();
            if (tmp == null)
            {
                _DailyLoanContext.MonthlyCost.Add(req);
            }
            else
            {
                tmp.HouseRent = req.HouseRent;
                tmp.Water = req.Water;
                tmp.Electric = req.Electric;
                tmp.Internet = req.Internet;

                tmp.PaperInk = req.PaperInk;
                tmp.GodFee = req.GodFee;

                tmp.Banquet = req.Banquet;
                tmp.BanquetRemark = req.BanquetRemark;
                tmp.VehicleCost = req.VehicleCost;
                tmp.VehicleRemark = req.VehicleRemark;

                tmp.Other = req.Other;
                tmp.OtherRemark = req.OtherRemark;
                tmp.Remark = req.Remark;
                tmp.UpdateBy = req.UpdateBy;
                tmp.UpdateDate = req.UpdateDate;
            }
            return (await _DailyLoanContext.SaveChangesAsync()) > 0;
        }
        public async Task<bool> SaveSalary(Salary req)
        {
            if (req.HouseId == 0) { req.HouseId = GetHouseIdByUserId(req.CreateBy); }
            if (req.Salary1 == null) req.Salary1 = 0;
            if (req.Performance == null) req.Performance = 0;
            if (req.Remark == null || req.Remark == "undefined") req.Remark = "";

            var tmp = _DailyLoanContext.Salary.Where(x => x.HouseId == req.HouseId && x.Month == req.Month && x.Year == req.Year&&x.UserId == req.UserId).FirstOrDefault();
            if (tmp == null)
            {
                _DailyLoanContext.Salary.Add(req);
            }
            else
            {
                tmp.Salary1 = req.Salary1;
                tmp.Performance = req.Performance;
                tmp.Remark = req.Remark;
                
                tmp.UpdateBy = req.UpdateBy;
                tmp.UpdateDate = req.UpdateDate;
            }
            return (await _DailyLoanContext.SaveChangesAsync()) > 0;
        }
        public async Task<MonthlyReport> GetMonthlyReport(DateTime start, DateTime end, int hid, int uid)
        {
            if (hid == 0) { hid = GetHouseIdByUserId(uid); }
            var house = _DailyLoanContext.House.Where(x => x.Id == hid).FirstOrDefault();
            string name = "-";
            if (house != null) name = house.HouseName;
            double salary = 0, houserent = 0, water = 0, electric = 0, internet = 0, paperink = 0, godfee = 0, banquet = 0, vehicle = 0, other = 0, total = 0;
            var housedata = await _DailyLoanContext.MonthlyCost.Where(x => x.HouseId==hid
                        && x.Date.Value >= new DateTime(start.Year,start.Month,1)
                        && x.Date.Value <= new DateTime(end.Year, end.Month, 1)).ToListAsync();
            
            if (housedata.Count > 0)
            {
                houserent = (double)housedata.Select(x => x.HouseRent).Sum();
                water = (double)housedata.Select(x => x.Water).Sum();
                electric = (double)housedata.Select(x => x.Electric).Sum();
                internet = (double)housedata.Select(x => x.Internet).Sum();
                paperink = (double)housedata.Select(x => x.PaperInk).Sum();
                godfee = (double)housedata.Select(x => x.GodFee).Sum();
                banquet = (double)housedata.Select(x => x.Banquet).Sum();
                vehicle = (double)housedata.Select(x => x.VehicleCost).Sum();
                other = (double)housedata.Select(x => x.Other).Sum();
                total = houserent + water + electric + internet + paperink + godfee + banquet + vehicle + other;
            }

            /*
            var salarydata = await _DailyLoanContext.Salary.Where(x => x.HouseId == hid
                        && x.Date.Value >= new DateTime(start.Year, start.Month, 1)
                        && x.Date.Value <= new DateTime(end.Year, end.Month, 1)).ToListAsync();
            */
            List<UserSalary> users = await (from s in _DailyLoanContext.Salary.Where(x => x.HouseId == hid 
                                            && x.Date.Value >= new DateTime(start.Year, start.Month, 1)
                                            && x.Date.Value <= new DateTime(end.Year, end.Month, 1))
                                            join u in _DailyLoanContext.User on s.UserId equals u.Id
                                            select new UserSalary()
                                            {
                                                UserId = s.UserId,
                                                Useraccess = u.UserAccess,
                                                Salary = s.Salary1.Value+ s.Performance.Value
                                            }).ToListAsync();
            Dictionary<string, double> clsalary = new Dictionary<string, double>();
            if (users.Count > 0)
            {
                for (int i = 0; i < users.Count; i++)
                {
                    if (users[i].Useraccess == StatusUserAccess.UserAccess_Agent)
                    {
                        var clid = GetCustomerLineIdByUserId(users[i].UserId);
                        if (!clsalary.ContainsKey(clid.ToString())) clsalary.Add(clid.ToString(), 0);
                        clsalary[clid.ToString()] += users[i].Salary;
                    }
                    else
                    {
                        salary += users[i].Salary;
                    }
                }
            }

            var customerlines = new List<CustomerLineMonthly>();

            var cl = await _DailyLoanContext.CustomerLine.Where(x => x.HouseId == hid).ToListAsync();
            for (int i = 0; i < cl.Count; i++)
            {
                int clid = cl[i].Id;
                var cost = await _DailyLoanContext.DailyCost.Where(x => x.CustomerLineId == clid
                                            && x.Date.Value >= start
                                            && x.Date.Value <= end).ToListAsync();
                var data = await (from tran in _DailyLoanContext.Transaction.Where(x => x.CustomerLineId == clid 
                                            && x.CreateDate.Date >= start
                                            && x.CreateDate.Date <= end)
                                  select tran).ToListAsync();
                double incToday = 0;
                var contractIn = await (from con in _DailyLoanContext.Contract
                                         where con.CustomerLineId == clid
                                         && con.Status != ContractStatus.StatusContract_WaitConfirm
                                         && con.Status != ContractStatus.StatusContract_NotApprove
                                            select con).ToListAsync();
                if(end >= DateTime.Now.Date)
                {
                    incToday += (double)contractIn.Where(x=>x.Status != ContractStatus.StatusContract_Closed).Select(x => x.Installment).Sum();
                }

                double paytocustomer = (double)data.Where(x => x.Type == TransactionType_Status.PayToCustomer).Select(x => x.Amount).Sum(),
                    mustcollect = (double)cost.Select(x => x.MustCollect).Sum() + incToday,
                    collect = (double)data.Where(x => x.Type == TransactionType_Status.CollectFromCustomer
                                                    || x.Type == TransactionType_Status.CollectFromCustomerCut
                                                    || x.Type == TransactionType_Status.SaveCollectFromCustomer).Select(x => x.Amount).Sum(),
                    loss = 0,
                    bounty = (double)data.Where(x => x.Type == TransactionType_Status.Bounty).Select(x => x.Amount).Sum(),
                    otherincome = (double)cost.Select(x=>x.OtherIncome).Sum(),
                    allowance = (double)cost.Select(x => x.Allowance).Sum(),
                    bike = (double)cost.Select(x => x.BikeMaintenance).Sum(),
                    gas = (double)cost.Select(x => x.Gas).Sum(),
                    topup = (double)cost.Select(x => x.Topup).Sum(),
                    police = (double)cost.Select(x => x.Police1+x.Police2+x.Police3).Sum(),
                    caught = (double)cost.Select(x => x.Caught).Sum(),
                    othercl = (double)cost.Select(x => x.Other).Sum(),
                    totalexpenses = 0,
                    salary1 = clsalary.ContainsKey(clid.ToString())?clsalary[clid.ToString()]:0,
                    salary2 = 0,//from salary ********************************************
                    cut = contractIn.Where(x=>x.Type==Request_Type.Cut|| x.Type == Request_Type.CutDecrease|| x.Type == Request_Type.CutIncrease
                                && x.CreateDate.Date >= start && x.CreateDate.Date <= end).Count(),
                    open = contractIn.Where(x => x.Type == Request_Type.NewContract && x.CreateDate.Date >= start && x.CreateDate.Date <= end).Count(),
                    close = contractIn.Where(x => x.Status == ContractStatus.StatusContract_Closed && x.CloseDate >= start && x.CloseDate <= end).Count();
                loss = mustcollect - collect;
                totalexpenses += bike + gas + topup + police + caught + othercl;
                customerlines.Add(new CustomerLineMonthly()
                {
                    Name = cl[i].CustomerLineName,
                    paytocustomer = paytocustomer,
                    mustcollect = mustcollect,
                    collect = collect,
                    loss = loss,
                    bounty = bounty,
                    otherincome = otherincome,
                    allowance = allowance,
                    bike = bike,
                    gas = gas,
                    topup = topup,
                    police = police,
                    caught = caught,
                    other = othercl,
                    totalexpenses = totalexpenses,
                    salary1 = salary1,
                    salary2 = salary2,
                    totalemploy = allowance + bounty + salary1 + salary2,
                    profit = collect + otherincome - paytocustomer,
                    cut = cut,
                    open = open,
                    close = close,
                    grandtotal = (collect + otherincome - paytocustomer) - totalexpenses - (allowance + bounty + salary1 + salary2)
                });
            }

            var rtn = await Task.FromResult(new MonthlyReport()
            {
                Name = name,
                start = start,
                end = end,
                salary = salary,
                houserent = houserent,
                water = water,
                electric = electric,
                internet = internet,
                paperink = paperink,
                godfee = godfee,
                banquet = banquet,
                vehicle = vehicle,
                other = other,
                totalhouseexpenses = total,

                customerlines = customerlines
            });
            return rtn; 
        }
        #endregion
    }
}
