﻿using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DailyLoan.Model.Entities.DailyLoan
{
    public partial class Request
    {
        public int Id { get; set; }
        public int ContractId { get; set; }
        public int ApproverId { get; set; }
        public int AgentId { get; set; }
        public double? Amount { get; set; }
        public int Status { get; set; }
        public int Type { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
