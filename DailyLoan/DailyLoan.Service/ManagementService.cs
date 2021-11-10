
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

        public async Task<List<House>> GetAllHouse()
        {
            return await _ManagementRepo.GetAllHouse();
        }
        public async Task<List<CustomerLine>> GetAllCustomerLine(int uid)
        {
            return await _ManagementRepo.GetAllCustomerLine(_ManagementRepo.GetHouseIdByUserId(uid));
        }
        public bool UsernameIsExist(string username)
        {
            return _ManagementRepo.UsernameIsExist(username);
        }
        public async Task<List<ManagementUser>> GetAllUser(int uid, int ua)
        {
            return await _ManagementRepo.GetAllUser(_ManagementRepo.GetHouseIdByUserId(uid),ua);
        }
        public ManagementUser GetUser(int uid)
        {
            return _ManagementRepo.GetUser(uid);
        }
        public async Task<bool> EditUser(EditUserRequest req,int uid)
        {
            return await _ManagementRepo.EditUser(req.ToUser(uid));
        }
        public async Task<bool> DeleteUser(int uid)
        {
            return await _ManagementRepo.DeleteUser(uid);
        }
    }
}
