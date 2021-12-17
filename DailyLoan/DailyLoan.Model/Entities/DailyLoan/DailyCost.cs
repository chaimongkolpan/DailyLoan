using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DailyLoan.Model.Entities.DailyLoan
{
    public partial class DailyCost
    {
        public int Id { get; set; }
        public int HouseId { get; set; }
        public int CustomerLineId { get; set; }
        public DateTime? Date { get; set; }
        public double? PayOut { get; set; }
        public double? Receive { get; set; }
        public double? Allowance { get; set; }
        public double? Police1 { get; set; }
        public string PoliceRemark1 { get; set; }
        public double? Police2 { get; set; }
        public string PoliceRemark2 { get; set; }
        public double? Police3 { get; set; }
        public string PoliceRemark3 { get; set; }
        public double? Gas { get; set; }
        public double? Topup { get; set; }
        public double? Caught { get; set; }
        public double? BikeMaintenance { get; set; }
        public double? Other { get; set; }
        public string OtherDetail { get; set; }
        public string Remark { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public double? OtherIncome { get; set; }
        public string OtherIncomeRemark { get; set; }
    }
}
