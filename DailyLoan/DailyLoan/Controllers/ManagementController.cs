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
            var UserAccess = "1";
            List<ManagementUser> res = await _managementService.GetAllUser(Convert.ToInt32(UserId), Convert.ToInt32(UserAccess));
            ViewBag.UserId = UserId; 
            ViewBag.UserAccess = UserAccess;
            if(Convert.ToInt32(UserAccess) == StatusUserAccess.UserAccess_Superadmin)
                ViewBag.House = await _managementService.GetAllHouse();
            ViewBag.PageData = res;
            ViewBag.partialView = ConstMessage.View_MNM_User;
            return View(ConstMessage.View_Index);
        }
        [HttpGet]
        [Route("GetUserDetail/{uid}")]
        public ActionResult GetUserDetail(int uid)
        {
            return Ok(_managementService.GetUser(uid));
        }
        [HttpGet]
        [Route("DeleteUser/{uid}")]
        public async Task<ActionResult> DeleteUser(int uid)
        {
            var UserAccess = "1";
            if(Convert.ToInt32(UserAccess) <= StatusUserAccess.UserAccess_Admin)
            {
                if(await _managementService.DeleteUser(uid))
                {
                    return Ok(ConstMessage.Message_Successful);
                }
                else return BadRequest(ConstMessage.Message_SomethingWentWrong);
            }
            else return BadRequest(ConstMessage.Message_DonotHavePermission);
        }
        [HttpPost]
        [Route("EditUser")]
        public async Task<ActionResult> EditUser(EditUserRequest req)
        {
            //var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserId = "1";
            if (!string.IsNullOrEmpty(req.Username))
            {
                if (validationpass(req.Password, req.ConfirmPassword))
                {
                    bool isExist = _managementService.UsernameIsExist(req.Username);
                    if ((!isExist&&req.isNew)||(isExist&&!req.isNew))
                    {
                        if (await _managementService.EditUser(req, Convert.ToInt32(UserId)))
                        {
                            return Ok(ConstMessage.Message_Successful);
                        }
                        else return BadRequest(ConstMessage.Message_SomethingWentWrong);
                    }
                    else return BadRequest(ConstMessage.Message_UsernameIsExist);
                }
                else
                {
                    return BadRequest(ConstMessage.Message_PasswordNotMatch);
                }
            }
            else
            {
                return BadRequest(ConstMessage.Message_SomethingWentWrong);
            }
        }
        private bool validationpass(string pass, string confirm)
        {
            if (!string.IsNullOrEmpty(pass) && !string.IsNullOrEmpty(confirm))
            {
                if (pass != confirm)
                    return false;
                else
                    return true;
            }
            else
            {
                return false;
            }
        }
    }
}
