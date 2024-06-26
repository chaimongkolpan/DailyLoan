﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DailyLoan.Library.Status
{
    public class ContractStatus
    {
        public const int StatusContract_WaitConfirm = 1;
        public const int StatusContract_Alert = 2;
        public const int StatusContract_Normal = 3;
        public const int StatusContract_Checking = 4;
        public const int StatusContract_Checked = 5;
        public const int StatusContract_Loss = 6;
        public const int StatusContract_Dead = 7;
        public const int StatusContract_Closed = 8;
        public const int StatusContract_NotApprove = 9;
    }
    public class CustomerStatus
    {
        public const int StatusCustomer_Good = 1;
        public const int StatusCustomer_Bad = 2;
        public const int StatusCustomer_Loos = 3;
    }
}
