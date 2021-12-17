using DailyLoan.Model;
using DailyLoan.Model.Entities.DailyLoan;
using DailyLoan.Model.Request.Management;
using DailyLoan.Model.Resoinse.Pay;
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
using static System.Net.Mime.MediaTypeNames;

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

        [HttpGet]
        [Route("GetAlert")]
        public async Task<ActionResult> GetAlert()
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else
            {
                var rtn = await _payService.GetAlert(Convert.ToInt32(UserId), Convert.ToInt32(UserAccess));
                rtn.Add(Convert.ToInt32(UserAccess)==StatusUserAccess.UserAccess_Agent?0:1);
                return Ok(rtn);
            }
        }
        #region system_setting
        public async Task<ActionResult> setting_systemAsync()
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else
            {
                if (Convert.ToInt32(UserAccess) == StatusUserAccess.UserAccess_Superadmin)
                {
                    ViewBag.UserId = UserId;
                    ViewBag.UserAccess = UserAccess;
                    /** /
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
                    /**/
                    ViewBag.PageData = await _managementService.GetConfig(1);
                    ViewBag.partialView = ConstMessage.View_PAY_setting_system;
                    return View(ConstMessage.View_Index);
                }
                else return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            }
        }
        [HttpGet]
        [Route("GetConfigDetail/{hid}")]
        public async Task<ActionResult> GetConfigDetail(int hid)
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else { return Ok(await _managementService.GetConfig(hid)); }
        }
        [HttpGet]
        [Route("GetSpecialRateDetail/{spid}")]
        public ActionResult GetSpecialRateDetail(int spid)
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else { return Ok(_managementService.GetSpecialRate(spid)); }
        }
        [HttpGet]
        [Route("DeleteSpecialRate/{spid}")]
        public async Task<ActionResult> DeleteSpecialRate(int spid)
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else { 
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
        }
        [HttpPost]
        [Route("EditConfig")]
        public async Task<ActionResult> EditConfig(EditConfigRequest req)
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else { 
                if (await _managementService.EditConfig(req, Convert.ToInt32(UserId)))
                {
                    return Ok(ConstMessage.Message_Successful);
                }
                else return BadRequest(ConstMessage.Message_SomethingWentWrong);
            }
        }
        [HttpPost]
        [Route("EditSpecialRate")]
        public async Task<ActionResult> EditSpecialRate(EditSpecialRateRequest req)
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else { 
                if (await _managementService.EditSpecialRate(req, Convert.ToInt32(UserId)))
                {
                    return Ok(ConstMessage.Message_Successful);
                }
                else return BadRequest(ConstMessage.Message_SomethingWentWrong);
            }
        }
        #endregion
        #region system_daily
        public async Task<ActionResult> setting_daily()
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else {
                if (Convert.ToInt32(UserAccess) <= StatusUserAccess.UserAccess_Admin)
                {
                    ViewBag.UserId = UserId;
                    ViewBag.UserAccess = UserAccess;
                    if (Convert.ToInt32(UserAccess) == StatusUserAccess.UserAccess_Superadmin)
                        ViewBag.House = await _managementService.GetAllHouse();
                    else
                        ViewBag.CustomerLine = await _managementService.GetAllCustomerLine(Convert.ToInt32(UserId));
                    //ViewBag.PageData = 35000;
                    ViewBag.partialView = ConstMessage.View_PAY_setting_daily;
                    return View(ConstMessage.View_Index);
                }
                else return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            }
        }

        [HttpPost]
        [Route("GetMustReturn")]
        public async Task<ActionResult> GetMustReturn(DailyCostSearchRequest req)
        {
            var rtn = await _payService.GetMustReturn(req.clid, req.date);
            return Ok(rtn);
        }
        [HttpPost]
        [Route("GetPayToCustomer")]
        public async Task<ActionResult> GetPayToCustomer(DailyCostSearchRequest req)
        {
            var rtn = await _payService.GetPayToCustomer(req.clid, req.date);
            return Ok(rtn);
        }
        [HttpPost]
        [Route("GetDailyCost")]
        public ActionResult GetDailyCost(DailyCostSearchRequest req)
        {
            var rtn = _payService.GetDailyCost(req.clid,req.date);
            return Ok(rtn);
        }
        [HttpGet]
        [Route("GetAllUserByCustomerLine/{cid}")]
        public async Task<List<User>> GetAllUserByCustomerLine(int cid)
        { 
            List<User> rtn = await _payService.GetAllUserByCustomerLine(cid);
            if (rtn == null) rtn = new List<User>();
            return rtn;
        }
        
        [HttpPost]
        [Route("SaveDailyCost")]
        public async Task<ActionResult> SaveDailyCost(DailyCost req)
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else { 
                if (await _payService.SaveDailyCost(req,Convert.ToInt32(UserId)))
                {
                    return Ok(ConstMessage.Message_Successful);
                }
                else return BadRequest(ConstMessage.Message_SomethingWentWrong);
            }
        }
        #endregion
        #region setting_home
        public async Task<ActionResult> setting_homeAsync()
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else
            {
                if (Convert.ToInt32(UserAccess) <= StatusUserAccess.UserAccess_Admin)
                {
                    List<ManagementHouse> res = await _managementService.GetAllHouseList();
                    List<ManagementCustomerLine> res1 = await _managementService.GetAllCustomerLineList(Convert.ToInt32(UserId), Convert.ToInt32(UserAccess));
                    ViewBag.UserId = UserId;
                    ViewBag.UserAccess = UserAccess;
                    ViewBag.House = res;
                    ViewBag.PageData = res1;
                    ViewBag.partialView = ConstMessage.View_PAY_setting_home;
                    return View(ConstMessage.View_Index);
                }
                return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            }
        }
        [HttpGet]
        [Route("GetHouseDetail/{hid}")]
        public ActionResult GetHouseDetail(int hid)
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else { return Ok(_managementService.GetHouse(hid)); }
        }
        [HttpGet]
        [Route("DeleteHouse/{hid}")]
        public async Task<ActionResult> DeleteHouse(int hid)
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else { 
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
        }
        [HttpPost]
        [Route("EditHouse")]
        public async Task<ActionResult> EditHouse(EditHouseRequest req)
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else { 
                if (await _managementService.EditHouse(req, Convert.ToInt32(UserId)))
                {
                    return Ok(ConstMessage.Message_Successful);
                }
                else return BadRequest(ConstMessage.Message_SomethingWentWrong);
            }
        }
        [HttpGet]
        [Route("GetCustomerLineDetail/{clid}")]
        public ActionResult GetCustomerLineDetail(int clid)
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else { return Ok(_managementService.GetCustomerLine(clid)); }
        }
        [HttpGet]
        [Route("DeleteCustomerLine/{clid}")]
        public async Task<ActionResult> DeleteCustomerLine(int clid)
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else {
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
        }
        [HttpPost]
        [Route("EditCustomerLine")]
        public async Task<ActionResult> EditCustomerLine(EditCustomerLineRequest req)
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else {
                if (await _managementService.EditCustomerLine(req, Convert.ToInt32(UserId)))
                {
                    return Ok(ConstMessage.Message_Successful);
                }
                else return BadRequest(ConstMessage.Message_SomethingWentWrong); 
            }
        }
        #endregion
        #region Customer
        public async Task<ActionResult> CustomerAsync(ContractSearchRequest req)
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else {
                List<ManagementCustomer> res = await _payService.GetAllCustomer(Convert.ToInt32(UserId), Convert.ToInt32(UserAccess));
                //List<ManagementCustomer> res = await _payService.SearchCustomer(Convert.ToInt32(UserId), req);
                ViewBag.UserId = UserId;
                ViewBag.UserAccess = UserAccess;
                if (Convert.ToInt32(UserAccess) == StatusUserAccess.UserAccess_Superadmin)
                    ViewBag.House = await _managementService.GetAllHouse();
                ViewBag.CustomerLine = await _managementService.GetAllCustomerLine(Convert.ToInt32(UserId));
                ViewBag.PageData = res;
                ViewBag.partialView = ConstMessage.View_PAY_Customer;
                return View(ConstMessage.View_Index); 
            }
        }
        [HttpGet]
        [Route("GetCustomerDetail/{cid}")]
        public ActionResult GetCustomerDetail(int cid)
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else {
                var rtn = _payService.GetCustomer(cid);
                string userpath = Path.Combine(Environment.WebRootPath, "upload", rtn.Idcard);
                if (Directory.Exists(userpath))
                    rtn.Images = Directory.GetFiles(userpath).Where(file => Path.GetFileName(file).Contains("idcard" + rtn.Idcard)).Select(file => rtn.Idcard+"/"+Path.GetFileName(file)).ToArray();
                else rtn.Images = null;
                return Ok(rtn);
            }
        }
        [HttpGet]
        [Route("DeleteCustomer/{cid}")]
        public async Task<ActionResult> DeleteCustomer(int cid)
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else { 
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
        }
        [HttpPost]
        [Route("EditCustomer")]
        public async Task<ActionResult> EditCustomer(EditCustomerRequest req)
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else {
                if (!ValidationIDCardNo.IsValidCheckPersonID(req.Idcard))
                {
                    return BadRequest(ConstMessage.Message_InValidID);
                }
                else
                {
                    var old = _payService.GetCustomer(req.Id);
                    bool isChange = req.isNew?false:(old.Idcard != req.Idcard);
                    bool isExist = _managementService.IdcardIsExist(req.Idcard);
                    if ((!isExist && req.isNew) || ((isExist||isChange) && !req.isNew))
                    {
                        if (!req.isNew)
                        {
                            if(old.Idcard != req.Idcard && Directory.Exists(Path.Combine(Environment.WebRootPath, "upload", old.Idcard)))
                            {
                                var oldpath = Path.Combine(Environment.WebRootPath, "upload", old.Idcard);
                                var newpath = Path.Combine(Environment.WebRootPath, "upload", req.Idcard);
                                string[] allfiles = Directory.GetFiles(oldpath).Select(file => Path.GetFileName(file)).ToArray();
                                Directory.CreateDirectory(newpath);
                                for (var i = 0;i < allfiles.Length; i++)
                                {
                                    var tmp = allfiles[i].Replace(old.Idcard, req.Idcard);
                                    System.IO.File.Move(oldpath+"/"+allfiles[i], newpath+"/"+tmp);
                                }
                                Directory.Delete(oldpath);
                            }
                        }
                        if (await _payService.EditCustomer(req, Convert.ToInt32(UserId)))
                        {
                            string uploadpath = Path.Combine(Environment.WebRootPath, "upload", "tmp");
                            string userpath = Path.Combine(Environment.WebRootPath, "upload", req.Idcard);
                            if (!Directory.Exists(userpath)) Directory.CreateDirectory(userpath);
                            if (req.uploaded0 != null) System.IO.File.Move(Path.Combine(uploadpath, req.uploaded0), Path.Combine(userpath, "idcard" + req.Idcard + "-0.jpg"), true);
                            if (req.uploaded1 != null) System.IO.File.Move(Path.Combine(uploadpath, req.uploaded1), Path.Combine(userpath, "idcard" + req.Idcard + "-1.jpg"), true);
                            if (req.uploaded2 != null) System.IO.File.Move(Path.Combine(uploadpath, req.uploaded2), Path.Combine(userpath, "idcard" + req.Idcard + "-2.jpg"), true);
                            if (req.uploaded3 != null) System.IO.File.Move(Path.Combine(uploadpath, req.uploaded3), Path.Combine(userpath, "idcard" + req.Idcard + "-3.jpg"), true);
                            if (req.uploaded4 != null) System.IO.File.Move(Path.Combine(uploadpath, req.uploaded4), Path.Combine(userpath, "idcard" + req.Idcard + "-4.jpg"), true);
                            if (req.uploaded5 != null) System.IO.File.Move(Path.Combine(uploadpath, req.uploaded5), Path.Combine(userpath, "idcard" + req.Idcard + "-5.jpg"), true);
                            if (req.uploaded6 != null) System.IO.File.Move(Path.Combine(uploadpath, req.uploaded6), Path.Combine(userpath, "idcard" + req.Idcard + "-6.jpg"), true);
                            if (req.uploaded7 != null) System.IO.File.Move(Path.Combine(uploadpath, req.uploaded7), Path.Combine(userpath, "idcard" + req.Idcard + "-7.jpg"), true);
                            if (req.uploaded8 != null) System.IO.File.Move(Path.Combine(uploadpath, req.uploaded8), Path.Combine(userpath, "idcard" + req.Idcard + "-8.jpg"), true);
                            if (req.uploaded9 != null) System.IO.File.Move(Path.Combine(uploadpath, req.uploaded9), Path.Combine(userpath, "idcard" + req.Idcard + "-9.jpg"), true);
                            return Ok(ConstMessage.Message_Successful);
                        }
                        else return BadRequest(ConstMessage.Message_SomethingWentWrong);
                    }
                    else return BadRequest(ConstMessage.Message_IdcardIsExist);
                } 
            }
        }
        [HttpPost]
        [Route("MoveCustomer")]
        public async Task<ActionResult> MoveCustomer(EditCustomerRequest req)
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else
            {
                if (await _payService.MoveCustomer(req, Convert.ToInt32(UserId))) return Ok(ConstMessage.Message_Successful);
                else return BadRequest(ConstMessage.Message_SomethingWentWrong);
            }
        }
        [HttpPost]
        [Route("SearchCustomerPage")]
        public async Task<ActionResult> SearchCustomerPage(ContractSearchRequest req)
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else {
                List<ManagementCustomer> res = await _payService.SearchCustomer(Convert.ToInt32(UserId), req);
                ViewBag.UserId = UserId;
                ViewBag.UserAccess = UserAccess;
                if (Convert.ToInt32(UserAccess) == StatusUserAccess.UserAccess_Superadmin)
                    ViewBag.House = await _managementService.GetAllHouse();
                ViewBag.CustomerLine = await _managementService.GetAllCustomerLine(Convert.ToInt32(UserId));
                ViewBag.PageData = res;
                ViewBag.partialView = ConstMessage.View_PAY_Customer;
                return View(ConstMessage.View_Index); 
            }
        }
        [HttpPost]
        [Route("UploadFileCustomer")]
        public async Task<IActionResult> UploadFileCustomer(UploadPic req)
        {
            try
            {
                if (req.uploadfile1 == null&&req.uploadfile2 == null)
                    return BadRequest(ConstMessage.Message_UploadFail);

                string uploadpath = Path.Combine(Environment.WebRootPath, "upload", "tmp");                 
                if (!Directory.Exists(uploadpath))
                {
                    Directory.CreateDirectory(uploadpath);
                }
                string filename = DateTime.Now.ToString("'idcard-'yyyyMMddHHmmssfff");
                List<string> filenames = new List<string>();
                int i = 0;
                if(req.uploadfile1 != null)
                {
                    i = 0;
                    foreach (var f in req.uploadfile1)
                    {
                        string fileLocation = Path.Combine(uploadpath, filename+"-"+i.ToString());
                        filenames.Add(filename + "-" + i.ToString());
                        using (Stream fileStream = new FileStream(fileLocation, FileMode.Create))
                        {
                            await f.CopyToAsync(fileStream);
                        }
                        i++;
                    }

                }
                if (req.uploadfile2 != null)
                {
                    i = 0;
                    foreach (var f in req.uploadfile2)
                    {
                        string fileLocation = Path.Combine(uploadpath, filename + "-" + i.ToString());
                        filenames.Add(filename + "-" + i.ToString());
                        using (Stream fileStream = new FileStream(fileLocation, FileMode.Create))
                        {
                            await f.CopyToAsync(fileStream);
                        }
                        i++;
                    }

                }
                UploadFileResponse rtn = new UploadFileResponse()
                {
                    path = "/upload/tmp",
                    filename = filenames.ToArray(),
                    message = ConstMessage.Message_Successful
                };
                return Ok(rtn);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                UploadFileResponse rtn = new UploadFileResponse()
                {
                    error = ConstMessage.Message_UploadFail,
                    message = ex.Message
                };
                return BadRequest(rtn);
            }
        }
        #endregion
        #region Contract
        public async Task<ActionResult> ContractAsync()
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else {
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
        }
        [HttpGet]
        [Route("GetContractDetail/{cid}")]
        public ActionResult GetContractDetail(int cid)
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else {
                var rtn = _payService.GetContract(cid);
                string idcard = _payService.GetIDcardByCustomerId(rtn.CustomerId);
                string userpath = Path.Combine(Environment.WebRootPath, "upload", idcard);
                if (Directory.Exists(userpath))
                {
                    rtn.Images = Directory.GetFiles(userpath).Where(file => Path.GetFileName(file).Contains("contract" + idcard + rtn.Id.ToString())).Select(file => idcard + "/" + Path.GetFileName(file)).ToArray();
                    rtn.Customer.Images = Directory.GetFiles(userpath).Where(file => Path.GetFileName(file).Contains("idcard" + idcard)).Select(file => idcard + "/" + Path.GetFileName(file)).ToArray();
                }
                else 
                {
                    rtn.Images = null;
                    rtn.Customer.Images = null;
                }
                string gidcard = _payService.GetIDcardByCustomerId(rtn.GuarantorId);
                string guapath = Path.Combine(Environment.WebRootPath, "upload", gidcard);
                if (Directory.Exists(guapath))
                {
                    rtn.Guaruntor.Images = Directory.GetFiles(guapath).Where(file => Path.GetFileName(file).Contains("idcard" + gidcard)).Select(file => gidcard + "/" + Path.GetFileName(file)).ToArray();
                }
                else
                {
                    rtn.Guaruntor.Images = null;
                }
                return Ok(rtn); 
            }
        }
        [HttpGet]
        [Route("DeleteContract/{cid}")]
        public async Task<ActionResult> DeleteContract(int cid)
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else {
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
        }
        [HttpPost]
        [Route("EditContract")]
        public async Task<ActionResult> EditContract(EditContractRequest req)
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else {
                if (await _payService.EditContract(req, Convert.ToInt32(UserId)))
                {
                    string uploadpath = Path.Combine(Environment.WebRootPath, "upload", "tmp");
                    string idcard = _payService.GetIDcardByCustomerId(req.CustomerId);
                    string userpath = Path.Combine(Environment.WebRootPath, "upload", idcard);
                    if (!Directory.Exists(userpath)) Directory.CreateDirectory(userpath);
                    if (req.uploaded0 != null) System.IO.File.Move(Path.Combine(uploadpath, req.uploaded0), Path.Combine(userpath, "contract" + idcard + req.Id.ToString() + "-0.jpg"), true);
                    if (req.uploaded1 != null) System.IO.File.Move(Path.Combine(uploadpath, req.uploaded1), Path.Combine(userpath, "contract" + idcard + req.Id.ToString() + "-1.jpg"), true);
                    if (req.uploaded2 != null) System.IO.File.Move(Path.Combine(uploadpath, req.uploaded2), Path.Combine(userpath, "contract" + idcard + req.Id.ToString() + "-2.jpg"), true);
                    if (req.uploaded3 != null) System.IO.File.Move(Path.Combine(uploadpath, req.uploaded3), Path.Combine(userpath, "contract" + idcard + req.Id.ToString() + "-3.jpg"), true);
                    if (req.uploaded4 != null) System.IO.File.Move(Path.Combine(uploadpath, req.uploaded4), Path.Combine(userpath, "contract" + idcard + req.Id.ToString() + "-4.jpg"), true);
                    if (req.uploaded5 != null) System.IO.File.Move(Path.Combine(uploadpath, req.uploaded5), Path.Combine(userpath, "contract" + idcard + req.Id.ToString() + "-5.jpg"), true);
                    if (req.uploaded6 != null) System.IO.File.Move(Path.Combine(uploadpath, req.uploaded6), Path.Combine(userpath, "contract" + idcard + req.Id.ToString() + "-6.jpg"), true);
                    if (req.uploaded7 != null) System.IO.File.Move(Path.Combine(uploadpath, req.uploaded7), Path.Combine(userpath, "contract" + idcard + req.Id.ToString() + "-7.jpg"), true);
                    if (req.uploaded8 != null) System.IO.File.Move(Path.Combine(uploadpath, req.uploaded8), Path.Combine(userpath, "contract" + idcard + req.Id.ToString() + "-8.jpg"), true);
                    if (req.uploaded9 != null) System.IO.File.Move(Path.Combine(uploadpath, req.uploaded9), Path.Combine(userpath, "contract" + idcard + req.Id.ToString() + "-9.jpg"), true);
                    return Ok(ConstMessage.Message_Successful);
                }
                else return BadRequest(ConstMessage.Message_SomethingWentWrong); 
            }
        }
        [HttpPost]
        [Route("SearchContract")]
        public async Task<ActionResult> SearchContract(ContractSearchRequest req)
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else {
                List<ManagementContract> res = await _payService.SearchContract(Convert.ToInt32(UserId), req);
                ViewBag.UserId = UserId;
                ViewBag.UserAccess = UserAccess;
                if (Convert.ToInt32(UserAccess) == StatusUserAccess.UserAccess_Superadmin)
                    ViewBag.House = await _managementService.GetAllHouse();
                ViewBag.CustomerLine = await _managementService.GetAllCustomerLine(Convert.ToInt32(UserId));
                ViewBag.PageData = res;
                ViewBag.partialView = ConstMessage.View_MNM_Contract;
                return View(ConstMessage.View_Index); 
            }
        }
        [HttpPost]
        [Route("SearchCustomer")]
        public async Task<ActionResult> SearchCustomer(ContractSearchRequest req)
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else {
                var rtn = await _payService.SearchCustomer(Convert.ToInt32(UserId),req);
                if (rtn != null)
                {
                    for(int i = 0; i < rtn.Count; i++)
                    {
                        string userpath = Path.Combine(Environment.WebRootPath, "upload", rtn[i].Idcard);
                        if (Directory.Exists(userpath))
                            rtn[i].Images = Directory.GetFiles(userpath).Where(file => Path.GetFileName(file).Contains("idcard" + rtn[i].Idcard)).Select(file => rtn[i].Idcard + "/" + Path.GetFileName(file)).ToArray();
                        else rtn[i].Images = null;
                    }
                    return Ok(rtn);
                }
                else return BadRequest(ConstMessage.Message_SomethingWentWrong); 
            }
        }
        [HttpPost]
        [Route("AddContract")]
        public async Task<ActionResult> AddContract(EditContractRequest req)
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else {
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
        }
        [HttpPost]
        [Route("UploadFileContract")]
        public async Task<IActionResult> UploadFileContract(UploadPic req)
        {
            try
            {
                if (req.uploadfile1 == null && req.uploadfile2 == null)
                {
                    UploadFileResponse err = new UploadFileResponse()
                    {
                        error = ConstMessage.Message_UploadFail,
                        message = ""
                    };
                    return BadRequest(err);
                }

                string uploadpath = Path.Combine(Environment.WebRootPath, "upload", "tmp");
                if (!Directory.Exists(uploadpath))
                {
                    Directory.CreateDirectory(uploadpath);
                }
                string filename = DateTime.Now.ToString("'evidence-'yyyyMMddHHmmssfff");
                List<string> filenames = new List<string>();
                int i = 0;
                if (req.uploadfile1 != null)
                {
                    i = 0;
                    foreach (var f in req.uploadfile1)
                    {
                        string fileLocation = Path.Combine(uploadpath, filename + "-" + i.ToString());
                        filenames.Add(filename + "-" + i.ToString());
                        using (Stream fileStream = new FileStream(fileLocation, FileMode.Create))
                        {
                            await f.CopyToAsync(fileStream);
                        }
                    }
                    i++;

                }
                if (req.uploadfile2 != null)
                {
                    i = 0;
                    foreach (var f in req.uploadfile2)
                    {
                        string fileLocation = Path.Combine(uploadpath, filename + "-" + i.ToString());
                        filenames.Add(filename + "-" + i.ToString());
                        using (Stream fileStream = new FileStream(fileLocation, FileMode.Create))
                        {
                            await f.CopyToAsync(fileStream);
                        }
                    }
                    i++;

                }
                UploadFileResponse rtn = new UploadFileResponse()
                {
                    path = "/upload/tmp",
                    filename = filenames.ToArray(),
                    message = ConstMessage.Message_Successful
                };
                return Ok(rtn);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                UploadFileResponse rtn = new UploadFileResponse()
                {
                    error = ConstMessage.Message_UploadFail,
                    message = ex.Message
                };
                return BadRequest(rtn);
            }
        }
        [HttpPost]
        [Route("UploadSign")]
        public async Task<IActionResult> UploadSign(UploadPic req)
        {
            try
            {
                if (req.uploadsign == null)
                {
                    UploadFileResponse err = new UploadFileResponse()
                    {
                        error = ConstMessage.Message_UploadFail,
                        message = ""
                    };
                    return BadRequest(err);
                }

                string uploadpath = Path.Combine(Environment.WebRootPath, "upload", "tmp");
                if (!Directory.Exists(uploadpath))
                {
                    Directory.CreateDirectory(uploadpath);
                }
                string filename = DateTime.Now.ToString("'evidence-'yyyyMMddHHmmssfff");
                if (req.uploadsign != null)
                {
                    string fileLocation = Path.Combine(uploadpath, filename);
                    string[] arr = req.uploadsign.Split("base64,");
                    if(arr.Length != 2)
                    {
                        UploadFileResponse rtn1 = new UploadFileResponse()
                        {
                            error = ConstMessage.Message_UploadFail,
                            message = "ข้อมูลผิดพลาด"
                        };
                        return BadRequest(rtn1);
                    }
                    req.uploadsign = arr[1];
                    byte[] bytes = Convert.FromBase64String(req.uploadsign);
                    using (Stream fileStream = new FileStream(fileLocation, FileMode.Create))
                    {
                        await fileStream.WriteAsync(bytes);
                    }

                }
                var tmp = new List<string>();
                tmp.Add(filename);
                UploadFileResponse rtn = new UploadFileResponse()
                {
                    path = "/upload/tmp",
                    filename = tmp.ToArray(),
                    message = ConstMessage.Message_Successful
                };
                return Ok(rtn);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                UploadFileResponse rtn = new UploadFileResponse()
                {
                    error = ConstMessage.Message_UploadFail,
                    message = ex.Message
                };
                return BadRequest(rtn);
            }
        }
        [HttpPost]
        [Route("UploadContract")]
        public ActionResult UploadContract(EditContractRequest req)
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else
            {
                string uploadpath = Path.Combine(Environment.WebRootPath, "upload", "tmp");
                string idcard = _payService.GetIDcardByCustomerId(req.CustomerId);
                string userpath = Path.Combine(Environment.WebRootPath, "upload", idcard);
                if (!Directory.Exists(userpath)) Directory.CreateDirectory(userpath);
                if (req.uploaded0 != null) System.IO.File.Move(Path.Combine(uploadpath, req.uploaded0), Path.Combine(userpath, "contract" + idcard + req.Id.ToString() + "-0.jpg"), true);
                if (req.uploaded1 != null) System.IO.File.Move(Path.Combine(uploadpath, req.uploaded1), Path.Combine(userpath, "contract" + idcard + req.Id.ToString() + "-1.jpg"), true);
                if (req.uploaded2 != null) System.IO.File.Move(Path.Combine(uploadpath, req.uploaded2), Path.Combine(userpath, "contract" + idcard + req.Id.ToString() + "-2.jpg"), true);
                if (req.uploaded3 != null) System.IO.File.Move(Path.Combine(uploadpath, req.uploaded3), Path.Combine(userpath, "contract" + idcard + req.Id.ToString() + "-3.jpg"), true);
                if (req.uploaded4 != null) System.IO.File.Move(Path.Combine(uploadpath, req.uploaded4), Path.Combine(userpath, "contract" + idcard + req.Id.ToString() + "-4.jpg"), true);
                if (req.uploaded5 != null) System.IO.File.Move(Path.Combine(uploadpath, req.uploaded5), Path.Combine(userpath, "contract" + idcard + req.Id.ToString() + "-5.jpg"), true);
                if (req.uploaded6 != null) System.IO.File.Move(Path.Combine(uploadpath, req.uploaded6), Path.Combine(userpath, "contract" + idcard + req.Id.ToString() + "-6.jpg"), true);
                if (req.uploaded7 != null) System.IO.File.Move(Path.Combine(uploadpath, req.uploaded7), Path.Combine(userpath, "contract" + idcard + req.Id.ToString() + "-7.jpg"), true);
                if (req.uploaded8 != null) System.IO.File.Move(Path.Combine(uploadpath, req.uploaded8), Path.Combine(userpath, "contract" + idcard + req.Id.ToString() + "-8.jpg"), true);
                if (req.uploaded9 != null) System.IO.File.Move(Path.Combine(uploadpath, req.uploaded9), Path.Combine(userpath, "contract" + idcard + req.Id.ToString() + "-9.jpg"), true);
                
                /*UploadFileResponse rtn = new UploadFileResponse()
                {
                    path = "/upload/tmp",
                    filename = null,
                    message = ConstMessage.Message_Successful
                };*/
                return Ok(ConstMessage.Message_Successful);
            }
        }
        [HttpPost]
        [Route("Approve")]
        public async Task<ActionResult> Approve(EditContractRequest req)
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else
            {
                if (await _payService.Approve(Convert.ToInt32(req.Id), Convert.ToInt32(UserId)))
                {
                    return Ok(ConstMessage.Message_Successful);
                }
                else return BadRequest(ConstMessage.Message_SomethingWentWrong);
            }
        }
        [HttpPost]
        [Route("Deny")]
        public async Task<ActionResult> Deny(EditContractRequest req)
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else
            {
                
                if (await _payService.Deny(Convert.ToInt32(req.Id),req.Remark))
                {
                    return Ok(ConstMessage.Message_Successful);
                }
                else return BadRequest(ConstMessage.Message_SomethingWentWrong);
            }
        }
        #endregion

        #region Collector
        public async Task<ActionResult> Collector()
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else
            {
                List<ManagementContract> res = await _payService.GetAllContract(Convert.ToInt32(UserId), Convert.ToInt32(UserAccess));
                ViewBag.UserId = UserId;
                ViewBag.UserAccess = UserAccess;
                //if (Convert.ToInt32(UserAccess) == StatusUserAccess.UserAccess_Superadmin)
                //    ViewBag.House = await _managementService.GetAllHouse();
                //ViewBag.CustomerLine = await _managementService.GetAllCustomerLine(Convert.ToInt32(UserId));
                ViewBag.PageData = res.Where(x => x.Status != ContractStatus.StatusContract_Closed
                                                &&x.Status != ContractStatus.StatusContract_WaitConfirm
                                                &&x.Status != ContractStatus.StatusContract_NotApprove ).ToList();
                ViewBag.partialView = ConstMessage.View_PAY_Collector;
                return View(ConstMessage.View_Index);
            }
        }
        [HttpPost]
        [Route("SearchCollector")]
        public async Task<ActionResult> SearchCollector(ContractSearchRequest req)
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else
            {
                List<ManagementContract> res = await _payService.SearchContract(Convert.ToInt32(UserId), req);
                ViewBag.UserId = UserId;
                ViewBag.UserAccess = UserAccess;
                ViewBag.PageData = res.Where(x => x.Status != ContractStatus.StatusContract_Closed
                                                && x.Status != ContractStatus.StatusContract_WaitConfirm
                                                && x.Status != ContractStatus.StatusContract_NotApprove).ToList(); ;
                ViewBag.partialView = ConstMessage.View_PAY_Collector;
                return View(ConstMessage.View_Index);
            }
        }
        [HttpGet]
        [Route("GetCollectDetail/{cid}")]
        public async Task<ActionResult> GetCollectDetail(int cid)
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else
            {
                var rtn = await _payService.GetCollector(cid);
                string idcard = _payService.GetIDcardByCustomerId(rtn.CustomerId);
                string userpath = Path.Combine(Environment.WebRootPath, "upload", idcard);
                if (Directory.Exists(userpath))
                {
                    rtn.Images = Directory.GetFiles(userpath).Where(file => Path.GetFileName(file).Contains("contract" +idcard + rtn.Id.ToString())).Select(file => idcard + "/" + Path.GetFileName(file)).ToArray();
                    rtn.Customer.Images = Directory.GetFiles(userpath).Where(file => Path.GetFileName(file).Contains("idcard" + idcard)).Select(file => idcard + "/" + Path.GetFileName(file)).ToArray();
                }
                else
                {
                    rtn.Images = null;
                    rtn.Customer.Images = null;
                }
                string gidcard = _payService.GetIDcardByCustomerId(rtn.GuarantorId);
                string guapath = Path.Combine(Environment.WebRootPath, "upload", gidcard);
                if (Directory.Exists(guapath))
                {
                    rtn.Guaruntor.Images = Directory.GetFiles(guapath).Where(file => Path.GetFileName(file).Contains("idcard" + gidcard)).Select(file => gidcard + "/" + Path.GetFileName(file)).ToArray();
                }
                else
                {
                    rtn.Guaruntor.Images = null;
                }
                return Ok(rtn);
            }
        }
        [HttpPost]
        [Route("Collect")]
        public async Task<ActionResult> Collect(Transaction req)
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else
            {
                if (await _payService.CollectCustomer(req, Convert.ToInt32(UserId)))
                {
                    return Ok(ConstMessage.Message_Successful);
                }
                else return BadRequest(ConstMessage.Message_SomethingWentWrong);
            }
        }
        #endregion
        #region Warn
        public async Task<ActionResult> WarnAsync()
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else
            {
                List<ManagementWarn> res = await _payService.GetAllWarn(Convert.ToInt32(UserId), Convert.ToInt32(UserAccess));
                ViewBag.UserId = UserId;
                ViewBag.UserAccess = UserAccess;
                ViewBag.PageData = res;
                ViewBag.partialView = ConstMessage.View_PAY_Warn;
                return View(ConstMessage.View_Index);
            }
        }
        [HttpGet]
        [Route("GetWarnDetail/{nid}")]
        public async Task<ActionResult> GetWarnDetail(int nid)
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else
            {
                var rtn = await _payService.GetWarn(nid);
                return Ok(rtn);
            }
        }
        [HttpGet]
        [Route("DeleteWarn/{nid}")]
        public async Task<ActionResult> DeleteWarn(int nid)
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else
            {
                if (Convert.ToInt32(UserAccess) <= StatusUserAccess.UserAccess_Admin)
                {
                    if (await _payService.DeleteNotification(nid))
                    {
                        return Ok(ConstMessage.Message_Successful);
                    }
                    else return BadRequest(ConstMessage.Message_SomethingWentWrong);
                }
                else return BadRequest(ConstMessage.Message_DonotHavePermission);
            }
        }
        [HttpPost]
        [Route("EditWarn")]
        public async Task<ActionResult> EditWarn(Notification req)
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else
            {
                if (await _payService.EditNotification(req, Convert.ToInt32(UserId)))
                {
                    return Ok(ConstMessage.Message_Successful);
                }
                else return BadRequest(ConstMessage.Message_SomethingWentWrong);
            }
        }
        #endregion
        #region DailyReport
        public async Task<ActionResult> DailyReportAsync()
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else 
            {
                DailyReportResponse res = await _payService.GetDailyReport(Convert.ToInt32(UserId), DateTime.Now);
                ViewBag.PageData = res;
                var clid = _payService.GetCustomerLineIdByUserId(Convert.ToInt32(UserId));
                ViewBag.House = _payService.GetHouseIdByUserId(Convert.ToInt32(UserId));
                ViewBag.CustomerLine = clid;
                ViewBag.PageDataCost  = _payService.GetDailyCost(clid, DateTime.Now);
                ViewBag.partialView = ConstMessage.View_PAY_DailyReport;
                return View(ConstMessage.View_Index);
            }
        }
        [HttpPost]
        [Route("SearchDailyReport")]
        public async Task<ActionResult> SearchDailyReport(DailyReportSearchRequest req)
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else
            {
                DailyReportResponse res = await _payService.GetDailyReport(Convert.ToInt32(UserId), req.date);
                ViewBag.PageData = res;
                var clid = _payService.GetCustomerLineIdByUserId(Convert.ToInt32(UserId));
                ViewBag.House = _payService.GetHouseIdByUserId(Convert.ToInt32(UserId));
                ViewBag.CustomerLine = clid;
                ViewBag.PageDataCost = _payService.GetDailyCost(clid, req.date);
                ViewBag.partialView = ConstMessage.View_PAY_DailyReport;
                return View(ConstMessage.View_Index);
            }
        }
        #endregion
        #region History
        public async Task<ActionResult> HistoryAsync()
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else
            {
                List<ManagementHistory> res = await _payService.GetAllHistory(Convert.ToInt32(UserId), Convert.ToInt32(UserAccess));
                ViewBag.UserId = UserId;
                ViewBag.UserAccess = UserAccess;
                ViewBag.PageData = res;
                ViewBag.partialView = ConstMessage.View_PAY_History;
                return View(ConstMessage.View_Index);
            }
        }
        [HttpGet]
        [Route("GetHistoryDetail/{cid}")]
        public async Task<ActionResult> GetHistoryDetail(int cid)
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else
            {
                var rtn = await _payService.GetHistory(cid);
                string idcard = _payService.GetIDcardByCustomerId(rtn.CustomerId);
                string userpath = Path.Combine(Environment.WebRootPath, "upload", idcard);
                if (Directory.Exists(userpath))
                {
                    rtn.Images = Directory.GetFiles(userpath).Where(file => Path.GetFileName(file).Contains("contract" + idcard + cid.ToString())).Select(file => idcard + "/" + Path.GetFileName(file)).ToArray();
                    rtn.Customer.Images = Directory.GetFiles(userpath).Where(file => Path.GetFileName(file).Contains("idcard" + idcard)).Select(file => idcard + "/" + Path.GetFileName(file)).ToArray();
                }
                else
                {
                    rtn.Images = null;
                    rtn.Customer.Images = null;
                }
                string gidcard = _payService.GetIDcardByCustomerId(rtn.GuarantorId);
                string guapath = Path.Combine(Environment.WebRootPath, "upload", gidcard);
                if (Directory.Exists(guapath))
                {
                    rtn.Guaruntor.Images = Directory.GetFiles(guapath).Where(file => Path.GetFileName(file).Contains("idcard" + gidcard)).Select(file => gidcard + "/" + Path.GetFileName(file)).ToArray();
                }
                else
                {
                    rtn.Guaruntor.Images = null;
                }
                return Ok(rtn);
            }
        }
        [HttpGet]
        [Route("GetCutDetail/{cid}")]
        public async Task<ActionResult> GetCutDetail(int cid)
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else
            {
                var rtn = await _payService.GetCutDetail(cid);
                return Ok(rtn);
            }
        }
        [HttpPost]
        [Route("Cut")]
        public async Task<ActionResult> Cut(CutRequest req)
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else
            {
                //bool isExist = _payService.isExistRequest(req.contractid,req.type);
                bool isExist = false;
                if (!isExist)
                {
                    if (await _payService.CutRequest(req.contractid,req.amount, Convert.ToInt32(UserId),req.type))
                    {
                        return Ok(ConstMessage.Message_Successful);
                    }
                    else return BadRequest(ConstMessage.Message_SomethingWentWrong);
                }
                else return BadRequest(ConstMessage.Message_SomethingWentWrong);
            }
        }
        [HttpPost]
        [Route("SearchHistory")]
        public async Task<ActionResult> SearchHistory(ContractSearchRequest req)
        {
            var UserId = HttpContext.Session.GetString(ConstMessage.Session_UserId);
            var UserAccess = HttpContext.Session.GetString(ConstMessage.Session_UserAccess);
            if (String.IsNullOrEmpty(UserId) || UserId == "0") return RedirectToAction(ConstMessage.View_Index, ConstMessage.Controller_Home);
            else
            {
                List<ManagementHistory> res = await _payService.SearchHistory(Convert.ToInt32(UserId), req);
                ViewBag.UserId = UserId;
                ViewBag.UserAccess = UserAccess;
                ViewBag.PageData = res;
                ViewBag.partialView = ConstMessage.View_PAY_History;
                return View(ConstMessage.View_Index);
            }
        }
        #endregion
    }
}
