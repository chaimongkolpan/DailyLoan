
using DailyLoan.Repository.Interfaces;
using DailyLoan.Library;
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
        public string GetHouseNameByHouseId(int hid)
        {
            return _DailyLoanContext.Houses.Where(x => x.Id == hid).FirstOrDefault().HouseName;
        }
        #endregion

        public async Task<List<House>> GetAllHouse()
        {
            return await _DailyLoanContext.Houses.ToListAsync();
        }
        public async Task<List<CustomerLine>> GetAllCustomerLine(int uid)
        {
            List<CustomerLine> rtn = await _DailyLoanContext.CustomerLines.Where(x =>x.HouseId== GetHouseIdByUserId(uid)).ToListAsync();
            if (rtn == null) rtn = new List<CustomerLine>();
            return rtn;
        }
        public async Task<List<CustomerLine>> GetAllCustomerLineByHouseID(int hid)
        {
            List<CustomerLine> rtn = await _DailyLoanContext.CustomerLines.Where(x => x.HouseId == hid).ToListAsync();
            if (rtn == null) rtn = new List<CustomerLine>();
            return rtn;
        }
        public bool UsernameIsExist(string username)
        {
            return _DailyLoanContext.Users.Where(x => x.Username == username).FirstOrDefault() != null;
        }
        public bool IdcardIsExist(string idcard)
        {
            return _DailyLoanContext.Customers.Where(x => x.Idcard == idcard).FirstOrDefault() != null;
        }

        #region User
        public async Task<List<ManagementUser>> GetAllUser(int uid,int useraccess)
        {
            List<ManagementUser> rtn = await (from u in _DailyLoanContext.Users
                                  join us in _DailyLoanContext.StatusUsers on u.Status equals us.Id
                                  join ua in _DailyLoanContext.UserAccesses on u.UserAccess equals ua.Id
                                  join h in _DailyLoanContext.Houses on u.HouseId equals h.Id
                                  select new ManagementUser()
                                  {
                                      Id = u.Id,
                                      Username = u.Username,
                                      Password = StringCipher.DecryptString(u.Password),
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
                rtn = rtn.Where(x => x.HouseId == GetHouseIdByUserId(uid)).ToList();
            for(int i = 0; i < rtn.Count(); i++)
            {
                rtn[i].CustomerLineId = GetCustomerLineIdByUserId(rtn[i].Id);
                if (rtn[i].CustomerLineId == 0) rtn[i].CustomerLineText = "-";
                else
                {
                    rtn[i].CustomerLineText = _DailyLoanContext.CustomerLines.Where(x => x.Id == rtn[i].CustomerLineId).FirstOrDefault().CustomerLineName;
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
                            Password = StringCipher.DecryptString(u.Password),
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
            rtn.CustomerLineId = GetCustomerLineIdByUserId(uid);
            if (rtn.CustomerLineId == 0)
            {
                rtn.CustomerLineText = "-";
            }
            else
            {
                rtn.CustomerLineText = _DailyLoanContext.CustomerLines.Where(x => x.Id == rtn.CustomerLineId).FirstOrDefault().CustomerLineName;
            }
            return rtn;
        }
        public async Task<int> EditUser(User req)
        {
            if (req.Id == 0)
                _DailyLoanContext.Users.Add(req);
            else
            {
                var user = await _DailyLoanContext.Users.FindAsync(req.Id);
                if (user == null)
                {
                    return await Task.FromResult(0);
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
                user.UpdateBy = req.UpdateBy;
                user.UpdateDate = req.UpdateDate;
            }
            if(await _DailyLoanContext.SaveChangesAsync()>0)return req.Id;
            return 0;
        }
        public async Task<bool> EditUserCustomerLine(List<int> data)
        {
            UserPermission up = _DailyLoanContext.UserPermissions.Where(x => x.UserId==data[0]).FirstOrDefault();
            if(up != null)
                _DailyLoanContext.UserPermissions.Remove(up);
            UserPermission newup = new UserPermission() 
            { 
                UserId = data[0],
                CustomerLineId = data[1],
                CreateBy = data[2],
                CreateDate = DateTime.Now
            };
            _DailyLoanContext.UserPermissions.Add(newup);
            return (await _DailyLoanContext.SaveChangesAsync()) > 0;
        }
        public async Task<bool> DeleteUser(int uid)
        {
            _DailyLoanContext.Users.Remove(_DailyLoanContext.Users.Where(x => x.Id==uid).FirstOrDefault());
            return (await _DailyLoanContext.SaveChangesAsync()) > 0;
        }
        #endregion
        #region Customer
        public async Task<List<ManagementCustomer>> GetAllCustomer(int uid, int useraccess)
        {
            List< ManagementCustomer> rtn = await (from c in _DailyLoanContext.Customers
                                            join us in _DailyLoanContext.StatusCustomers on c.Status equals us.Id
                                            join cl in _DailyLoanContext.CustomerLines on c.CustomerLineId equals cl.Id
                                            join h in _DailyLoanContext.Houses on cl.HouseId equals h.Id
                                            select new ManagementCustomer()
                                            {
                                                Id = c.Id,
                                                Firstname = c.Firstname,
                                                Lastname = c.Lastname,
                                                Nickname = c.Nickname,
                                                Phone1 = c.Phone1,
                                                Phone2 = c.Phone2,
                                                Status = c.Status,
                                                Idcard = c.Idcard,
                                                Address = c.Address,
                                                ShortAddress = c.ShortAddress,
                                                CustomerLineId = c.CustomerLineId,
                                                CreateBy = c.CreateBy,
                                                CreateDate = c.CreateDate,
                                                UpdateBy = c.UpdateBy,
                                                UpdateDate = c.UpdateDate,
                                                StatusText = us.Status,
                                                HouseId = h.Id,
                                                HouseText = h.HouseName,
                                                CustomerLineText = cl.CustomerLineName
                                            }).ToListAsync();
            if (useraccess != StatusUserAccess.UserAccess_Superadmin)
                rtn = rtn.Where(x => x.HouseId == GetHouseIdByUserId(uid)).ToList();
            return rtn;
        }
        public ManagementCustomer GetCustomer(int cid)
        {
            ManagementCustomer rtn = (from c in _DailyLoanContext.Customers
                                  join us in _DailyLoanContext.StatusCustomers on c.Status equals us.Id
                                  join cl in _DailyLoanContext.CustomerLines on c.CustomerLineId equals cl.Id
                                  join h in _DailyLoanContext.Houses on cl.HouseId equals h.Id
                                  where c.Id == cid
                                  select new ManagementCustomer()
                                  {
                                      Id = c.Id,
                                      Firstname = c.Firstname,
                                      Lastname = c.Lastname,
                                      Nickname = c.Nickname,
                                      Phone1 = c.Phone1,
                                      Phone2 = c.Phone2,
                                      Status = c.Status,
                                      Idcard = c.Idcard,
                                      Address = c.Address,
                                      ShortAddress = c.ShortAddress,
                                      CustomerLineId = c.CustomerLineId,
                                      CreateBy = c.CreateBy,
                                      CreateDate = c.CreateDate,
                                      UpdateBy = c.UpdateBy,
                                      UpdateDate = c.UpdateDate,
                                      StatusText = us.Status,
                                      HouseId = h.Id,
                                      HouseText = h.HouseName,
                                      CustomerLineText = cl.CustomerLineName
                                  }).FirstOrDefault();
            return rtn;
        }
        public async Task<bool> EditCustomer(Customer req)
        {
            if (req.Id == 0)
                _DailyLoanContext.Customers.Add(req);
            else
            {
                var cus = await _DailyLoanContext.Customers.FindAsync(req.Id);
                if (cus == null)
                {
                    return await Task.FromResult(false);
                }
                cus.Idcard = req.Idcard;
                cus.Firstname = req.Firstname;
                cus.Lastname = req.Lastname;
                cus.Nickname = req.Nickname;
                cus.Address = req.Address;
                cus.ShortAddress = req.ShortAddress;
                cus.Phone1 = req.Phone1;
                cus.Phone2 = req.Phone2;
                cus.Status = req.Status;
                cus.CustomerLineId = req.CustomerLineId;
                cus.UpdateBy = req.UpdateBy;
                cus.UpdateDate = req.UpdateDate;
            }
            return (await _DailyLoanContext.SaveChangesAsync()) > 0;
        }
        public async Task<bool> DeleteCustomer(int cid)
        {
            _DailyLoanContext.Customers.Remove(_DailyLoanContext.Customers.Where(x => x.Id == cid).FirstOrDefault());
            return (await _DailyLoanContext.SaveChangesAsync()) > 0;
        }
        #endregion
        #region House
        public async Task<List<ManagementHouse>> GetAllHouseList()
        {
            List<ManagementHouse> rtn = await (from h in _DailyLoanContext.Houses
                                               join us in _DailyLoanContext.StatusHouses on h.Status equals us.Id
                                                  select new ManagementHouse()
                                                  {
                                                      Id = h.Id,
                                                      HouseName = h.HouseName,
                                                      Address = h.Address,
                                                      Status = h.Status,
                                                      Province = h.Province,
                                                      District = h.District,
                                                      SubDistrict = h.SubDistrict,
                                                      Remark = h.Remark,
                                                      CreateBy = h.CreateBy,
                                                      CreateDate = h.CreateDate,
                                                      UpdateBy = h.UpdateBy,
                                                      UpdateDate = h.UpdateDate,
                                                      StatusText = us.Status,
                                                  }).ToListAsync();
            return rtn;
        }
        public ManagementHouse GetHouse(int hid)
        {
            ManagementHouse rtn = (from h in _DailyLoanContext.Houses
                                   join us in _DailyLoanContext.StatusHouses on h.Status equals us.Id
                                   where h.Id == hid
                                      select new ManagementHouse()
                                      {
                                          Id = h.Id,
                                          HouseName = h.HouseName,
                                          Address = h.Address,
                                          Status = h.Status,
                                          Province = h.Province,
                                          District = h.District,
                                          SubDistrict = h.SubDistrict,
                                          Remark = h.Remark,
                                          CreateBy = h.CreateBy,
                                          CreateDate = h.CreateDate,
                                          UpdateBy = h.UpdateBy,
                                          UpdateDate = h.UpdateDate,
                                          StatusText = us.Status,
                                      }).FirstOrDefault();
            return rtn;
        }
        public async Task<bool> EditHouse(House req)
        {
            bool isAdd = false,isDone = false;
            if (req.Id == 0)
            {
                _DailyLoanContext.Houses.Add(req);
                isAdd = true;
            }
            else
            {
                var house = await _DailyLoanContext.Houses.FindAsync(req.Id);
                if (house == null)
                {
                    return await Task.FromResult(false);
                }
                house.HouseName = req.HouseName;
                house.Address = req.Address;
                house.Province = req.Province;
                house.District = req.District;
                house.SubDistrict = req.SubDistrict;
                house.Remark = req.Remark;
                house.Status = req.Status;
                house.UpdateBy = req.UpdateBy;
                house.UpdateDate = req.UpdateDate;
            }
            isDone = (await _DailyLoanContext.SaveChangesAsync()) > 0;
            if (isAdd)
            {
                List<Config> configs = new List<Config>();
                var init = new InitialConfig();
                foreach (var key in init.config_template.Keys)
                {
                    configs.Add(new Config()
                    {
                        HouseId = req.Id,
                        Name = key,
                        Value = init.config_template[key],
                        CreateBy = req.CreateBy,
                        CreateDate = req.CreateDate
                    });
                }
                await _DailyLoanContext.Configs.AddRangeAsync(configs);
                isDone = (await _DailyLoanContext.SaveChangesAsync()) > 0;
            }
            return isDone;
        }
        public async Task<bool> DeleteHouse(int hid)
        {
            _DailyLoanContext.Houses.Remove(_DailyLoanContext.Houses.Where(x => x.Id == hid).FirstOrDefault());
            _DailyLoanContext.Configs.RemoveRange(await _DailyLoanContext.Configs.Where(x => x.HouseId == hid).ToListAsync());
            return (await _DailyLoanContext.SaveChangesAsync()) > 0;
        }
        #endregion
        #region CustomerLine
        public async Task<List<ManagementCustomerLine>> GetAllCustomerLineList(int uid, int useraccess)
        {
            List<ManagementCustomerLine> rtn = await (from cl in _DailyLoanContext.CustomerLines
                                                      join us in _DailyLoanContext.StatusHouses on cl.Status equals us.Id
                                                      join h in _DailyLoanContext.Houses on cl.HouseId equals h.Id
                                               select new ManagementCustomerLine()
                                               {
                                                   Id = cl.Id,
                                                   CustomerLineName = cl.CustomerLineName,
                                                   HouseId = cl.HouseId,
                                                   Status = cl.Status,
                                                   Remark = cl.Remark,
                                                   CreateBy = cl.CreateBy,
                                                   CreateDate = cl.CreateDate,
                                                   UpdateBy = cl.UpdateBy,
                                                   UpdateDate = cl.UpdateDate,
                                                   StatusText = us.Status,
                                                   HouseText = h.HouseName
                                               }).ToListAsync();
            if (useraccess != StatusUserAccess.UserAccess_Superadmin)
                rtn = rtn.Where(x => x.HouseId == GetHouseIdByUserId(uid)).ToList();
            return rtn;
        }
        public ManagementCustomerLine GetCustomerLine(int clid)
        {
            ManagementCustomerLine rtn = (from cl in _DailyLoanContext.CustomerLines
                                          join us in _DailyLoanContext.StatusHouses on cl.Status equals us.Id
                                          join h in _DailyLoanContext.Houses on cl.HouseId equals h.Id
                                          where cl.Id == clid
                                   select new ManagementCustomerLine()
                                   {
                                       Id = cl.Id,
                                       CustomerLineName = cl.CustomerLineName,
                                       HouseId = cl.HouseId,
                                       Status = cl.Status,
                                       Remark = cl.Remark,
                                       CreateBy = cl.CreateBy,
                                       CreateDate = cl.CreateDate,
                                       UpdateBy = cl.UpdateBy,
                                       UpdateDate = cl.UpdateDate,
                                       StatusText = us.Status,
                                       HouseText = h.HouseName
                                   }).FirstOrDefault();
            return rtn;
        }
        public async Task<bool> EditCustomerLine(CustomerLine req)
        {
            if (req.Id == 0)
                _DailyLoanContext.CustomerLines.Add(req);
            else
            {
                var house = await _DailyLoanContext.CustomerLines.FindAsync(req.Id);
                if (house == null)
                {
                    return await Task.FromResult(false);
                }
                house.CustomerLineName = req.CustomerLineName;
                house.HouseId = req.HouseId;
                house.Remark = req.Remark;
                house.Status = req.Status;
                house.UpdateBy = req.UpdateBy;
                house.UpdateDate = req.UpdateDate;
            }
            return (await _DailyLoanContext.SaveChangesAsync()) > 0;
        }
        public async Task<bool> DeleteCustomerLine(int clid)
        {
            _DailyLoanContext.CustomerLines.Remove(_DailyLoanContext.CustomerLines.Where(x => x.Id == clid).FirstOrDefault());
            return (await _DailyLoanContext.SaveChangesAsync()) > 0;
        }
        #endregion
        #region Config
        public async Task<ManagementConfig> GetConfig(int hid)
        {
            List<Config> conf = await _DailyLoanContext.Configs.Where(x => x.HouseId == hid).ToListAsync();
            ManagementConfig rtn = new ManagementConfig();
            rtn.Configs = new Dictionary<string, string>();
            rtn.SpecialRates = await _DailyLoanContext.SpecialRates.Where(x => x.HouseId == hid).ToListAsync();
            for(int i = 0;i < conf.Count(); i++)
            {
                rtn.Configs.Add(conf[i].Name, conf[i].Value);
            }
            return rtn;
        }
        public SpecialRate GetSpecialRate(int spid)
        {
            return _DailyLoanContext.SpecialRates.Where(x => x.Id == spid).FirstOrDefault();
        }
        public async Task<bool> EditConfig(Dictionary<string,string> req,int hid,int uid)
        {
            List<Config> confs = await _DailyLoanContext.Configs.Where(x => x.HouseId==hid).ToListAsync();
            if (confs == null)
            {
                return await Task.FromResult(false);
            }
            for (int i = 0; i < confs.Count(); i++)
            {
                confs[i].Value = req[confs[i].Name];
                confs[i].UpdateBy = uid;
                confs[i].UpdateDate = DateTime.Now;
            }
            return (await _DailyLoanContext.SaveChangesAsync()) > 0;
        }
        public async Task<bool> EditSpecialRate(SpecialRate req)
        {
            if (req.Id == 0)
                _DailyLoanContext.SpecialRates.Add(req);
            else
            {
                var special = await _DailyLoanContext.SpecialRates.FindAsync(req.Id);
                if (special == null)
                {
                    return await Task.FromResult(false);
                }
                special.StartDate = req.StartDate;
                special.EndDate = req.EndDate;
                special.CustomerRate = req.CustomerRate;
                special.AgentRate = req.AgentRate;
                special.HouseRate = req.HouseRate;
                special.MinCutDay = req.MinCutDay;
                special.UpdateBy = req.UpdateBy;
                special.UpdateDate = req.UpdateDate;
            }
            return (await _DailyLoanContext.SaveChangesAsync()) > 0;
        }
        public async Task<bool> DeleteSpecialRate(int spid)
        {
            _DailyLoanContext.SpecialRates.Remove(_DailyLoanContext.SpecialRates.Where(x => x.Id == spid).FirstOrDefault());
            return (await _DailyLoanContext.SaveChangesAsync()) > 0;
        }
        #endregion

    }
}
 