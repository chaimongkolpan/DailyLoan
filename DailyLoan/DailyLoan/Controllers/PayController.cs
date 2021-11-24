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
        private readonly IPayService _payService;
        private IWebHostEnvironment Environment;
        public PayController(IWebHostEnvironment _environment,
            IManagementService managementService,
            IPayService payService)
        {
            Environment = _environment;
            _managementService = managementService;
            _payService = payService;
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
            List<ManagementCustomer> res = await _payService.GetAllCustomer(Convert.ToInt32(UserId), Convert.ToInt32(UserAccess));
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
            return Ok(_payService.GetCustomer(cid));
        }
        [HttpGet]
        [Route("DeleteCustomer/{cid}")]
        public async Task<ActionResult> DeleteCustomer(int cid)
        {
            var UserAccess = "1";
            if (Convert.ToInt32(UserAccess) <= StatusUserAccess.UserAccess_Admin)
            {
                if (await _payService.DeleteCustomer(cid))
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
                    if (await _payService.EditCustomer(req, Convert.ToInt32(UserId)))
                    {
                        return Ok(ConstMessage.Message_Successful);
                    }
                    else return BadRequest(ConstMessage.Message_SomethingWentWrong);
                }
                else return BadRequest(ConstMessage.Message_UsernameIsExist);
            }
        }
        #endregion
        #region Contract
        public async Task<ActionResult> ContractActionAsync()
        {
            //var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserId = "1";
            var UserAccess = "1";
            List<ManagementContract> res = await _payService.GetAllContract(Convert.ToInt32(UserId), Convert.ToInt32(UserAccess));
            ViewBag.UserId = UserId;
            ViewBag.UserAccess = UserAccess;
            if (Convert.ToInt32(UserAccess) == StatusUserAccess.UserAccess_Superadmin)
                ViewBag.House = await _managementService.GetAllHouse();
            ViewBag.CustomerLine = await _managementService.GetAllCustomerLine(Convert.ToInt32(UserId));
            ViewBag.PageData = res;
            ViewBag.partialView = ConstMessage.View_MNM_Contract;
            return View(ConstMessage.View_Index);
        }
        [HttpGet]
        [Route("GetContractDetail/{cid}")]
        public ActionResult GetContractDetail(int cid)
        {
            return Ok(_payService.GetContract(cid));
        }
        [HttpGet]
        [Route("DeleteContract/{cid}")]
        public async Task<ActionResult> DeleteContract(int cid)
        {
            var UserAccess = "1";
            if (Convert.ToInt32(UserAccess) <= StatusUserAccess.UserAccess_Admin)
            {
                if (await _payService.DeleteContract(cid))
                {
                    return Ok(ConstMessage.Message_Successful);
                }
                else return BadRequest(ConstMessage.Message_SomethingWentWrong);
            }
            else return BadRequest(ConstMessage.Message_DonotHavePermission);
        }
        [HttpPost]
        [Route("EditContract")]
        public async Task<ActionResult> EditContract(EditContractRequest req)
        {
            //var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserId = "1";
            if (await _payService.EditContract(req, Convert.ToInt32(UserId)))
            {
                return Ok(ConstMessage.Message_Successful);
            }
            else return BadRequest(ConstMessage.Message_SomethingWentWrong);
        }
        [HttpPost]
        [Route("SearchCustomer")]
        public ActionResult SearchCustomer(ContractSearchRequest req)
        {
            var rtn = _payService.SearchCustomer(req);
            if (rtn != null)
            {
                return Ok(rtn);
            }
            else return BadRequest(ConstMessage.Message_SomethingWentWrong);
        }
        [HttpPost]
        [Route("AddContract")]
        public async Task<ActionResult> AddContract(EditContractRequest req)
        {
            //var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserId = "1ถ5";
            bool isExist = _payService.isExistContract(req.CustomerId);
            if (!isExist && req.isNew)
            {
                if (await _payService.EditContract(req, Convert.ToInt32(UserId)))
                {
                    return Ok(ConstMessage.Message_Successful);
                }
                else return BadRequest(ConstMessage.Message_SomethingWentWrong);
            }
            else return BadRequest(ConstMessage.Message_UsernameIsExist);

        }
        #endregion
    }
}
