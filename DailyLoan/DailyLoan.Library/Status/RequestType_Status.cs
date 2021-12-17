using System;
using System.Collections.Generic;
using System.Text;

namespace DailyLoan.Library.Status
{
    public class Request_Type
    {
        public const int NewContract = 1;
        public const int Cut = 2;
        public const int CutDecrease = 3;
        public const int CutIncrease = 4;
    }
    public class Request_Status
    {
        public const int Pending = 1;
        public const int Approve = 2;
        public const int Deny = 3;
    }
    public class Notification_Type
    {
        public const int NotPay = 1;
        public const int OpenContract = 2;
        public const int Cut = 3;
        public const int Other = 4;
    }
    public class Notification_Status
    {
        public const int Alert = 2;
        public const int Checking = 4;
        public const int Checked = 5;
    }
    public class Notification_Message
    {
        public const string NotPay = "ไม่ชำระเงินติดต่อกัน";
        public const string PayPartial = "ชำระเงินไม่ถึง 50% ติดต่อกัน";
    }
}
