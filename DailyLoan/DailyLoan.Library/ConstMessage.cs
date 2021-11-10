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
        public const string View_MNM_User = "User";

        public const string Message_DonotHavePermission = "คุณไม่มีสิทธิ์";
        public const string Message_Successful = "สำเร็จ";
        public const string Message_SomethingWentWrong = "มีบางอย่างไม่ถูกต้อง";
        public const string Message_UsernameIsExist = "ชื่อผู้ใช้นี้ถูกใช้ไปแล้ว";
        public const string Message_PasswordNotMatch = "รหัสผ่านไม่ตรงกัน";


        public const string Login_UserNamePasswordNotMatching = "User Name หรือ Password ไม่ถูกต้อง";
    }
}
