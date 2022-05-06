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
        public double? cutamount { get; set; }
        public double? openamount { get; set; }
        public int cutcount { get; set; }
        public int opencount { get; set; }
        public List<collectTransaction> transactions { get; set; }
    }
    public class collectTransaction
    {
        public string customername { get; set; }
        public string guarantorname { get; set; }
        public string address { get; set; }
        public double? mustcollect { get; set; }
        public double? collect { get; set; }
        public int type { get; set; }

    }
}
