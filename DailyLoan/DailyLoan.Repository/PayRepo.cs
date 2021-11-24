
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

namespace DailyLoan.Repository
{
    public class PayRepo : IPayRepo
    {
        private readonly DailyLoanContext _DailyLoanContext;
        public PayRepo(DailyLoanContext dailyLoanContext)
        {
            _DailyLoanContext = dailyLoanContext;
        }
        #region getValue
        public int GetHouseIdByUserId(int uid)
        {
            return (_DailyLoanContext.Users.Where(x => x.Id == uid).FirstOrDefault().HouseId);
        }
        public int GetCustomerLineIdByUserId(int uid)
        {
            var rtn = _DailyLoanContext.UserPermissions.Where(x => x.UserId == uid).FirstOrDefault();
            return (rtn == null ? 0 : rtn.CustomerLineId);
        }
        #endregion
        #region Customer
        public async Task<List<ManagementCustomer>> GetAllCustomer(int uid, int useraccess)
        {
            List<ManagementCustomer> rtn = await (from c in _DailyLoanContext.Customers
                                                  join us in _DailyLoanContext.StatusCustomers on c.Status equals us.Id
                                                  join cl in _DailyLoanContext.CustomerLines on c.CustomerLineId equals cl.Id
                                                  join h in _DailyLoanContext.Houses on cl.HouseId equals h.Id
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
            ManagementCustomer rtn = (from c in _DailyLoanContext.Customers
                                      join us in _DailyLoanContext.StatusCustomers on c.Status equals us.Id
                                      join cl in _DailyLoanContext.CustomerLines on c.CustomerLineId equals cl.Id
                                      join h in _DailyLoanContext.Houses on cl.HouseId equals h.Id
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
                _DailyLoanContext.Customers.Add(req);
            else
            {
                var cus = await _DailyLoanContext.Customers.FindAsync(req.Id);
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
            _DailyLoanContext.Customers.Remove(_DailyLoanContext.Customers.Where(x => x.Id == cid).FirstOrDefault());
            return (await _DailyLoanContext.SaveChangesAsync()) > 0;
        }
        #endregion
        #region Contract
        public async Task<List<ManagementContract>> GetAllContract(int uid, int useraccess)
        {
            List<ManagementContract> rtn = await (from c in _DailyLoanContext.Contracts
                                                  join cc in _DailyLoanContext.Customers on c.CustomerId equals cc.Id
                                                  join cg in _DailyLoanContext.Customers on c.GuarantorId equals cg.Id
                                                  join cu in _DailyLoanContext.Users on c.ApproverId equals cu.Id
                                                  join us in _DailyLoanContext.StatusContracts on c.Status equals us.Id
                                                  join cl in _DailyLoanContext.CustomerLines on cc.CustomerLineId equals cl.Id
                                                  join h in _DailyLoanContext.Houses on cl.HouseId equals h.Id
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
                                                      HouseId = h.Id,
                                                      HouseText = h.HouseName,
                                                      CustomerLineText = cl.CustomerLineName,
                                                      CustomerLineId = cl.Id,
                                                      Firstname = cc.Firstname,
                                                      Lastname = cc.Lastname,
                                                      ShortAddress = cc.ShortAddress,
                                                      GuaruntorName = cg.Firstname + " " + cg.Lastname,
                                                      ApproverName = cu.Firstname + " " + cu.Lastname
                                                  }).ToListAsync();
            if (useraccess == StatusUserAccess.UserAccess_Admin || useraccess == StatusUserAccess.UserAccess_Audit)
                rtn = rtn.Where(x => x.HouseId == GetHouseIdByUserId(uid)).ToList();
            else if (useraccess == StatusUserAccess.UserAccess_Agent)
                rtn = rtn.Where(x => x.CustomerLineId == GetCustomerLineIdByUserId(uid)).ToList();
            return rtn;
        }
        public ManagementContract GetContract(int cid)
        {
            ManagementContract rtn = (from c in _DailyLoanContext.Contracts
                                                 join cc in _DailyLoanContext.Customers on c.CustomerId equals cc.Id
                                                 join cg in _DailyLoanContext.Customers on c.GuarantorId equals cg.Id
                                                 join cu in _DailyLoanContext.Users on c.ApproverId equals cu.Id
                                                 join us in _DailyLoanContext.StatusContracts on c.Status equals us.Id
                                                 join cl in _DailyLoanContext.CustomerLines on cc.CustomerLineId equals cl.Id
                                                 join h in _DailyLoanContext.Houses on cl.HouseId equals h.Id
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
                                                     HouseId = h.Id,
                                                     HouseText = h.HouseName,
                                                     CustomerLineText = cl.CustomerLineName,
                                                     CustomerLineId = cl.Id,
                                                     Firstname = cc.Firstname,
                                                     Lastname = cc.Lastname,
                                                     ShortAddress = cc.ShortAddress,
                                                     GuaruntorName = cg.Firstname + " " + cg.Lastname,
                                                     ApproverName = cu.Firstname + " " + cu.Lastname
                                                 }).FirstOrDefault();
            return rtn;
        }
        public async Task<bool> EditContract(Contract req)
        {
            if (req.Id == 0)
                _DailyLoanContext.Contracts.Add(req);
            else
            {
                var cus = await _DailyLoanContext.Contracts.FindAsync(req.Id);
                if (cus == null)
                {
                    return await Task.FromResult(false);
                }

                cus.ContractId = req.ContractId;
                cus.CustomerId = req.CustomerId;
                cus.GuarantorId = req.GuarantorId;
                cus.ApproverId = req.ApproverId;
                cus.TotalAmount = req.TotalAmount;
                cus.TotalPay = req.TotalPay;
                cus.Status = req.Status;
                cus.SpecialRateCount = req.SpecialRateCount;
                cus.CutCount = req.CutCount;
                cus.UpdateBy = req.UpdateBy;
                cus.UpdateDate = req.UpdateDate;
            }
            return (await _DailyLoanContext.SaveChangesAsync()) > 0;
        }
        public async Task<bool> DeleteContract(int cid)
        {
            _DailyLoanContext.Contracts.Remove(_DailyLoanContext.Contracts.Where(x => x.Id == cid).FirstOrDefault());
            return (await _DailyLoanContext.SaveChangesAsync()) > 0;
        }
        #endregion
    }
}
