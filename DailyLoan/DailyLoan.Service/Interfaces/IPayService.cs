using DailyLoan.Model.Entities.DailyLoan;
using DailyLoan.Model.Request.Management;
using DailyLoan.Model.Resoinse.Pay;
using DailyLoan.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace DailyLoan.Service.Interfaces
{
    public interface IPayService
    {
        Task<List<int>> GetAlert(int uid, int useraccess);
        Task<List<User>> GetAllUserByCustomerLine(int cid);
        int GetHouseIdByUserId(int uid);
        int GetCustomerLineIdByUserId(int uid);
        Task<List<ManagementCustomer>> SearchCustomer(int uid, ContractSearchRequest req);
        Task<List<ManagementContract>> SearchContract(int uid, ContractSearchRequest req);
        bool isExistContract(int cid);
        string GetContractID(string idcard);
        string GetIDcardByCustomerId(int cid);
        #region Customer
        Task<List<ManagementCustomer>> GetAllCustomer(int uid, int ua);
        ManagementCustomer GetCustomer(int cid);
        Task<bool> EditCustomer(EditCustomerRequest req, int uid);
        Task<bool> MoveCustomer(EditCustomerRequest req, int uid);
        Task<bool> DeleteCustomer(int cid);
        #endregion
        #region Contract
        Task<List<ManagementContract>> GetAllContract(int uid, int useraccess);
        ManagementContract GetContract(int cid);
        Task<bool> EditContract(EditContractRequest req, int uid);
        Task<bool> DeleteContract(int cid);
        Task<bool> Approve(int cid, int uid);
        Task<bool> Deny(int cid, string remark);
        #endregion
        #region DailyCost
        Task<bool> SaveDailyCost(DailyCost req,int uid);
        DailyCost GetDailyCost(int clid, DateTime date);
        Task<DailyReportResponse> GetMustReturn(int clid, DateTime date);
        Task<DailyReportResponse> GetMustReturnDaily(int clid, DateTime date);
        Task<double> GetPayToCustomer(int clid, DateTime date);
        #endregion
        #region Collector
        Task<List<ManagementContract>> SearchContractCollect(int uid, ContractSearchRequest req);
        Task<ManagementCollector> GetCollector(int cid);
        Task<bool> CollectCustomer(Transaction req,int uid);

        #endregion
        #region History
        Task<List<ManagementHistory>> GetAllHistory(int uid,int ua);
        Task<ManagementCut> GetCutDetail(int cid);
        Task<ManagementHistory> GetHistory(int cid);
        Task<bool> CutRequest(int cid, double amount, int uid, int type);
        bool isExistRequest(int cid, int type);
        Task<List<ManagementHistory>> SearchHistory(int uid, ContractSearchRequest req);
        #endregion
        #region DailyReport
        Task<bool> SaveDailyCostAgent(DailyCost req, int uid);
        Task<DailyReportResponse> GetDailyReport(int uid, DateTime date);
        #endregion
        #region Warn
        Task<List<ManagementWarn>> GetAllWarn(int uid, int useraccess);
        Task<ManagementWarn> GetWarn(int nid);
        Task<bool> EditNotification(Notification req, int uid);
        Task<bool> DeleteNotification(int nid);
        #endregion

        Task<MonthlyInput> GetMonthlyCost(int m, int y, int hid, int uid);
        Task<bool> SaveMonthlyCost(MonthlyInput req, int uid);
        Task<MonthlyReport> GetMonthlyReport(DateTime start, DateTime end, int hid, int uid);
    }
}
