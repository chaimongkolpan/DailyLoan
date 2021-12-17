using System;
using System.Collections.Generic;
using System.Text;

namespace DailyLoan.Library.Status
{
    public class TransactionType_Status
    {
        public const int CollectFromCustomer = 1;
        public const int Bounty = 2;
        public const int PayToCustomer = 3;
        public const int SaveCollectFromCustomer = 4;
        public const int CollectFromCustomerCut = 5;
    }
    public class Transaction_Status
    {
        public const int Normal = 1;
        public const int PartialPay = 2;
        public const int NotPay = 3;
        public const int Cut = 4;
    }
}
