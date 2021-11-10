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
        Task<List<House>> GetAllHouse();
        Task<List<CustomerLine>> GetAllCustomerLine(int uid);
        bool UsernameIsExist(string username);
        Task<List<ManagementUser>> GetAllUser(int uid, int ua);
        ManagementUser GetUser(int uid);
        Task<bool> EditUser(EditUserRequest req, int uid);
        Task<bool> DeleteUser(int uid);
    }
}
