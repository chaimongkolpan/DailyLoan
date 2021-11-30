using DailyLoan.Repository.Interfaces;
using DailyLoan.Model.Entities.DailyLoan;
using DailyLoan.Model;
using System.Linq;

namespace DailyLoan.Repository
{
    public class LogInRepo : ILogInRepo
    {
        private readonly DailyLoanContext _DailyLoanContext;
        public LogInRepo(DailyLoanContext dailyLoanContext)
        {
            _DailyLoanContext = dailyLoanContext;
        }
        public User LogIn(string username,string password)
        {
            var rtn = _DailyLoanContext.User.Where(x => x.Username == username&&x.Password == password&&x.Status==1).FirstOrDefault();
            return rtn;
        }
    }
}
