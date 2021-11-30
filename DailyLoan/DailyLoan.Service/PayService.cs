
using DailyLoan.Repository.Interfaces;
using DailyLoan.Service.Interfaces;
using DailyLoan.Model.Entities.DailyLoan;
using DailyLoan.Model.Request.Management;
using DailyLoan.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DailyLoan.Service
{
    public class PayService : IPayService
    {
        private readonly IPayRepo _PayRepo;
        public PayService(IPayRepo payRepo)
        {
            _PayRepo = payRepo;
        }
        public async Task<List<User>> GetAllUserByCustomerLine(int cid)
        {
            return await _PayRepo.GetAllUserByCustomerLine(cid);
        }
        public int GetHouseIdByUserId(int uid)
        {
            return _PayRepo.GetHouseIdByUserId(uid);
        }
        public string GetContractID(string idcard)
        {
            return _PayRepo.GetContractIDByIdcard(idcard);
        }
        public string GetIDcardByCustomerId(int cid)
        {
            return _PayRepo.GetIdcardByCustomerId(cid);
        }
        public async Task<List<ManagementCustomer>> SearchCustomer(int uid, ContractSearchRequest req)
        {
            return await _PayRepo.SearchCustomer(uid, req.Idcard,req.Name,req.Firstname,req.Lastname,req.Address);
        }
        public async Task<List<ManagementContract>> SearchContract(int uid, ContractSearchRequest req)
        {
            return await _PayRepo.SearchContract(uid, req.Idcard, req.Firstname, req.Lastname, req.Address);
        }
        public bool isExistContract(int cid)
        {
            return _PayRepo.isExistContract(cid);
        }
        #region Customer
        public async Task<List<ManagementCustomer>> GetAllCustomer(int uid, int ua)
        {
            return await _PayRepo.GetAllCustomer(uid, ua);
        }
        public ManagementCustomer GetCustomer(int cid)
        {
            return _PayRepo.GetCustomer(cid);
        }
        public async Task<bool> EditCustomer(EditCustomerRequest req, int uid)
        {
            return await _PayRepo.EditCustomer(req.ToCustomer(uid));
        }
        public async Task<bool> DeleteCustomer(int cid)
        {
            return await _PayRepo.DeleteCustomer(cid);
        }
        #endregion
        #region Contract
        public async Task<List<ManagementContract>> GetAllContract(int uid, int ua)
        {
            return await _PayRepo.GetAllContract(uid, ua);
        }
        public ManagementContract GetContract(int cid)
        {
            return _PayRepo.GetContract(cid);
        }
        public async Task<bool> EditContract(EditContractRequest req, int uid)
        {
            return await _PayRepo.EditContract(req.ToContract(uid,_PayRepo.GetIdcardByCustomerId(req.CustomerId)));
        }
        public async Task<bool> DeleteContract(int cid)
        {
            return await _PayRepo.DeleteContract(cid);
        }
        #endregion
        #region DailyCost
        public async Task<bool> SaveDailyCost(DailyCost req)
        {
            return await _PayRepo.SaveDailyCost(req);
        }
        #endregion
    }
}
