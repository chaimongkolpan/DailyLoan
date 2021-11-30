using DailyLoan.Model.Entities.DailyLoan;
using DailyLoan.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DailyLoan.Model.Resoinse.Pay;

namespace DailyLoan.Repository.Interfaces
{
    public interface IPayRepo
    {
        #region getvalue
        Task<List<User>> GetAllUserByCustomerLine(int cid);
        int GetHouseIdByUserId(int uid);
        int GetCustomerLineIdByUserId(int uid);
        string GetIdcardByCustomerId(int cid);
        string GetContractIDByIdcard(string idcard);
        Task<List<ManagementCustomer>> SearchCustomer(int uid,string idcard, string name, string firstname, string lastname, string address);
        Task<List<ManagementContract>> SearchContract(int uid, string idcard, string firstname, string lastname, string address);
        bool isExistContract(int cid);
        #endregion

        #region Customer
        Task<List<ManagementCustomer>> GetAllCustomer(int uid, int useraccess);
        ManagementCustomer GetCustomer(int cid);
        Task<bool> EditCustomer(Customer req);
        Task<bool> DeleteCustomer(int cid);
        #endregion

        #region Contract
        Task<List<ManagementContract>> GetAllContract(int uid, int useraccess);
        ManagementContract GetContract(int cid);
        Task<bool> EditContract(Contract req);
        Task<bool> DeleteContract(int cid);
        #endregion
        #region DailyCost
        Task<bool> SaveDailyCost(DailyCost req);
        #endregion
        #region DailyReport
        Task<DailyReportResponse> GetDailyReport(int uid, DateTime date);
        #endregion
    }
}
