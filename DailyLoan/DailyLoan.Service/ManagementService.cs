
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
    public class ManagementService : IManagementService
    {
        private readonly IManagementRepo _ManagementRepo;
        public ManagementService(IManagementRepo managementRepo)
        {
            _ManagementRepo = managementRepo;
        }

        public int GetHouseIdByUserId(int uid)
        {
            return _ManagementRepo.GetHouseIdByUserId(uid);
        }
        public async Task<List<House>> GetAllHouse()
        {
            return await _ManagementRepo.GetAllHouse();
        }
        public async Task<List<CustomerLine>> GetAllCustomerLine(int uid)
        {
            return await _ManagementRepo.GetAllCustomerLine(uid);
        }
        public async Task<List<CustomerLine>> GetAllCustomerLineByHouseID(int hid)
        {
            return await _ManagementRepo.GetAllCustomerLineByHouseID(hid);
        }
        public bool UsernameIsExist(string username)
        {
            return _ManagementRepo.UsernameIsExist(username);
        }
        public bool IdcardIsExist(string idcard)
        {
            return _ManagementRepo.IdcardIsExist(idcard);
        }

        #region User
        public async Task<List<ManagementUser>> GetAllUser(int uid, int ua)
        {
            return await _ManagementRepo.GetAllUser(uid,ua);
        }
        public ManagementUser GetUser(int uid)
        {
            return _ManagementRepo.GetUser(uid);
        }
        public async Task<bool> EditUser(EditUserRequest req,int uid)
        {
            req.Id = await _ManagementRepo.EditUser(req.ToUser(uid,_ManagementRepo.GetHouseIdByUserId(uid)));
            if (req.Id != 0 && req.isNewCustomerLine())
            {
                return (await _ManagementRepo.EditUserCustomerLine(req.ToUserPermission(uid)));
            }
            else return req.Id != 0;
        }
        public async Task<bool> DeleteUser(int uid)
        {
            return await _ManagementRepo.DeleteUser(uid);
        }
        #endregion
        #region Customer
        public async Task<List<ManagementCustomer>> GetAllCustomer(int uid, int ua)
        {
            return await _ManagementRepo.GetAllCustomer(uid, ua);
        }
        public ManagementCustomer GetCustomer(int cid)
        {
            return _ManagementRepo.GetCustomer(cid);
        }
        public async Task<bool> EditCustomer(EditCustomerRequest req, int uid)
        {
            return await _ManagementRepo.EditCustomer(req.ToCustomer(uid));
        }
        public async Task<bool> DeleteCustomer(int cid)
        {
            return await _ManagementRepo.DeleteCustomer(cid);
        }
        #endregion
        #region House
        public async Task<List<ManagementHouse>> GetAllHouseList()
        {
            return await _ManagementRepo.GetAllHouseList();
        }
        public ManagementHouse GetHouse(int hid)
        {
            return _ManagementRepo.GetHouse(hid);
        }
        public async Task<bool> EditHouse(EditHouseRequest req, int uid)
        {
            return await _ManagementRepo.EditHouse(req.ToHouse(uid));
        }
        public async Task<bool> DeleteHouse(int hid)
        {
            return await _ManagementRepo.DeleteHouse(hid);
        }
        #endregion
        #region CustomerLine
        public async Task<List<ManagementCustomerLine>> GetAllCustomerLineList(int uid, int ua)
        {
            return await _ManagementRepo.GetAllCustomerLineList(uid, ua);
        }
        public ManagementCustomerLine GetCustomerLine(int clid)
        {
            return _ManagementRepo.GetCustomerLine(clid);
        }
        public async Task<bool> EditCustomerLine(EditCustomerLineRequest req, int uid)
        {
            return await _ManagementRepo.EditCustomerLine(req.ToCustomerLine(uid, _ManagementRepo.GetHouseIdByUserId(uid)));
        }
        public async Task<bool> DeleteCustomerLine(int clid)
        {
            return await _ManagementRepo.DeleteCustomerLine(clid);
        }
        #endregion
        #region Config
        public async Task<ManagementConfig> GetConfig(int hid)
        {
            return await _ManagementRepo.GetConfig(hid);
        }
        public SpecialRate GetSpecialRate(int spid)
        {
            return _ManagementRepo.GetSpecialRate(spid);
        }
        public async Task<bool> EditConfig(EditConfigRequest req, int uid)
        {
            Dictionary<string, string> configs = new Dictionary<string, string>();
            configs.Add("CustomerRate",req.CustomerRate);
            configs.Add("AgentRate",req.AgentRate);
            configs.Add("HouseRate",req.HouseRate);
            configs.Add("MinCutDay",req.MinCutDay);
            configs.Add("IncCutCriteria",req.IncCutCriteria);
            configs.Add("DecCutCriteria",req.DecCutCriteria);
            configs.Add("DecCutPercen",req.DecCutPercen);
            configs.Add("SpecialRateCriteria",req.SpecialRateCriteria);
            configs.Add("TotalProfit",req.TotalProfit);
            configs.Add("NotPayAlert",req.NotPayAlert);
            configs.Add("PartialPayAlert",req.PartialPayAlert);
            return await _ManagementRepo.EditConfig(configs,req.HouseId,uid);
        }
        public async Task<bool> EditSpecialRate(EditSpecialRateRequest req, int uid)
        {
            return await _ManagementRepo.EditSpecialRate(req.ToSpecialRate(uid));
        }
        public async Task<bool> DeleteSpecialRate(int spid)
        {
            return await _ManagementRepo.DeleteSpecialRate(spid);
        }
        #endregion
    }
}
