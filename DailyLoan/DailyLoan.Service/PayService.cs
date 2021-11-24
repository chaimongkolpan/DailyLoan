
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
        public int GetHouseIdByUserId(int uid)
        {
            return _PayRepo.GetHouseIdByUserId(uid);
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
            return await _PayRepo.EditContract(req.ToContract(uid));
        }
        public async Task<bool> DeleteContract(int cid)
        {
            return await _PayRepo.DeleteContract(cid);
        }
        #endregion
    }
}
