using DailyLoan.Model.Entities.DailyLoan;
using DailyLoan.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DailyLoan.Repository.Interfaces
{
    public interface IManagementRepo
    {
        
        #region getvalue
        int GetHouseIdByUserId(int uid);
        int GetCustomerLineIdByUserId(int uid);
        string GetHouseNameByHouseId(int hid);
        #endregion
        

        Task<List<House>> GetAllHouse();
        Task<List<CustomerLine>> GetAllCustomerLine(int uid);
        Task<List<CustomerLine>> GetAllCustomerLineByHouseID(int hid);
        bool UsernameIsExist(string username);
        bool IdcardIsExist(string idcard);

        #region User
        Task<List<ManagementUser>> GetAllUser(int uid, int useraccess);
        ManagementUser GetUser(int uid);
        Task<bool> EditUserCustomerLine(List<int> data);
        Task<int> EditUser(User req);
        Task<bool> DeleteUser(int uid);
        #endregion
        #region Customer
        Task<List<ManagementCustomer>> GetAllCustomer(int uid, int useraccess);
        ManagementCustomer GetCustomer(int cid);
        Task<bool> EditCustomer(Customer req);
        Task<bool> DeleteCustomer(int cid);
        #endregion
        #region House
        Task<List<ManagementHouse>> GetAllHouseList();
        ManagementHouse GetHouse(int hid);
        Task<bool> EditHouse(House req);
        Task<bool> DeleteHouse(int hid);
        #endregion
        #region CustomerLine
        Task<List<ManagementCustomerLine>> GetAllCustomerLineList(int uid, int useraccess);
        ManagementCustomerLine GetCustomerLine(int clid);
        Task<bool> EditCustomerLine(CustomerLine req);
        Task<bool> DeleteCustomerLine(int clid);
        #endregion
        #region Config
        Task<ManagementConfig> GetConfig(int hid);
        SpecialRate GetSpecialRate(int spid);
        Task<bool> EditConfig(Dictionary<string, string> req, int hid, int uid);
        Task<bool> EditSpecialRate(SpecialRate req);
        Task<bool> DeleteSpecialRate(int spid);
        #endregion

    }
}
