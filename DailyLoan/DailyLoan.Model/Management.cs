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
}
