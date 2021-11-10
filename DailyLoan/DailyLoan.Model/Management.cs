using System;
using System.Collections.Generic;
using System.Text;
using DailyLoan.Model.Entities.DailyLoan;

namespace DailyLoan.Model
{
    public class ManagementUser : User
    {
        public string StatusText { get; set; }
        public string AccessText { get; set; }
        public string HouseText { get; set; }
        public string CustomerLineText { get; set; }
    }
}
