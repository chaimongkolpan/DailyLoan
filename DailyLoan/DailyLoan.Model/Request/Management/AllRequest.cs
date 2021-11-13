using System;
using System.Collections.Generic;
using System.Text;
using DailyLoan.Model.Entities.DailyLoan;

namespace DailyLoan.Model.Request.Management
{
    public class EditUserRequest : User
    {
        public bool isNew { get; set; }
        public string ConfirmPassword { get; set; }
        public int CustomerLineId { get; set; }
        public User ToUser(int uid,int hid)
        {
            User u = new User()
            {
                Id = this.Id,
                Username = this.Username,
                Password = this.Password,
                UserAccess = this.UserAccess,
                Firstname = this.Firstname,
                Lastname = this.Lastname,
                Nickname = this.Nickname,
                Phone1 = this.Phone1,
                Phone2 = this.Phone2,
                Status = this.Status,
                HouseId = this.HouseId
            };
            if(u.HouseId == 0)
            {
                u.HouseId = hid;
            }
            if (this.isNew)
            {
                u.CreateBy = uid;
                u.CreateDate = DateTime.Now;
            }
            else
            {
                u.CreateBy = this.CreateBy;
                u.CreateDate = this.CreateDate;
                u.UpdateBy = uid;
                u.UpdateDate = DateTime.Now;
            }
            return u;
        }
        public bool isNewCustomerLine()
        {
            return this.CustomerLineId != 0;
        }
        public List<int> ToUserPermission(int uid)
        {
            List<int> rtn = new List<int>();
            rtn.Add(this.Id);
            rtn.Add(this.CustomerLineId);
            rtn.Add(uid);
            return rtn;
        }
    }
    public class EditCustomerRequest : Customer
    {
        public bool isNew { get; set; }
        public Customer ToCustomer(int uid)
        {
            Customer u = new Customer()
            {
                Id = this.Id,
                Firstname = this.Firstname,
                Lastname = this.Lastname,
                Nickname = this.Nickname,
                Phone1 = this.Phone1,
                Phone2 = this.Phone2,
                Status = this.Status,
                Idcard = this.Idcard,
                Address = this.Address,
                ShortAddress = this.ShortAddress,
                CustomerLineId = this.CustomerLineId
            };
            if (this.isNew)
            {
                u.CreateBy = uid;
                u.CreateDate = DateTime.Now;
            }
            else
            {
                u.CreateBy = this.CreateBy;
                u.CreateDate = this.CreateDate;
                u.UpdateBy = uid;
                u.UpdateDate = DateTime.Now;
            }
            return u;
        }
    }
    public class EditHouseRequest : House
    {
        public bool isNew { get; set; }
        public House ToHouse(int uid)
        {
            House u = new House()
            {
                Id = this.Id,
                HouseName = this.HouseName,
                Status = this.Status,
                Province = this.Province,
                District = this.District,
                SubDistrict = this.SubDistrict,
                Address = this.Address,
                Remark = this.Remark,
            };
            if (this.isNew)
            {
                u.CreateBy = uid;
                u.CreateDate = DateTime.Now;
            }
            else
            {
                u.CreateBy = this.CreateBy;
                u.CreateDate = this.CreateDate;
                u.UpdateBy = uid;
                u.UpdateDate = DateTime.Now;
            }
            return u;
        }
    }
    public class EditCustomerLineRequest : CustomerLine
    {
        public bool isNew { get; set; }
        public CustomerLine ToCustomerLine(int uid,int hid)
        {
            CustomerLine u = new CustomerLine()
            {
                Id = this.Id,
                CustomerLineName = this.CustomerLineName,
                Status = this.Status,
                HouseId = this.HouseId,
                Remark = this.Remark,
            };
            if (u.HouseId == 0)
            {
                u.HouseId = hid;
            }
            if (this.isNew)
            {
                u.CreateBy = uid;
                u.CreateDate = DateTime.Now;
            }
            else
            {
                u.CreateBy = this.CreateBy;
                u.CreateDate = this.CreateDate;
                u.UpdateBy = uid;
                u.UpdateDate = DateTime.Now;
            }
            return u;
        }
    }
    public class EditSpecialRateRequest : SpecialRate
    {
        public bool isNew { get; set; }
        public SpecialRate ToSpecialRate(int uid)
        {
            SpecialRate u = new SpecialRate()
            {
                Id = this.Id,
                HouseId = this.HouseId,
                StartDate = this.StartDate,
                EndDate = this.EndDate,
                CustomerRate = this.CustomerRate,
                AgentRate = this.AgentRate,
                HouseRate = this.HouseRate,
                MinCutDay = this.MinCutDay
            };
            if (this.isNew)
            {
                u.CreateBy = uid;
                u.CreateDate = DateTime.Now;
            }
            else
            {
                u.CreateBy = this.CreateBy;
                u.CreateDate = this.CreateDate;
                u.UpdateBy = uid;
                u.UpdateDate = DateTime.Now;
            }
            return u;
        }
    }
    public class EditConfigRequest
    {
        public int HouseId { get; set; }
        public string CustomerRate { get; set; }
        public string AgentRate { get; set; }
        public string HouseRate { get; set; }
        public string MinCutDay { get; set; }
        public string IncCutCriteria { get; set; }
        public string DecCutCriteria { get; set; }
        public string DecCutPercen { get; set; }
        public string SpecialRateCriteria { get; set; }
        public string TotalProfit { get; set; }
        public string NotPayAlert { get; set; }
        public string PartialPayAlert { get; set; }
    }
}
