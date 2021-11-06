using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DailyLoan.Model.Entities.DailyLoan
{
    public partial class CustomerLine
    {
        public int Id { get; set; }
        public string CustomerLineName { get; set; }
        public int? HouseId { get; set; }
        public string Remark { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int Status { get; set; }
    }
}
