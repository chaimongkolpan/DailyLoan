
using DailyLoan.Repository.Interfaces;
using DailyLoan.Service.Interfaces;
using DailyLoan.Model.Entities.DailyLoan;
using DailyLoan.Model.Request.Management;
using DailyLoan.Model.Resoinse.Pay;
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
        public async Task<List<int>> GetAlert(int uid, int useraccess)
        {
            return await _PayRepo.GetAlert(uid, useraccess);
        }
        public async Task<List<User>> GetAllUserByCustomerLine(int cid)
        {
            return await _PayRepo.GetAllUserByCustomerLine(cid);
        }
        public int GetHouseIdByUserId(int uid)
        {
            return _PayRepo.GetHouseIdByUserId(uid);
        }
        public int GetCustomerLineIdByUserId(int uid)
        {
            return _PayRepo.GetCustomerLineIdByUserId(uid);
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
        public async Task<bool> MoveCustomer(EditCustomerRequest req, int uid)
        {
            return await _PayRepo.MoveCustomer(req.Id,req.CustomerLineId,uid);
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
        public async Task<bool> Approve(int cid, int uid)
        {
            return await _PayRepo.Approve(cid,uid);
        }
        public async Task<bool> Deny(int cid,string remark)
        {
            return await _PayRepo.Deny(cid,remark);
        }
        public async Task<bool> DeleteContract(int cid)
        {
            return await _PayRepo.DeleteContract(cid);
        }
        #endregion
        #region Collector
        public async Task<ManagementCollector> GetCollector(int cid)
        {
            ManagementContract tmp = _PayRepo.GetContract(cid);
            ManagementCollector rtn = new ManagementCollector(tmp);
            rtn.History = await _PayRepo.GetPayHistory(cid);
            var hrate = rtn.SpecialRateCount.Value < 3 ? Convert.ToDouble(_PayRepo.getHouseRate(rtn.CreateBy, DateTime.Now)) : Convert.ToDouble(_PayRepo.getConfig("HouseRate", rtn.CreateBy));
            var mincutday = Convert.ToDouble(_PayRepo.getConfig("TotalProfit", rtn.CreateBy)) + hrate + Convert.ToDouble(_PayRepo.getConfig("AgentRate", rtn.CreateBy));
            //var mincutday = Convert.ToDouble(_PayRepo.getConfig("TotalProfit", rtn.CreateBy)) + Convert.ToDouble(_PayRepo.getHouseRate(rtn.CreateBy, DateTime.Now)) + Convert.ToDouble(_PayRepo.getConfig("AgentRate", rtn.CreateBy));
            mincutday = mincutday / 100;
            var inc = Convert.ToDouble(_PayRepo.getConfig("IncCutCriteria", rtn.CreateBy));
            var dec = (Convert.ToDouble(_PayRepo.getConfig("DecCutPercen", rtn.CreateBy)) / 100);
            rtn.canCut = rtn.TotalPay >= rtn.TotalAmount * mincutday;
            rtn.canIncCut = rtn.CutCount >= inc && rtn.TotalPay >= rtn.TotalAmount * mincutday;
            rtn.canDecCut = rtn.TotalPay >= rtn.TotalAmount * dec;
            rtn.remainCut = (double)((rtn.TotalAmount * mincutday) - rtn.TotalPay);
            rtn.remainIncCut = inc - rtn.CutCount.Value;
            rtn.remainDecCut = (double)((rtn.TotalAmount * dec) - rtn.TotalPay);
            return rtn;
        }
        public async Task<bool> CollectCustomer(Transaction req,int uid)
        {
            return await _PayRepo.CollectCustomer(uid,req.ContractId,req.Amount.Value,req.Remark);
        }
        #endregion
        #region History
        public async Task<List<ManagementHistory>> GetAllHistory(int uid,int ua)
        {
            List<ManagementHistory> rtn = new List<ManagementHistory>();
            List<ManagementContract> tmp = await _PayRepo.GetAllContract(uid, ua);
            for(int i = 0; i < tmp.Count; i++)
            {
                ManagementHistory tt = new ManagementHistory(tmp[i]);
                tt.History = await _PayRepo.GetPayHistory(tmp[i].Id);
                //var mincutday = Convert.ToDouble(_PayRepo.getConfig("MinCutDay",uid)) * Convert.ToDouble(_PayRepo.getConfig("HouseRate", uid));
                var hrate = tmp[i].SpecialRateCount.Value < 3 ? Convert.ToDouble(_PayRepo.getHouseRate(uid, DateTime.Now)) : Convert.ToDouble(_PayRepo.getConfig("HouseRate", uid));
                var mincutday = Convert.ToDouble(_PayRepo.getConfig("TotalProfit", uid))+ hrate + Convert.ToDouble(_PayRepo.getConfig("AgentRate", uid));
                mincutday = mincutday / 100;
                tt.canCut = tmp[i].TotalPay >= tmp[i].TotalAmount * mincutday;
                tt.canIncCut = tmp[i].CutCount >= Convert.ToDouble(_PayRepo.getConfig("IncCutCriteria", uid)) && tmp[i].TotalPay >= tmp[i].TotalAmount * mincutday;
                tt.canDecCut = tmp[i].TotalPay >= tmp[i].TotalAmount * (Convert.ToDouble(_PayRepo.getConfig("DecCutPercen", uid)) / 100);
                rtn.Add(tt);
            }
            return rtn;
        }
        public async Task<ManagementHistory> GetHistory(int cid)
        {
            ManagementContract tmp = _PayRepo.GetContract(cid);
            ManagementHistory rtn = new ManagementHistory(tmp);
            rtn.History = await _PayRepo.GetPayHistory(tmp.Id);
            //rtn.canCut = true;
            //rtn.canIncCut = true;
            //rtn.canDecCut = true;
            return rtn;
        }
        public async Task<ManagementCut> GetCutDetail(int cid)
        {
            ManagementCut tmp = await _PayRepo.GetCutDetail(cid);
            return tmp;
        }
        public async Task<bool> CutRequest(int cid, double amount, int uid, int type)
        {
            return await _PayRepo.CutRequest(cid,amount,uid, type);
        }
        public bool isExistRequest(int cid, int type)
        {
            return _PayRepo.isExistRequest(cid,type);
        }
        public async Task<List<ManagementHistory>> SearchHistory(int uid, ContractSearchRequest req)
        {
            List<ManagementHistory> rtn = new List<ManagementHistory>();
            List<ManagementContract> tmp = await _PayRepo.SearchContract(uid, req.Idcard, req.Firstname, req.Lastname, req.Address);

            for (int i = 0; i < tmp.Count; i++)
            {
                ManagementHistory tt = new ManagementHistory(tmp[i]);
                tt.History = await _PayRepo.GetPayHistory(tmp[i].Id);
                //var mincutday = Convert.ToDouble(_PayRepo.getConfig("MinCutDay",uid)) * Convert.ToDouble(_PayRepo.getConfig("HouseRate", uid));
                var hrate = tmp[i].SpecialRateCount.Value < 3 ? Convert.ToDouble(_PayRepo.getHouseRate(uid, DateTime.Now)) : Convert.ToDouble(_PayRepo.getConfig("HouseRate", uid));
                var mincutday = Convert.ToDouble(_PayRepo.getConfig("TotalProfit", uid)) + hrate + Convert.ToDouble(_PayRepo.getConfig("AgentRate", uid));
                mincutday = mincutday / 100;
                tt.canCut = tmp[i].TotalPay >= tmp[i].TotalAmount * mincutday;
                tt.canIncCut = tmp[i].CutCount >= Convert.ToDouble(_PayRepo.getConfig("IncCutCriteria", uid)) && tmp[i].TotalPay >= tmp[i].TotalAmount * mincutday;
                tt.canDecCut = tmp[i].TotalPay >= tmp[i].TotalAmount * (Convert.ToDouble(_PayRepo.getConfig("DecCutPercen", uid)) / 100);
                rtn.Add(tt);
            }
            return rtn;
        }
        #endregion
        #region DailyReport
        public async Task<DailyReportResponse> GetDailyReport(int uid, DateTime date)
        {
            DailyReportResponse rtn = await _PayRepo.GetDailyReport(_PayRepo.GetCustomerLineIdByUserId(uid), date);
            rtn.date = date;
            return rtn;
        }
        #endregion
        #region DailyCost
        public async Task<bool> SaveDailyCost(DailyCost req,int uid)
        {
            return await _PayRepo.SaveDailyCost(req,uid);
        }
        public async Task<DailyReportResponse> GetMustReturn(int clid, DateTime date)
        {
            return await _PayRepo.GetMustReturn(clid,date);
        }
        public async Task<double> GetPayToCustomer(int clid, DateTime date)
        {
            return await _PayRepo.GetPayToCustomer(clid, date);
        }
        public DailyCost GetDailyCost(int clid, DateTime date)
        {
            return _PayRepo.GetDailyCost(clid,date);
        }
        #endregion
        #region Warn
        public async Task<List<ManagementWarn>> GetAllWarn(int uid, int useraccess)
        {
            return await _PayRepo.GetAllWarn(uid, useraccess);
        }
        public async Task<ManagementWarn> GetWarn(int nid)
        {
            return await _PayRepo.GetWarn(nid);
        }
        public async Task<bool> EditNotification(Notification req, int uid)
        {
            return await _PayRepo.EditNotification(req.Id, req.Remark, req.Status,uid);
        }
        public async Task<bool> DeleteNotification(int nid)
        {
            return await _PayRepo.DeleteNotification(nid);
        }
        #endregion
    }
}
