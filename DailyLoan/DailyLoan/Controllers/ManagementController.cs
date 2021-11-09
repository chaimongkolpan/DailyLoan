using DailyLoan.Model;
using DailyLoan.Model.Entities.DailyLoan;
using DailyLoan.Model.Request.Management;
using DailyLoan.Library;
using DailyLoan.Library.Status;
using DailyLoan.Service.Interfaces;
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
        private readonly IManagementService _managementService;
        private readonly IWebHostEnvironment Environment;
        public ManagementController(IWebHostEnvironment _environment, 
            IManagementService managementService)
        {
            _managementService = managementService;
            Environment = _environment;
        }
        public IActionResult Index()
        {
            return View(ConstMessage.View_Index);
        }
        public ActionResult ConfigAction()
        {
            ViewBag.partialView = ConstMessage.View_MNM_Config;
            return View(ConstMessage.View_Index);
        }
        public ActionResult ContractAction()
        {
            ViewBag.partialView = ConstMessage.View_MNM_Contract;
            return View(ConstMessage.View_Index);
        }
        public ActionResult CustomerAction()
        {
            ViewBag.partialView = ConstMessage.View_MNM_Customer;
            return View(ConstMessage.View_Index);
        }
        public async Task<ActionResult> UserActionAsync()
        {
            //var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserId = "1";
            List<ManagementUser> res = await _managementService.GetAllUser(Convert.ToInt32(UserId));
            ViewBag.PageData = res;
            ViewBag.partialView = ConstMessage.View_MNM_User;
            return View(ConstMessage.View_Index);
        }
    }
}
