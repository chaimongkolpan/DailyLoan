
using DailyLoan.Repository.Interfaces;
using DailyLoan.Library.Status;
using DailyLoan.Model.Entities.DailyLoan;
using DailyLoan.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace DailyLoan.Repository
{
    public class ManagementRepo : IManagementRepo
    {
        private readonly DailyLoanContext _DailyLoanContext;
        public ManagementRepo(DailyLoanContext dailyLoanContext)
        {
            _DailyLoanContext = dailyLoanContext;
        }

        #region getvalue
        public int GetHouseIdByUserId(int uid)
        {
            return (_DailyLoanContext.Users.Where(x => x.Id==uid).FirstOrDefault().HouseId);
        }
        #endregion

        public async Task<List<ManagementUser>> GetAllUser(int hid)
        {
            List<ManagementUser> rtn = await (from u in _DailyLoanContext.Users
                                  join ua in _DailyLoanContext.UserAccesses on u.UserAccess equals ua.Id
                                  join h in _DailyLoanContext.Houses on u.HouseId equals h.Id
                                  join up in _DailyLoanContext.UserPermissions on u.Id equals up.UserId
                                  join cl in _DailyLoanContext.CustomerLines on up.CustomerLineId equals cl.Id
                                  select new ManagementUser()
                                  {
                                      Id = u.Id,
                                      Username = u.Username,
                                      Password = u.Password,
                                      UserAccess = u.UserAccess,
                                      Firstname = u.Firstname,
                                      Lastname = u.Lastname,
                                      Nickname = u.Nickname,
                                      Phone1 = u.Phone1,
                                      Phone2 = u.Phone2,
                                      Status = u.Status,
                                      HouseId = u.HouseId,
                                      CreateBy = u.CreateBy,
                                      CreateDate = u.CreateDate,
                                      UpdateBy = u.UpdateBy,
                                      UpdateDate = u.UpdateDate,
                                      AccessText = ua.UserAccess1,
                                      HouseText = h.HouseName,
                                      CustomerLineText = cl.CustomerLineName
                                  }).ToListAsync();
            if(hid != 0)
                rtn = rtn.Where(x => x.HouseId == hid).ToList();
            return rtn;
        }
        public ManagementUser GetUser(int uid)
        {
            ManagementUser rtn = (from u in _DailyLoanContext.Users
                    join ua in _DailyLoanContext.UserAccesses on u.UserAccess equals ua.Id
                    join h in _DailyLoanContext.Houses on u.HouseId equals h.Id
                    join up in _DailyLoanContext.UserPermissions on u.Id equals up.UserId
                    join cl in _DailyLoanContext.CustomerLines on up.CustomerLineId equals cl.Id
                    where u.Id == uid
                       select new ManagementUser()
                       {
                            Id = u.Id,
                            Username = u.Username,
                            Password = u.Password,
                            UserAccess = u.UserAccess,
                            Firstname = u.Firstname,
                            Lastname = u.Lastname,
                            Nickname = u.Nickname,
                            Phone1 = u.Phone1,
                            Phone2 = u.Phone2,
                            Status = u.Status,
                            HouseId = u.HouseId,
                            CreateBy = u.CreateBy,
                            CreateDate = u.CreateDate,
                            UpdateBy = u.UpdateBy,
                            UpdateDate = u.UpdateDate,
                            AccessText = ua.UserAccess1,
                            HouseText = h.HouseName,
                            CustomerLineText = cl.CustomerLineName
                       }).FirstOrDefault();
            return rtn;
        }
        public async Task<bool> EditUser(User req)
        {
            if (req.Id == 0)
                _DailyLoanContext.Users.Add(req);
            else
                _DailyLoanContext.Users.Update(req);
            return (await _DailyLoanContext.SaveChangesAsync()) > 0;
        }
        public async Task<bool> DeleteUser(int uid)
        {
            _DailyLoanContext.Users.Remove(_DailyLoanContext.Users.Where(x => x.Id==uid).FirstOrDefault());
            return (await _DailyLoanContext.SaveChangesAsync()) > 0;
        }
    }
}
