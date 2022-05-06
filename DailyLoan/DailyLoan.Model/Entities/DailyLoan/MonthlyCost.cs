using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DailyLoan.Model.Entities.DailyLoan
{
    public partial class MonthlyCost
    {
        public int Id { get; set; }
        public int HouseId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public double? HouseRent { get; set; }
        public double? Water { get; set; }
        public double? Electric { get; set; }
        public double? Internet { get; set; }
        public double? PaperInk { get; set; }
        public double? GodFee { get; set; }
        public double? Banquet { get; set; }
        public string BanquetRemark { get; set; }
        public double? VehicleCost { get; set; }
        public string VehicleRemark { get; set; }
        public double? Other { get; set; }
        public string OtherRemark { get; set; }
        public string Remark { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? Date { get; set; }
    }
}
