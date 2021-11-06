using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DailyLoan.Library
{
    public static class ValidationIDCardNo
    {
        public static bool IsValidCheckPersonID(string pid)
        {
            if (string.IsNullOrEmpty(pid))
                return false;

            Regex rexPersonal = new Regex(@"^[0-9]{13}$");
            if (rexPersonal.IsMatch(pid))
            {
                int sum = 0;

                /* x13, x12, x11, ... */
                for (int i = 0; i < 12; i++)
                {
                    sum += int.Parse(pid[i].ToString()) * (13 - i);
                }

                /* complements(12, sum mod 11) */
                return int.Parse(pid[12].ToString()) == ((11 - (sum % 11)) % 10);
            }
            return false;

        }
    }
}
