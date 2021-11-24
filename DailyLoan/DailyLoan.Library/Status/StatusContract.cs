using System;
using System.Collections.Generic;
using System.Text;

namespace DailyLoan.Library.Status
{
    public class ContractStatus
    {
        public const int StatusContract_WaitConfirm = 1;
        public const int StatusContract_Normal = 2;
        public const int StatusContract_Closed = 3;
        public const int StatusContract_Alert = 4;
        public const int StatusContract_Checking = 5;
        public const int StatusContract_Checked = 6;
        public const int StatusContract_Loss = 7;
    }
}
