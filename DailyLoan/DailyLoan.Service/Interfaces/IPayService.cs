using DailyLoan.Model.Entities.DailyLoan;
using DailyLoan.Model.Request.Management;
using DailyLoan.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace DailyLoan.Service.Interfaces
{
    public interface IPayService
    {
        Task<List<User>> GetAllUserByCustomerLine(int cid);
        int GetHouseIdByUserId(int uid);
        Task<List<ManagementCustomer>> SearchCustomer(ContractSearchRequest req);
        bool isExistContract(int cid);
        #region Customer
        Task<List<ManagementCustomer>> GetAllCustomer(int uid, int ua);
        ManagementCustomer GetCustomer(int cid);
        Task<bool> EditCustomer(EditCustomerRequest req, int uid);
        Task<bool> DeleteCustomer(int cid);
        #endregion
        #region Contract
        Task<List<ManagementContract>> GetAllContract(int uid, int useraccess);
        ManagementContract GetContract(int cid);
        Task<bool> EditContract(EditContractRequest req, int uid);
        Task<bool> DeleteContract(int cid);
        #endregion
        #region DailyCost
        Task<bool> SaveDailyCost(DailyCost req);
        #endregion
    }
}
