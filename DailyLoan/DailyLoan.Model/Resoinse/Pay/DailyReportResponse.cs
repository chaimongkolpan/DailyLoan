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
        public double? collectall { get; set; }
        public double? collect { get; set; }
        public double? housepayout { get; set; }
        public double? paytocustomer { get; set; }
        public double? mustcollect { get; set; }
        public double? sumexpense { get; set; }
        public double? mustreturn { get; set; }
    }
}
