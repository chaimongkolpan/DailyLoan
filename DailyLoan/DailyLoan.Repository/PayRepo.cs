
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

namespace DailyLoan.Repository
{
    public class PayRepo : IPayRepo
    {
        private readonly DailyLoanContext _DailyLoanContext;
        private readonly AppsettingModel _appsettingModel;
        public PayRepo(DailyLoanContext dailyLoanContext,
            AppsettingModel appsettingModel)
        {
            _DailyLoanContext = dailyLoanContext;
            _appsettingModel = appsettingModel;
        }
        #region getValue
        public int GetHouseIdByUserId(int uid)
        {
            return (_DailyLoanContext.User.Where(x => x.Id == uid).FirstOrDefault().HouseId);
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
        public async Task<List<ManagementCustomer>> SearchCustomer(string idcard,string name)
        {
            if (String.IsNullOrEmpty(idcard)) idcard = "";
            if (String.IsNullOrEmpty(name)) name = "";
            List<ManagementCustomer> rtn = await (from c in _DailyLoanContext.Customer
                                      join us in _DailyLoanContext.StatusCustomer on c.Status equals us.Id
                                      join cl in _DailyLoanContext.CustomerLine on c.CustomerLineId equals cl.Id
                                      join h in _DailyLoanContext.House on cl.HouseId equals h.Id
                                      where c.Idcard.Contains(idcard) || c.Firstname.Contains(name) || c.Lastname.Contains(name) || c.Nickname.Contains(name)
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
                                          CreateBy = c.CreateBy,
                                          CreateDate = c.CreateDate,
                                          UpdateBy = c.UpdateBy,
                                          UpdateDate = c.UpdateDate,
                                          StatusText = us.Status,
                                          HouseId = h.Id,
                                          HouseText = h.HouseName,
                                          CustomerLineText = cl.CustomerLineName
                                      }).ToListAsync();
            return rtn;
        }
        public bool isExistContract(int cid)
        {
            var rtn = _DailyLoanContext.Contract.Where(x => x.CustomerId == cid&&x.Status != ContractStatus.StatusContract_Closed).FirstOrDefault();
            return (rtn != null);
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
                                                      CreateBy = c.CreateBy,
                                                      CreateDate = c.CreateDate,
                                                      UpdateBy = c.UpdateBy,
                                                      UpdateDate = c.UpdateDate,
                                                      StatusText = us.Status,
                                                      HouseId = h.Id,
                                                      HouseText = h.HouseName,
                                                      CustomerLineText = cl.CustomerLineName
                                                  }).ToListAsync();
            if (useraccess == StatusUserAccess.UserAccess_Admin|| useraccess == StatusUserAccess.UserAccess_Audit)
                rtn = rtn.Where(x => x.HouseId == GetHouseIdByUserId(uid)).ToList();
            else if (useraccess == StatusUserAccess.UserAccess_Agent)
                rtn = rtn.Where(x => x.CustomerLineId == GetCustomerLineIdByUserId(uid)).ToList();
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
                _DailyLoanContext.Customer.Add(req);
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
        public async Task<bool> DeleteCustomer(int cid)
        {
            _DailyLoanContext.Customer.Remove(_DailyLoanContext.Customer.Where(x => x.Id == cid).FirstOrDefault());
            return (await _DailyLoanContext.SaveChangesAsync()) > 0;
        }
        #endregion
        #region Contract
        public async Task<List<ManagementContract>> GetAllContract(int uid, int useraccess)
        {
            List<ManagementContract> rtn = await (from c in _DailyLoanContext.Contract
                                                  join cu in _DailyLoanContext.User on c.ApproverId equals cu.Id
                                                  join us in _DailyLoanContext.StatusContract on c.Status equals us.Id
                                                  select new ManagementContract()
                                                  {
                                                      Id = c.Id,
                                                      ContractId = c.ContractId,
                                                      CustomerId = c.CustomerId,
                                                      GuarantorId = c.GuarantorId,
                                                      ApproverId = c.ApproverId,
                                                      TotalAmount = c.TotalAmount,
                                                      TotalPay = c.TotalPay,
                                                      Status = c.Status,
                                                      SpecialRateCount = c.SpecialRateCount,
                                                      CutCount = c.CutCount,
                                                      CreateBy = c.CreateBy,
                                                      CreateDate = c.CreateDate,
                                                      UpdateBy = c.UpdateBy,
                                                      UpdateDate = c.UpdateDate,
                                                      StatusText = us.Status,
                                                      ApproverName = cu.Firstname + " " + cu.Lastname
                                                  }).ToListAsync();
            if (useraccess == StatusUserAccess.UserAccess_Admin || useraccess == StatusUserAccess.UserAccess_Audit)
                rtn = rtn.Where(x => x.Customer.HouseId == GetHouseIdByUserId(uid)).ToList();
            else if (useraccess == StatusUserAccess.UserAccess_Agent)
                rtn = rtn.Where(x => x.Customer.CustomerLineId == GetCustomerLineIdByUserId(uid)).ToList();
            for(int i = 0;i < rtn.Count();i++)
            {
                rtn[i].Guaruntor = GetCustomer(rtn[i].GuarantorId);
                rtn[i].Customer = GetCustomer(rtn[i].CustomerId);
            }
            return rtn;
        }
        public ManagementContract GetContract(int cid)
        {
            ManagementContract rtn = (from c in _DailyLoanContext.Contract
                                                 join cu in _DailyLoanContext.User on c.ApproverId equals cu.Id
                                                 join us in _DailyLoanContext.StatusContract on c.Status equals us.Id
                                                   where c.Id == cid
                                                 select new ManagementContract()
                                                 {
                                                     Id = c.Id,
                                                     ContractId = c.ContractId,
                                                     CustomerId = c.CustomerId,
                                                     GuarantorId = c.GuarantorId,
                                                     ApproverId = c.ApproverId,
                                                     TotalAmount = c.TotalAmount,
                                                     TotalPay = c.TotalPay,
                                                     Status = c.Status,
                                                     SpecialRateCount = c.SpecialRateCount,
                                                     CutCount = c.CutCount,
                                                     CreateBy = c.CreateBy,
                                                     CreateDate = c.CreateDate,
                                                     UpdateBy = c.UpdateBy,
                                                     UpdateDate = c.UpdateDate,
                                                     StatusText = us.Status,
                                                     ApproverName = cu.Firstname + " " + cu.Lastname,
                                                 }).FirstOrDefault();
            rtn.Guaruntor = GetCustomer(rtn.GuarantorId);
            rtn.Customer = GetCustomer(rtn.CustomerId);
            return rtn;
        }
        public async Task<bool> EditContract(Contract req)
        {
            if (req.Id == 0)
                _DailyLoanContext.Contract.Add(req);
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
                //cus.Status = req.Status;
                //cus.SpecialRateCount = req.SpecialRateCount;
                //cus.CutCount = req.CutCount;
                cus.UpdateBy = req.UpdateBy;
                cus.UpdateDate = req.UpdateDate;
            }
            return (await _DailyLoanContext.SaveChangesAsync()) > 0;
        }
        public async Task<bool> DeleteContract(int cid)
        {
            _DailyLoanContext.Contract.Remove(_DailyLoanContext.Contract.Where(x => x.Id == cid).FirstOrDefault());
            return (await _DailyLoanContext.SaveChangesAsync()) > 0;
        }
        #endregion 
        #region DailyReport
        public async Task<DailyReportResponse> GetDailyReport(int uid, DateTime date)
        {
            var result = new DailyReportResponse();
            var data = await (from tran in _DailyLoanContext.Transaction.Where(x => x.CreateDate.Date == date.Date && x.AgentId == uid)
                                select tran ).ToListAsync();

            var collect = await (from con in _DailyLoanContext.Contract
                                 join cus in _DailyLoanContext.Customer on con.CustomerId equals cus.Id
                                 where cus.CustomerLineId == uid
                                 select cus).ToListAsync();
            if(data != null)
            {
                double bounty = (double)data.Where(x => x.Type == TransactionType_Status.Bounty).Select(x => x.Amount).Sum();
                result = new DailyReportResponse()
                {
                    bounty = bounty,
                    allowance = bounty >= _appsettingModel.Allowance ? 0 : (_appsettingModel.Allowance - bounty),
                    collect = data.Where(x => x.Type == TransactionType_Status.Pay).Select(x => x.Amount).Sum(),
                    mustcollect = collect.Select(x => x.DailyCollect).Sum()
                };
            }
           

            return result;
        }
        #endregion
    }
}
