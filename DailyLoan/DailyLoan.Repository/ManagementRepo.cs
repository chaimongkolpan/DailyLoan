
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
        public int GetCustomerLineIdByUserId(int uid)
        {
            var rtn = _DailyLoanContext.UserPermissions.Where(x => x.UserId == uid).FirstOrDefault();
            return (rtn==null?0:rtn.CustomerLineId);
        }
        #endregion

        public async Task<List<House>> GetAllHouse()
        {
            return await _DailyLoanContext.Houses.ToListAsync();
        }
        public async Task<List<CustomerLine>> GetAllCustomerLine(int hid)
        {
            return await _DailyLoanContext.CustomerLines.Where(x =>x.HouseId==hid).ToListAsync();
        }
        public bool UsernameIsExist(string username)
        {
            return _DailyLoanContext.Users.Where(x => x.Username == username).FirstOrDefault() != null;
        }
        public async Task<List<ManagementUser>> GetAllUser(int hid,int useraccess)
        {
            List<ManagementUser> rtn = await (from u in _DailyLoanContext.Users
                                  join us in _DailyLoanContext.StatusUsers on u.Status equals us.Id
                                  join ua in _DailyLoanContext.UserAccesses on u.UserAccess equals ua.Id
                                  join h in _DailyLoanContext.Houses on u.HouseId equals h.Id
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
                                      StatusText = us.Status,
                                      AccessText = ua.UserAccess1,
                                      HouseText = h.HouseName
                                  }).ToListAsync();
            if(useraccess != StatusUserAccess.UserAccess_Superadmin)
                rtn = rtn.Where(x => x.HouseId == hid).ToList();
            for(int i = 0; i < rtn.Count(); i++)
            {
                int clid = GetCustomerLineIdByUserId(rtn[i].Id);
                if (clid == 0) rtn[i].CustomerLineText = "-";
                else
                {
                    rtn[i].CustomerLineText = _DailyLoanContext.CustomerLines.Where(x => x.Id == clid).FirstOrDefault().CustomerLineName;
                }
            }
            return rtn;
        }
        public ManagementUser GetUser(int uid)
        {
            ManagementUser rtn = (from u in _DailyLoanContext.Users
                                  join us in _DailyLoanContext.StatusUsers on u.Status equals us.Id
                                  join ua in _DailyLoanContext.UserAccesses on u.UserAccess equals ua.Id
                                join h in _DailyLoanContext.Houses on u.HouseId equals h.Id
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
                           StatusText = us.Status,
                           AccessText = ua.UserAccess1,
                            HouseText = h.HouseName
                       }).FirstOrDefault();
            int clid = GetCustomerLineIdByUserId(uid);
            if (clid == 0) rtn.CustomerLineText = "-";
            else
            {
                rtn.CustomerLineText = _DailyLoanContext.CustomerLines.Where(x => x.Id==clid).FirstOrDefault().CustomerLineName;
            }
            return rtn;
        }
        public async Task<bool> EditUser(User req)
        {
            if (req.Id == 0)
                _DailyLoanContext.Users.Add(req);
            else
            {
                var user = await _DailyLoanContext.Users.FindAsync(req.Id);
                if (user == null)
                {
                    return await Task.FromResult(false);
                }
                user.Username = req.Username;
                user.Password = req.Password;
                user.UserAccess = req.UserAccess;
                user.Firstname = req.Firstname;
                user.Lastname = req.Lastname;
                user.Nickname = req.Nickname;
                user.Phone1 = req.Phone1;
                user.Phone2 = req.Phone2;
                user.Status = req.Status;
                user.HouseId = req.HouseId;
            }
            return (await _DailyLoanContext.SaveChangesAsync()) > 0;
        }
        public async Task<bool> DeleteUser(int uid)
        {
            _DailyLoanContext.Users.Remove(_DailyLoanContext.Users.Where(x => x.Id==uid).FirstOrDefault());
            return (await _DailyLoanContext.SaveChangesAsync()) > 0;
        }
    }
}
