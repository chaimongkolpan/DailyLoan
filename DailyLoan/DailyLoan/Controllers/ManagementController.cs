using DailyLoan.Model;
/*using DailyLoan.Library;
using DailyLoan.Library.Status;
using DailyLoan.Model.Request.LogIn;
using DailyLoan.Service.Interfaces;*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace DailyLoan.Controllers
{
    public class ManagementController : Controller
    {
        //private readonly IMasterServices _masterService;
        //private readonly ILoginServices _loginService;
        //private readonly IProjectServices _projectService;
        private IWebHostEnvironment Environment;
        public ManagementController(IWebHostEnvironment _environment)
        {
            //_masterService = masterService;
            //_loginService = loginService;
            //_projectService = projectService;
            Environment = _environment;
        }
        public IActionResult Index()
        {
            return Redirect("~/Management/User");
        }
    }
}
