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
        public ActionResult ContractAction()
        {
            ViewBag.partialView = ConstMessage.View_MNM_Contract;
            return View(ConstMessage.View_Index);
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
        #region User
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
            ViewBag.CustomerLine = await _managementService.GetAllCustomerLine(Convert.ToInt32(UserId));
            ViewBag.PageData = res;
            ViewBag.partialView = ConstMessage.View_MNM_User;
            return View(ConstMessage.View_Index);
        }
        [HttpGet]
        [Route("GetCustomerLineAll/{hid}")]
        public async Task<ActionResult> GetCustomerLineAll(int hid)
        {
            return Ok(await _managementService.GetAllCustomerLineByHouseID(hid));
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
                        req.Password = StringCipher.EncryptString(req.Password);
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
        #endregion
        #region House
        public async Task<ActionResult> HouseActionAsync()
        {
            //var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            List<ManagementHouse> res = await _managementService.GetAllHouseList();
            ViewBag.PageData = res;
            ViewBag.partialView = ConstMessage.View_MNM_House;
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
        #endregion
        #region CustomerLine
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
        #region Config
        public async Task<ActionResult> ConfigActionAsync()
        {
            //var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserId = "2";
            var UserAccess = "2";
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
            ViewBag.partialView = ConstMessage.View_MNM_Config;
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
    }
}
