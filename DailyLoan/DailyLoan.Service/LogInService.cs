using DailyLoan.Repository.Interfaces;
using DailyLoan.Service.Interfaces;
using DailyLoan.Model.Entities.DailyLoan;

namespace DailyLoan.Service
{
    public class LogInService : ILogInService
    {

        private readonly ILogInRepo _LoginRepo;
        public LogInService(ILogInRepo loginRepo)
        {
            _LoginRepo = loginRepo;
        }
        public User LogIn(string username, string password)
        {
            return _LoginRepo.LogIn(username, password);
        }
    }
}
