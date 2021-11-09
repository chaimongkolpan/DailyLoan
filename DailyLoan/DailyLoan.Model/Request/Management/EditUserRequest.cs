using System;
using System.Collections.Generic;
using System.Text;
using DailyLoan.Model.Entities.DailyLoan;

namespace DailyLoan.Model.Request.Management
{
    public class EditUserRequest : User
    {
        public bool isNew { get; set; }
        public User ToUser(int uid)
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
                HouseId = this.HouseId,
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
}
