using DailyLoan.Model.Entities.DailyLoan;
using DailyLoan.Model.Request.Management;
using DailyLoan.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DailyLoan.Service.Interfaces
{
    public interface IManagementService
    {
        int GetHouseIdByUserId(int uid);
        Task<List<House>> GetAllHouse();
        Task<List<CustomerLine>> GetAllCustomerLine(int uid); 
        Task<List<CustomerLine>> GetAllCustomerLineByHouseID(int hid); 
        bool UsernameIsExist(string username);
        bool IdcardIsExist(string idcard);
        Task<List<ManagementUser>> SearchUser(int uid, ContractSearchRequest req);
        #region User
        Task<List<ManagementUser>> GetAllUser(int uid, int ua);
        ManagementUser GetUser(int uid);
        Task<bool> EditUser(EditUserRequest req, int uid);
        Task<bool> DeleteUser(int uid);
        #endregion
        #region House
        Task<List<ManagementHouse>> GetAllHouseList();
        ManagementHouse GetHouse(int hid);
        Task<bool> EditHouse(EditHouseRequest req, int uid);
        Task<bool> DeleteHouse(int hid);
        #endregion
        #region CustomerLine
        Task<List<ManagementCustomerLine>> GetAllCustomerLineList(int uid, int useraccess);
        ManagementCustomerLine GetCustomerLine(int clid);
        Task<bool> EditCustomerLine(EditCustomerLineRequest req, int uid);
        Task<bool> DeleteCustomerLine(int clid);
        #endregion
        #region Config
        Task<ManagementConfig> GetConfig(int hid);
        SpecialRate GetSpecialRate(int spid);
        Task<bool> EditConfig(EditConfigRequest req, int uid);
        Task<bool> EditSpecialRate(EditSpecialRateRequest req, int uid);
        Task<bool> DeleteSpecialRate(int spid);
        #endregion
    }
}
