using Milkent.AccessSpecifier;
using Milkent.DAL;
using Milkent.Models;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Linq.Dynamic;
using System.Linq;
using System.IO;

namespace Milkent.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        [CheckRole(AllowedRoles = "admin")]
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }


        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(MdlLogin mdl)
        {
            try
            {
                if (mdl.Email != null && mdl.Password != null)
                {
                    DALAccount obj = new DALAccount();
                    MdlLogin Rtn = obj.DAL_IsEmailExist(mdl.Email);
                    if (Rtn.Email == mdl.Email && Rtn.Password == mdl.Password)
                    {
                        FormsAuthentication.SetAuthCookie(mdl.Email, true);
                        Session["UserID"] = Rtn.ID;
                        Session["Image"] = Rtn.Image;
                        Session["Role"] = Rtn.Role;
                        return RedirectToAction("Index", "Home");
                       
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "The User Name or Password is Invalid");
                        return View(mdl);
                    }
                }
                else
                    return View(mdl);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Login", "Account");
        }

        [CheckRole(AllowedRoles = "admin")]
        public ActionResult CreateAccount()
        {
            return View();
        }
        [HttpPost]
        [CheckRole(AllowedRoles = "admin")]
        public ActionResult CreateAccount(MdlLogin mdl, HttpPostedFileBase file)
        {
           
            if (ModelState.IsValid&&file!=null)
            {
                var allowedextention = new[] { ".jpg", ".Jpg", ".jpeg", ".png" };
                var ext = Path.GetExtension(file.FileName);
                if (allowedextention.Contains(ext))
                {
                    var filename = Path.GetFileName(file.FileName);
                    var path = "~/Images/User/";
                    var newFileName = Guid.NewGuid() + ext;
                    string filePath = Path.Combine(path, newFileName);
                    if (!Directory.Exists(Server.MapPath(path)))
                    {
                        Directory.CreateDirectory(Server.MapPath(path));
                    }
                    file.SaveAs(Server.MapPath(filePath));

                    mdl.Image = filePath.Replace("~/", "/../");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Please Upload correct format of Picture. Picture Must be in " + allowedextention + " Format");
                    return View(mdl);
                }
                DALAccount obj = new DALAccount();
                mdl.Role = "user";
                obj.Add(mdl);
                return RedirectToAction("Index");
            }
            else
            {
                return View(mdl);
            }
        }

        public ActionResult LoadAccountData()
        {
            try
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
                searchValue = searchValue.ToLower();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                DALAccount obj = new DALAccount();
                var AccountData = obj.DalGetAllAccount();
                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {

                    AccountData = AccountData.OrderBy(sortColumn + " " + sortColumnDir).Skip(skip).Take(pageSize).ToList();
                }
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    AccountData = AccountData.Where(m => m.UserName.ToLower().Contains(searchValue) || m.Email.ToLower().Contains(searchValue)).ToList();
                }

                //total number of rows count     
                recordsTotal = AccountData.Count();
                //Paging     
                var data = AccountData.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception)
            {

                throw;
            }

        }

        [CheckRole(AllowedRoles = "admin")]
        public ActionResult EditAccount(int ID)
        {
            MdlLogin mdl = new MdlLogin();
            DALAccount obj = new DALAccount();
            mdl = obj.DAL_Read_Account(ID);
            return View(mdl);
        }
        [HttpPost]
        [CheckRole(AllowedRoles = "admin")]
        public ActionResult EditAccount(MdlLogin mdl, HttpPostedFileBase file)
        {
            if (ModelState.IsValid && file != null)
            {
                var allowedextention = new[] { ".jpg", ".Jpg", ".jpeg", ".png" };
                var ext = Path.GetExtension(file.FileName);
                if (allowedextention.Contains(ext))
                {
                    var filename = Path.GetFileName(file.FileName);
                    var path = "~/Images/User/";
                    var newFileName = Guid.NewGuid() + ext;
                    string filePath = Path.Combine(path, newFileName);
                    if (!Directory.Exists(Server.MapPath(path)))
                    {
                        Directory.CreateDirectory(Server.MapPath(path));
                    }
                    file.SaveAs(Server.MapPath(filePath));
                    mdl.Image = filePath.Replace("~/", "/../");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Please Upload correct format of Picture. Picture Must be in " + allowedextention + " Format");
                    return View(mdl);
                }
                DALAccount obj = new DALAccount();
                obj.DAL_Update_Account(mdl);
                return RedirectToAction("Index");
            }
            else
            {
                return View(mdl);
            }

        }

        [CheckRole(AllowedRoles = "admin")]
        public ActionResult DeleteAccount(int ID)
        {
            DALAccount obj = new DALAccount();
            MdlLogin mdl = obj.DAL_Read_Account(ID);
            mdl.Image = mdl.Image.Replace("/../", "~/");
            string path = Server.MapPath(mdl.Image);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            if (obj.DAL_Delete_Account(ID) == 1)
                return Json(true);
            else
                return Json(false);
        }
    }
}