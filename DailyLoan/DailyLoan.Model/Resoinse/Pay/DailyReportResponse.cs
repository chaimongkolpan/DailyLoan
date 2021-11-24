using System;
using System.Collections.Generic;
using System.Text;

namespace DailyLoan.Model.Resoinse.Pay
{
    public class DailyReportResponse
    {
        public int uid { get; set; }
        public DateTime date { get; set; }
        public double? bounty { get; set; }
        public double? allowance { get; set; }
        public double? collect { get; set; }
        public double? mustcollect { get; set; }
        public double? balance { get { return collect - (bounty + allowance); } }
    }
}
