﻿using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DailyLoan.Model.Entities.DailyLoan
{
    public partial class Contract
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int GuarantorId { get; set; }
        public int ApproverId { get; set; }
        public double? TotalAmount { get; set; }
        public double? TotalPay { get; set; }
        public int? SpecialRateCount { get; set; }
        public int? CutCount { get; set; }
        public int Status { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string ContractId { get; set; }
        public string Remark { get; set; }
        public double? ExContractPay { get; set; }
        public int? Type { get; set; }
    }
}
