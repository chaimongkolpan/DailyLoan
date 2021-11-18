using DailyLoan.Model;
using DailyLoan.Models;
/*using DailyLoan.Library;
using DailyLoan.Library.Status;
using DailyLoan.Model.Request.LogIn;
using DailyLoan.Service.Interfaces;*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using DailyLoan.Model.Request.Home;
using DailyLoan.Library;

namespace DailyLoan.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View("LogIn");
        }

        public IActionResult LogIn()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<ActionResult> LogInActionAsync(LogInRequest req)
        {
            return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Management);
            //var result = await _loginService.LoginValidation(req.Username, req.Password);
            //if (result != null)
            //{
            //    return View(ConstMessage.View_Index);
            //}
            //else
            //{
            //    ViewBag.NotValidUser = ConstMessage.Login_UserNamePasswordNotMatching;
            //    return View(ConstMessage.View_Index);
            //}
        }
    }
}
