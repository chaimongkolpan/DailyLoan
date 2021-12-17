using DailyLoan.Models;
using DailyLoan.Library;
using DailyLoan.Library.Status;
using DailyLoan.Model.Request.Home;
using DailyLoan.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace DailyLoan.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ILogInService _loginService;

        public HomeController(ILogger<HomeController> logger, ILogInService loginService)
        {
            _logger = logger;
            _loginService = loginService;
        }

        public IActionResult Index()
        {
            HttpContext.Session.Remove(ConstMessage.Session_UserId);
            HttpContext.Session.Remove(ConstMessage.Session_UserAccess);
            return View("LogIn");
        }

        public IActionResult LogIn()
        {
            HttpContext.Session.Remove(ConstMessage.Session_UserId);
            HttpContext.Session.Remove(ConstMessage.Session_UserAccess);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public ActionResult LogInActionAsync(LogInRequest req)
        {
            var result = _loginService.LogIn(req.Username,StringCipher.EncryptString(req.Password));
            if (result != null)
            {
                HttpContext.Session.SetString(ConstMessage.Session_UserId, result.Id.ToString());
                HttpContext.Session.SetString(ConstMessage.Session_Username, result.Username);
                HttpContext.Session.SetString(ConstMessage.Session_UserAccess, result.UserAccess.ToString());
                if (result.UserAccess <= StatusUserAccess.UserAccess_Admin)
                    return RedirectToAction(ConstMessage.View_MNM_User, ConstMessage.Controller_Management);
                else if(result.UserAccess == StatusUserAccess.UserAccess_Audit)
                    return RedirectToAction(ConstMessage.View_PAY_Customer, ConstMessage.Controller_Pay);
                else
                    return RedirectToAction(ConstMessage.View_PAY_Collector, ConstMessage.Controller_Pay);
            }
            else
            {
                ViewBag.NotValidUser = ConstMessage.Login_UserNamePasswordNotMatching;
                return View("LogIn");
            }
        }
    }
}
