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
    public class PayController : Controller
    {

        private readonly IManagementService _managementService;
        private IWebHostEnvironment Environment;
        public PayController(IWebHostEnvironment _environment,
            IManagementService managementService)
        {
            Environment = _environment;
            _managementService = managementService;
        }

        public IActionResult UserAction()
        {
            return View();
        }
        #region Customer
        public async Task<ActionResult> CustomerActionAsync()
        {
            //var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserId = "1";
            var UserAccess = "1";
            List<ManagementCustomer> res = await _managementService.GetAllCustomer(Convert.ToInt32(UserId), Convert.ToInt32(UserAccess));
            ViewBag.UserId = UserId;
            ViewBag.UserAccess = UserAccess;
            if (Convert.ToInt32(UserAccess) == StatusUserAccess.UserAccess_Superadmin)
                ViewBag.House = await _managementService.GetAllHouse();
            ViewBag.CustomerLine = await _managementService.GetAllCustomerLine(Convert.ToInt32(UserId));
            ViewBag.PageData = res;
            ViewBag.partialView = ConstMessage.View_MNM_Customer;
            return View(ConstMessage.View_Index);
        }
        [HttpGet]
        [Route("GetCustomerDetail/{cid}")]
        public ActionResult GetCustomerDetail(int cid)
        {
            return Ok(_managementService.GetCustomer(cid));
        }
        [HttpGet]
        [Route("DeleteCustomer/{cid}")]
        public async Task<ActionResult> DeleteCustomer(int cid)
        {
            var UserAccess = "1";
            if (Convert.ToInt32(UserAccess) <= StatusUserAccess.UserAccess_Admin)
            {
                if (await _managementService.DeleteCustomer(cid))
                {
                    return Ok(ConstMessage.Message_Successful);
                }
                else return BadRequest(ConstMessage.Message_SomethingWentWrong);
            }
            else return BadRequest(ConstMessage.Message_DonotHavePermission);
        }
        [HttpPost]
        [Route("EditCustomer")]
        public async Task<ActionResult> EditCustomer(EditCustomerRequest req)
        {
            //var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserId = "1";
            if (!ValidationIDCardNo.IsValidCheckPersonID(req.Idcard))
            {
                return BadRequest(ConstMessage.Message_InValidID);
            }
            else
            {
                bool isExist = _managementService.IdcardIsExist(req.Idcard);
                if ((!isExist && req.isNew) || (isExist && !req.isNew))
                {
                    if (await _managementService.EditCustomer(req, Convert.ToInt32(UserId)))
                    {
                        return Ok(ConstMessage.Message_Successful);
                    }
                    else return BadRequest(ConstMessage.Message_SomethingWentWrong);
                }
                else return BadRequest(ConstMessage.Message_UsernameIsExist);
            }
        }
        #endregion
    }
}
