using System;
using System.Collections.Generic;
using System.Text;
using DailyLoan.Model.Entities.DailyLoan;
using DailyLoan.Library.Status;
using Microsoft.AspNetCore.Http;

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
        public string uploaded0{ get; set; }
        public string uploaded1{ get; set; }
        public string uploaded2{ get; set; }
        public string uploaded3{ get; set; }
        public string uploaded4{ get; set; }
        public string uploaded5{ get; set; }
        public string uploaded6{ get; set; }
        public string uploaded7{ get; set; }
        public string uploaded8{ get; set; }
        public string uploaded9{ get; set; }
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
    public class EditContractRequest : Contract
    {
        public bool isNew { get; set; }
        public string uploaded0 { get; set; }
        public string uploaded1 { get; set; }
        public string uploaded2 { get; set; }
        public string uploaded3 { get; set; }
        public string uploaded4 { get; set; }
        public string uploaded5 { get; set; }
        public string uploaded6 { get; set; }
        public string uploaded7 { get; set; }
        public string uploaded8 { get; set; }
        public string uploaded9 { get; set; }
        public Contract ToContract(int uid,string idcard)
        {
            Contract u = new Contract()
            {
                Id = this.Id,
                CustomerId = this.CustomerId,
                GuarantorId = this.GuarantorId,
                TotalAmount = this.TotalAmount
            };
            if (this.isNew)
            {
                u.CreateBy = uid;
                u.CreateDate = DateTime.Now;
                u.Status = ContractStatus.StatusContract_WaitConfirm;
                u.TotalPay = 0;
                u.SpecialRateCount = 0;
                u.CutCount = 0;
                u.ContractId = idcard + "-" + DateTime.Now.ToString("yyyyMMdd"); 
                u.ApproverId = uid;
            }
            else
            {
                u.Status = this.Status;
                u.TotalPay = this.TotalPay;
                u.SpecialRateCount = this.SpecialRateCount;
                u.CutCount = this.CutCount;
                u.ContractId = this.ContractId;
                u.ApproverId = this.ApproverId;
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
                Region = this.Region,
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
    public class ContractSearchRequest
    {
        public string Idcard { get; set; }
        public string Name { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Address { get; set; }
    }
    public class UploadPic
    {
        public List<IFormFile> uploadfile1 { get; set; }
        public List<IFormFile> uploadfile2 { get; set; }
    }
}
