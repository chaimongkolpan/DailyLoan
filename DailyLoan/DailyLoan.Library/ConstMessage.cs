using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyLoan.Library
{
    public static class ConstMessage
    {
        public const string Controller_Home = "Home";
        public const string Controller_Management = "Management";
        public const string Controller_Operation = "Operation";
        public const string Controller_Pay = "Pay";

        public const string View_Index = "Index";
        public const string View_MNM_Config = "Config";
        public const string View_MNM_Contract = "Contract";
        public const string View_MNM_Customer = "Customer";
        public const string View_MNM_House = "House";
        public const string View_MNM_CustomerLine = "CustomerLine";
        public const string View_MNM_User = "User";

        public const string Message_InValidID = "เลขประประชาชนไม่ถูกต้อง";
        public const string Message_DonotHavePermission = "คุณไม่มีสิทธิ์";
        public const string Message_Successful = "สำเร็จ";
        public const string Message_SomethingWentWrong = "มีบางอย่างไม่ถูกต้อง";
        public const string Message_UsernameIsExist = "ชื่อผู้ใช้นี้ถูกใช้ไปแล้ว";
        public const string Message_PasswordNotMatch = "รหัสผ่านไม่ตรงกัน";

        public const string Login_UserNamePasswordNotMatching = "User Name หรือ Password ไม่ถูกต้อง";

    }
    public class InitialConfig
    {
        public Dictionary<string, string> config_template = new Dictionary<string, string>() {
            { "CustomerRate", "90" },
            { "AgentRate", "5" },
            { "HouseRate", "5" },
            { "MinCutDay", "6" },
            { "IncCutCriteria", "3" },
            { "DecCutCriteria", "3" },
            { "DecCutPercen", "80" },
            { "SpecialRateCriteria", "3" },
            { "TotalProfit", "20" },
            { "NotPayAlert", "3" },
            { "PartialPayAlert", "3" }
        };
        public Dictionary<string, string> config_th = new Dictionary<string, string>() {
            { "CustomerRate", "เปอร์เซ็นต์จ่ายลูกค้า" },
            { "AgentRate", "เปอร์เซ็นต์ค่าหัว" },
            { "HouseRate", "เปอร์เซ็นต์หักเข้าบ้าน" },
            { "MinCutDay", "จำนวนวันตัดขั้นต่ำ" },
            { "IncCutCriteria", "เงื่อนไขตัดเพิ่ม(กี่ตัด)" },
            { "DecCutCriteria", "เงื่อนไขตัดลด(กี่ตัด)" },
            { "DecCutPercen", "เงื่อนไขตัดลด(ยอดจ่าย %)" },
            { "SpecialRateCriteria", "จำนวนจ่ายเรตพิเศษ(วัน)" },
            { "TotalProfit", "กำไร(%)" },
            { "NotPayAlert", "เงื่อนไขแจ้งเตือไม่จ่าย(ครั้ง)" },
            { "PartialPayAlert", "เงื่อนไขแจ้งเตือนจ่ายไม่ครบ(ครั้ง)" }
        };
    }
}
