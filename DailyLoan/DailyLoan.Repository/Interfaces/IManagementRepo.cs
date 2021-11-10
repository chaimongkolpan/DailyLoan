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
        #endregion

        Task<List<House>> GetAllHouse();
        Task<List<CustomerLine>> GetAllCustomerLine(int hid);
        bool UsernameIsExist(string username);
        Task<List<ManagementUser>> GetAllUser(int hid, int useraccess);
        ManagementUser GetUser(int uid);
        Task<bool> EditUser(User req);
        Task<bool> DeleteUser(int uid);
    }
}
