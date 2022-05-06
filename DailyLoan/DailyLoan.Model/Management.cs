using System;
using System.Collections.Generic;
using System.Text;
using DailyLoan.Model.Entities.DailyLoan;

namespace DailyLoan.Model
{
    public class ManagementUser : User
    {
        public string StatusText { get; set; }
        public string AccessText { get; set; }
        public string HouseText { get; set; }
        public string CustomerLineText { get; set; }
        public int CustomerLineId { get; set; }
    }
    public class ManagementCustomer : Customer
    {
        public string StatusText { get; set; }
        public string HouseText { get; set; }
        public int HouseId { get; set; }
        public string CustomerLineText { get; set; }
        public string[] Images { get; set; }
    }
    public class ManagementCustomerLine : CustomerLine
    {
        public string StatusText { get; set; }
        public string HouseText { get; set; }
    }
    public class ManagementHouse : House
    {
        public string StatusText { get; set; }
    }
    public class ManagementContract : Contract
    {
        public bool iscollect { get; set; }
        public string StatusText { get; set; }
        public string TypeText { get; set; }
        public double Bounty { get; set; }
        public double Special { get; set; }
        //public double PaytoCustomer { get { return (double)(TotalAmount - Collect - Bounty - Special); } }
        public double PaytoCustomer { get; set; }
        public double Collect { get; set; }
        public string ApproverName { get; set; }
        public ManagementCustomer Customer { get; set; }
        public ManagementCustomer Guaruntor { get; set; }
        public string[] Images { get; set; }
    }
    public class ManagementCut
    {
        public int hid { get; set; }
        public double installment { get; set; }
        public double profitrate { get; set; }
        public double totalamount { get; set; }
        public double totalprofit { get { return (totalamount + (totalamount * profitrate / 100)); } }
        public double totalpay { get; set; }
        public int payday { get { return (int)Math.Floor(totalpay/ installment); } }
        public string paypercen { get { return (totalpay % installment).ToString(); } }
        public double remain { get { return (totalprofit - totalpay); } }
        public double cutcount { get; set; }
    }
    public class ManagementWarn : Notification
    {
        public string StatusText { get; set; }
        public ManagementHistory Contract { get; set; }
    }
    public class ManagementCollector : Contract
    {
        public string StatusText { get; set; }
        public string ApproverName { get; set; }
        public ManagementCustomer Customer { get; set; }
        public ManagementCustomer Guaruntor { get; set; }
        public string[] Images { get; set; }
        public List<Transaction> History { get; set; }
        public bool canCut { get; set; }
        public bool canIncCut { get; set; }
        public bool canDecCut { get; set; }
        public double remainCut { get; set; }
        public double remainIncCut { get; set; }
        public double remainDecCut { get; set; }
        public ManagementCollector(ManagementContract contract)
        {
            this.StatusText = contract.StatusText;
            this.ApproverName = contract.ApproverName;
            this.Customer = contract.Customer;
            this.Guaruntor = contract.Guaruntor;
            this.Images = contract.Images;
            this.Id = contract.Id;
            this.CustomerId = contract.CustomerId;
            this.GuarantorId = contract.GuarantorId;
            this.CreateDate = contract.CreateDate;
            this.TotalAmount = contract.TotalAmount;
            this.TotalPay = contract.TotalPay;
            this.Status = contract.Status;
            this.TotalPay = contract.TotalPay;
            this.SpecialRateCount = contract.SpecialRateCount;
            this.CutCount = contract.CutCount;
            this.ContractId = contract.ContractId;
            this.ApproverId = contract.ApproverId;
            this.CreateBy = contract.CreateBy;
            this.CreateDate = contract.CreateDate;
        }
    }
    public class ManagementHistory : Contract
    {
        public string StatusText { get; set; }
        public string ApproverName { get; set; }
        public ManagementCustomer Customer { get; set; }
        public ManagementCustomer Guaruntor { get; set; }
        public string[] Images { get; set; }
        public List<Transaction> History { get; set; }
        public bool canCut { get; set; }
        public bool canIncCut { get; set; }
        public bool canDecCut { get; set; }
        public ManagementHistory(ManagementContract contract)
        {
            this.StatusText = contract.StatusText;
            this.ApproverName = contract.ApproverName;
            this.Customer = contract.Customer;
            this.Guaruntor = contract.Guaruntor;
            this.Images = contract.Images;
            this.Id = contract.Id;
            this.CustomerId = contract.CustomerId;
            this.GuarantorId = contract.GuarantorId;
            this.CreateDate = contract.CreateDate;
            this.TotalAmount = contract.TotalAmount;
            this.TotalPay = contract.TotalPay;
            this.Status = contract.Status;
            this.TotalPay = contract.TotalPay;
            this.SpecialRateCount = contract.SpecialRateCount;
            this.CutCount = contract.CutCount;
            this.ContractId = contract.ContractId;
            this.ApproverId = contract.ApproverId;
            this.CreateBy = contract.CreateBy;
            this.CreateDate = contract.CreateDate;
        }
    }
    public class ManagementConfig
    {
        public Dictionary<string,string> Configs { get; set; }
        public List<SpecialRate> SpecialRates { get; set; }
    }
    public class MonthlyInput : MonthlyCost
    {
        public List<UserSalary> Users { get; set; }
        public string jsonUser { get; set; }
    }
    public class UserSalary
    {
        public int HouseId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public int Useraccess { get; set; }
        public string UseraccessText { get; set; }
        public int Workdays { get; set; }
        public double TotalCollect { get; set; }
        public double Salary { get; set; }
        public double Performance { get; set; }
    }
    public class MonthlyReport
    {
        public string Name { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public double salary { get; set; }
        public double houserent { get; set; }
        public double water { get; set; }
        public double electric { get; set; }
        public double internet { get; set; }
        public double paperink { get; set; }
        public double godfee { get; set; }
        public double banquet { get; set; }
        public double vehicle { get; set; }
        public double other { get; set; }
        public double totalhouseexpenses { get; set; }
        public List<CustomerLineMonthly> customerlines { get; set; }
    }
    public class CustomerLineMonthly
    {
        public string Name { get; set; }
        public double paytocustomer { get; set; }
        public double mustcollect { get; set; }
        public double collect { get; set; }
        public double otherincome { get; set; }
        public double loss { get; set; }
        public double profit { get; set; }
        public double allowance { get; set; }
        public double bounty { get; set; }
        public double salary1 { get; set; }
        public double salary2 { get; set; }
        public double totalemploy { get; set; }
        public double bike { get; set; }
        public double gas { get; set; }
        public double topup { get; set; }
        public double police { get; set; }
        public double caught { get; set; }
        public double other { get; set; }
        public double totalexpenses { get; set; }
        public double cut { get; set; }
        public double open { get; set; }
        public double close { get; set; }
        public double grandtotal { get; set; }
    }
}
