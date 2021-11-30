using DailyLoan.Model.Entities.DailyLoan;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DailyLoan.Service.Interfaces
{
    public interface ILogInService
    {
        User LogIn(string username, string password);
    }
}
