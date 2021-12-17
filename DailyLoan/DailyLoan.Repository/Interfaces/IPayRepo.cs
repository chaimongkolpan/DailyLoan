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
        Task<List<int>> GetAlert(int uid, int useraccess);
        Task<List<User>> GetAllUserByCustomerLine(int cid);
        int GetHouseIdByUserId(int uid);
        int GetCustomerLineIdByUserId(int uid);
        string GetIdcardByCustomerId(int cid);
        string GetContractIDByIdcard(string idcard);
        Task<List<ManagementCustomer>> SearchCustomer(int uid,string idcard, string name, string firstname, string lastname, string address);
        Task<List<ManagementContract>> SearchContract(int uid, string idcard, string firstname, string lastname, string address);
        bool isExistContract(int cid);
        string getConfig(string name, int uid);
        string getHouseRate(int uid, DateTime date);
        #endregion

        #region Customer
        Task<List<ManagementCustomer>> GetAllCustomer(int uid, int useraccess);
        ManagementCustomer GetCustomer(int cid);
        Task<bool> EditCustomer(Customer req);
        Task<bool> MoveCustomer(int cid,int clid,int uid);
        Task<bool> DeleteCustomer(int cid);
        #endregion

        #region Contract
        Task<List<ManagementContract>> GetAllContract(int uid, int useraccess);
        ManagementContract GetContract(int cid);
        Task<bool> EditContract(Contract req);
        Task<bool> DeleteContract(int cid);
        Task<bool> Approve(int cid,int uid);
        Task<bool> Deny(int cid, string remark);
        #endregion
        #region DailyCost
        Task<bool> SaveDailyCost(DailyCost req,int uid);
        DailyCost GetDailyCost(int clid, DateTime date);
        Task<DailyReportResponse> GetMustReturn(int clid, DateTime date);
        Task<double> GetPayToCustomer(int clid, DateTime date);
        #endregion
        #region DailyReport
        Task<DailyReportResponse> GetDailyReport(int uid, DateTime date);
        #endregion
        #region Collector
        Task<List<Transaction>> GetPayHistory(int cid);
        Task<bool> CollectCustomer(int uid, int cid, double amount, string remark);

        #endregion
        #region Warn
        Task<List<ManagementWarn>> GetAllWarn(int uid, int useraccess);
        Task<ManagementWarn> GetWarn(int nid);
        Task<bool> EditNotification(int id, string remark, int status, int uid);
        Task<bool> DeleteNotification(int nid);
        #endregion
        #region History
        Task<ManagementCut> GetCutDetail(int cid);
        Task<bool> CutRequest(int cid, double amount, int uid, int type);
        bool isExistRequest(int cid, int type);
        #endregion
    }
}
