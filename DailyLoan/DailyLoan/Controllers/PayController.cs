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
        public ActionResult Collector()
        {
            //var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            ViewBag.partialView = ConstMessage.View_PAY_Collector;
            return View(ConstMessage.View_Index);
        }
        public ActionResult Warn()
        {
            //var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            ViewBag.partialView = ConstMessage.View_PAY_Warn;
            return View(ConstMessage.View_Index);
        }
        #region system_setting
        public async Task<ActionResult> setting_systemAsync()
        {
            //var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserId = "1";
            var UserAccess = "1";
            ViewBag.UserId = UserId;
            ViewBag.UserAccess = UserAccess;
            if (Convert.ToInt32(UserAccess) == StatusUserAccess.UserAccess_Superadmin)
            {
                ViewBag.House = await _managementService.GetAllHouseList();
            }
            else
            {
                int hid = _managementService.GetHouseIdByUserId(Convert.ToInt32(UserId));
                ViewBag.House = hid;
                ViewBag.PageData = await _managementService.GetConfig(hid);
            }
            ViewBag.partialView = ConstMessage.View_PAY_setting_system;
            return View(ConstMessage.View_Index);
        }
        [HttpGet]
        [Route("GetConfigDetail/{hid}")]
        public async Task<ActionResult> GetConfigDetail(int hid)
        {
            return Ok(await _managementService.GetConfig(hid));
        }
        [HttpGet]
        [Route("GetSpecialRateDetail/{spid}")]
        public ActionResult GetSpecialRateDetail(int spid)
        {
            return Ok(_managementService.GetSpecialRate(spid));
        }
        [HttpGet]
        [Route("DeleteSpecialRate/{spid}")]
        public async Task<ActionResult> DeleteSpecialRate(int spid)
        {
            var UserAccess = "1";
            if (Convert.ToInt32(UserAccess) <= StatusUserAccess.UserAccess_Admin)
            {
                if (await _managementService.DeleteSpecialRate(spid))
                {
                    return Ok(ConstMessage.Message_Successful);
                }
                else return BadRequest(ConstMessage.Message_SomethingWentWrong);
            }
            else return BadRequest(ConstMessage.Message_DonotHavePermission);
        }
        [HttpPost]
        [Route("EditConfig")]
        public async Task<ActionResult> EditConfig(EditConfigRequest req)
        {
            //var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserId = "1";
            if (await _managementService.EditConfig(req, Convert.ToInt32(UserId)))
            {
                return Ok(ConstMessage.Message_Successful);
            }
            else return BadRequest(ConstMessage.Message_SomethingWentWrong);
        }
        [HttpPost]
        [Route("EditSpecialRate")]
        public async Task<ActionResult> EditSpecialRate(EditSpecialRateRequest req)
        {
            //var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserId = "1";
            if (await _managementService.EditSpecialRate(req, Convert.ToInt32(UserId)))
            {
                return Ok(ConstMessage.Message_Successful);
            }
            else return BadRequest(ConstMessage.Message_SomethingWentWrong);
        }
        #endregion
        #region setting_home
        public ActionResult setting_daily()
        {
            //var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            ViewBag.partialView = ConstMessage.View_PAY_setting_daily;
            return View(ConstMessage.View_Index);
        }
        public async Task<ActionResult> setting_homeAsync()
        {
            //var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserId = "1";
            var UserAccess = "1";
            List<ManagementHouse> res = await _managementService.GetAllHouseList();
            List<ManagementCustomerLine> res1 = await _managementService.GetAllCustomerLineList(Convert.ToInt32(UserId), Convert.ToInt32(UserAccess));
            ViewBag.UserId = UserId;
            ViewBag.UserAccess = UserAccess;
            ViewBag.House = res;
            ViewBag.PageData = res1;
            ViewBag.partialView = ConstMessage.View_PAY_setting_home;
            return View(ConstMessage.View_Index);
        }
        [HttpGet]
        [Route("GetHouseDetail/{hid}")]
        public ActionResult GetHouseDetail(int hid)
        {
            return Ok(_managementService.GetHouse(hid));
        }
        [HttpGet]
        [Route("DeleteHouse/{hid}")]
        public async Task<ActionResult> DeleteHouse(int hid)
        {
            var UserAccess = "1";
            if (Convert.ToInt32(UserAccess) == StatusUserAccess.UserAccess_Superadmin)
            {
                if (await _managementService.DeleteHouse(hid))
                {
                    return Ok(ConstMessage.Message_Successful);
                }
                else return BadRequest(ConstMessage.Message_SomethingWentWrong);
            }
            else return BadRequest(ConstMessage.Message_DonotHavePermission);
        }
        [HttpPost]
        [Route("EditHouse")]
        public async Task<ActionResult> EditHouse(EditHouseRequest req)
        {
            //var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserId = "1";
            if (await _managementService.EditHouse(req, Convert.ToInt32(UserId)))
            {
                return Ok(ConstMessage.Message_Successful);
            }
            else return BadRequest(ConstMessage.Message_SomethingWentWrong);
        }
        public async Task<ActionResult> CustomerLineActionAsync()
        {
            //var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserId = "1";
            var UserAccess = "1";
            List<ManagementCustomerLine> res = await _managementService.GetAllCustomerLineList(Convert.ToInt32(UserId), Convert.ToInt32(UserAccess));
            ViewBag.UserId = UserId;
            ViewBag.UserAccess = UserAccess;
            if (Convert.ToInt32(UserAccess) == StatusUserAccess.UserAccess_Superadmin)
                ViewBag.House = await _managementService.GetAllHouse();
            ViewBag.PageData = res;
            ViewBag.partialView = ConstMessage.View_MNM_CustomerLine;
            return View(ConstMessage.View_Index);
        }
        [HttpGet]
        [Route("GetCustomerLineDetail/{clid}")]
        public ActionResult GetCustomerLineDetail(int clid)
        {
            return Ok(_managementService.GetCustomerLine(clid));
        }
        [HttpGet]
        [Route("DeleteCustomerLine/{clid}")]
        public async Task<ActionResult> DeleteCustomerLine(int clid)
        {
            var UserAccess = "1";
            if (Convert.ToInt32(UserAccess) <= StatusUserAccess.UserAccess_Admin)
            {
                if (await _managementService.DeleteCustomerLine(clid))
                {
                    return Ok(ConstMessage.Message_Successful);
                }
                else return BadRequest(ConstMessage.Message_SomethingWentWrong);
            }
            else return BadRequest(ConstMessage.Message_DonotHavePermission);
        }
        [HttpPost]
        [Route("EditCustomerLine")]
        public async Task<ActionResult> EditCustomerLine(EditCustomerLineRequest req)
        {
            //var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserId = "1";
            if (await _managementService.EditCustomerLine(req, Convert.ToInt32(UserId)))
            {
                return Ok(ConstMessage.Message_Successful);
            }
            else return BadRequest(ConstMessage.Message_SomethingWentWrong);
        }
        #endregion
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
            ViewBag.partialView = ConstMessage.View_PAY_Customer;
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
        public async Task<ActionResult> SearchCustomer(ContractSearchRequest req)
        {
            var rtn = await _payService.SearchCustomer(req);
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

        #region DailyReport
        public async Task<ActionResult> DailyReportAsync()
        {
            //var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserId = "1";
            var UserAccess = "1";
            return View();
        }
        #endregion
        #region History
        public async Task<ActionResult> HistoryAsync()
        {
            //var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserId = "1";
            var UserAccess = "1";
            return View();
        }
        #endregion
    }
}
