using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DailyLoan.Model.Entities.DailyLoan
{
    public partial class Salary
    {
        public int Id { get; set; }
        public int HouseId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int UserId { get; set; }
        public double? Salary1 { get; set; }
        public double? Performance { get; set; }
        public string Remark { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? Date { get; set; }
    }
}
