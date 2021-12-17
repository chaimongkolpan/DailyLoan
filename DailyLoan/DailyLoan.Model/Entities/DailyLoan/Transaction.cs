using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DailyLoan.Model.Entities.DailyLoan
{
    public partial class Transaction
    {
        public int Id { get; set; }
        public int ContractId { get; set; }
        public int CustomerLineId { get; set; }
        public double? Amount { get; set; }
        public int Type { get; set; }
        public string Remark { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? PayDate { get; set; }
        public int? FromContractId { get; set; }
        public int? CustomerId { get; set; }
    }
}
