using Milkent.AccessSpecifier;
using Milkent.DAL;
using Milkent.Models;
using System;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;

namespace Milkent.Controllers
{
    [Authorize]
    public class SupplierController : Controller
    {
        // GET: Supplier
        #region Supplier
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult AddSupplier()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult AddSupplier(MdlSupplier mdl, HttpPostedFileBase file)
        {
            try
            {
                if (ModelState.IsValid && file != null)
                {
                    var allowedextention = new[] { ".jpg", ".Jpg", ".jpeg", ".png" };
                    var ext = Path.GetExtension(file.FileName);
                    if (allowedextention.Contains(ext))
                    {
                        var filename = Path.GetFileName(file.FileName);
                        var path = "~/Images/SupplierImages/";
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
                        return RedirectToAction("Index");
                    }
                    DALSupplier obj = new DALSupplier();
                    obj.Add(mdl);
                }
                else
                {
                    return View(mdl);
                }
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }

        }
        [CheckRole(AllowedRoles = "admin")]
        public ActionResult EditSupplier(int ID)
        {
            DALSupplier obj = new DALSupplier();
            MdlSupplier mdl = obj.DAL_Read_Supplier(ID);
            return View(mdl);
        }
        [HttpPost]
        [CheckRole(AllowedRoles = "admin")]
        public ActionResult EditSupplier(MdlSupplier mdl, HttpPostedFileBase file)
        {
            try
            {
                if (ModelState.IsValid && file != null)
                {
                    var allowedextention = new[] { ".jpg", ".Jpg", ".jpeg", ".png" };
                    var ext = Path.GetExtension(file.FileName);
                    if (allowedextention.Contains(ext))
                    {
                        var filename = Path.GetFileName(file.FileName);
                        var path = "~/Images/CustomerImages/";
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
                        return RedirectToAction("Index");
                    }
                    DALSupplier obj = new DALSupplier();
                    obj.DAL_Update_Supplier(mdl);
                }
                else
                {
                    return View(mdl);
                }
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }

        }
        [CheckRole(AllowedRoles = "admin")]
        public ActionResult DeleteSupplier(int ID)
        {
            DALSupplier obj = new DALSupplier();
            MdlSupplier mdl = obj.DAL_Read_Supplier(ID);
            mdl.Image = mdl.Image.Replace("/../", "~/");
            string path = Server.MapPath(mdl.Image);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            if (obj.Dal_Delete_Supplier(ID) == 1)
                return Json(true);
            else
                return Json(false);
        }
        #endregion Supplier

        #region SupplierReceipt
        public ActionResult SupplierReceipt()
        {
            MdlPurchase mdl = new MdlPurchase();
            DALSupplier obj = new DALSupplier();
            mdl.SupplierList = obj.DalGetAllSuplier();
            return View(mdl);
        }

        public ActionResult ViewReceipt(int ID /*Bill_ID*/)
        {
            DALSupplier obj = new DALSupplier();
            MdlPurchase mdl = obj.DAL_Get_SupplierReceipt(ID/*Bill_ID*/);
            DALAccount obj1 = new DALAccount();
            TempData["User"] = obj1.DAL_Read_Account(mdl.UserID);
            DALPurchase obj2 = new DALPurchase();
            TempData["modelList"] = obj2.DAL_GetAllPurchasesOf_OneReceipt(ID/*Bill_ID*/);
            TempData["Supplier"] = obj.DAL_Read_Supplier(mdl.SupplierID/*Supplier_ID*/);
            return View(mdl);
        }
        [CheckRole(AllowedRoles = "admin")]
        public ActionResult DeletePurchaseReceipt(int ID /*Bill_ID*/)
        {
            DALSupplier obj = new DALSupplier();
            MdlPurchase mdl = obj.DAL_Get_SupplierReceipt(ID);
            if (mdl.Credit == 0)
            {
                if (obj.Dal_Delete_SupplierPurchaseReceipt(ID) >= 1)
                    return Json(true);
                else
                    return Json(false);
            }
            else
                return Json(false);
        }
        #endregion SupplierReceipt

        #region SupplierPayment
        public ActionResult SupplierPayment(int ID/*Bill ID*/)
        {
            Session["PurchaseBill_ID"] = ID;
            return View();
        }

        public ActionResult Addpayment()
        {
            int ID = 0;
            if (System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["PurchaseBill_ID"] != null)
            {
                ID = (int)Session["PurchaseBill_ID"];
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
            DALSupplier obj = new DALSupplier();
            MdlPurchase mdl = obj.DAL_Get_SupplierReceipt(ID);
            MdlPayment mdlPayment = new MdlPayment();
            mdlPayment.Credit_Debit = mdl.Credit;
            mdlPayment.Total = mdl.Total;
            mdlPayment.Bill_ID = mdl.ID;
            return View(mdlPayment);
        }
        [HttpPost]
        public ActionResult Addpayment(MdlPayment mdl)
        {
            if (ModelState.IsValid)
            {
                DALSupplier obj = new DALSupplier();
                MdlPurchase mdlReceipt = obj.DAL_Get_SupplierReceipt(mdl.Bill_ID);
                if(mdlReceipt.Total!=mdlReceipt.Credit)
                {
                    if (mdlReceipt.Total - (mdlReceipt.Credit + mdl.Credit_Debit) < 1)
                        mdl.Total = mdlReceipt.Total;
                    else
                        mdl.Total = mdlReceipt.Credit + mdl.Credit_Debit;
                    mdl.Date = DateTime.Today.Date;
                    obj.DAL_AddSupplierPayment(mdl);
                    return RedirectToAction("SupplierPayment",new { id=mdl.Bill_ID});
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Bill is Cleared now you dont add further payment");
                    return View(mdl);
                }

            }
            return View(mdl);
        }
        [CheckRole(AllowedRoles = "admin")]
        public ActionResult EditPayment(int ID)
        {
            DALSupplier obj = new DALSupplier();
            MdlPayment mdlPayment = obj.DAL_Get_SupplierPayment(ID);
            MdlPurchase mdl = obj.DAL_Get_SupplierReceipt(mdlPayment.Bill_ID);
            mdlPayment.Total = mdl.Total - mdl.Credit;
            return View(mdlPayment);
        }
        [HttpPost]
        [CheckRole(AllowedRoles = "admin")]
        public ActionResult EditPayment(MdlPayment mdlNew)
        {
            if (ModelState.IsValid)
            {
                DALSupplier obj = new DALSupplier();
                MdlPayment mdlPrevious = obj.DAL_Get_SupplierPayment(mdlNew.ID);
                MdlPurchase mdlReceipt = obj.DAL_Get_SupplierReceipt(mdlNew.Bill_ID);
                if (mdlPrevious.Credit_Debit < mdlNew.Credit_Debit)
                {
                    double TotalCredit = mdlNew.Total + mdlPrevious.Credit_Debit;
                    if ((TotalCredit - mdlNew.Credit_Debit)<1)
                        mdlNew.Total = mdlReceipt.Total;
                    else
                        mdlNew.Total = mdlReceipt.Credit + (mdlNew.Credit_Debit - mdlPrevious.Credit_Debit);
                }
                else if (mdlPrevious.Credit_Debit > mdlNew.Credit_Debit)
                {
                    mdlNew.Total = mdlReceipt.Credit - (mdlPrevious.Credit_Debit - mdlNew.Credit_Debit);
                }
                obj.DAL_EditSupplierPayment(mdlNew);
                return RedirectToAction("SupplierPayment", new { id= mdlNew.Bill_ID });
            }
            return View(mdlNew);
        }
        [CheckRole(AllowedRoles = "admin")]
        public ActionResult DeletePayment(int ID)
        {
            DALSupplier obj = new DALSupplier();
            if (obj.DAL_Delete_SupplierPayment(ID) == 1)
                return Json(true);
            else
                return Json(false);
        }
        #endregion Supplier

        #region AjaxLoad
        public ActionResult LoadPurchaseReceipt(int ID/*SupplierID*/)
        {
            try
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
                var Clear = Request["searchByfilter"].ToString();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                DALSupplier obj = new DALSupplier();
                var PurchaseData = obj.DAL_GetAll_SupplierPurchasesReceipt(ID);
                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {

                    PurchaseData = PurchaseData.OrderBy(sortColumn + " " + sortColumnDir).Skip(skip).Take(pageSize).ToList();
                }
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    PurchaseData = PurchaseData.Where(m => m.Date.Date.ToString().Contains(searchValue)).ToList();
                }
                if (!string.IsNullOrEmpty(Clear))
                {
                    if (Clear == "Cleared")
                        PurchaseData = PurchaseData.Where(m => m.Total == m.Credit).ToList();
                    else if (Clear == "UnCleared")
                        PurchaseData = PurchaseData.Where(m => m.Total != m.Credit).ToList();
                }
                //total number of rows count     
                recordsTotal = PurchaseData.Count();
                //Paging     
                var data = PurchaseData.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception)
            {

                throw;
            }

        }

        public ActionResult LoadPaymentData()
        {
            int ID = 0;
            if (System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["PurchaseBill_ID"] != null)
            {
                ID = (int)Session["PurchaseBill_ID"];
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
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
                DALSupplier obj = new DALSupplier();
                var PaymentData = obj.DAL_Get_PaymentOfPurchaseReceipt(ID /*SupplierID*/);
                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {

                    PaymentData = PaymentData.OrderBy(sortColumn + " " + sortColumnDir).Skip(skip).Take(pageSize).ToList();
                }
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    PaymentData = PaymentData.Where(m => m.ID.ToString().Contains(searchValue) || m.Date.ToString().Contains(searchValue)).ToList();
                }

                //total number of rows count     
                recordsTotal = PaymentData.Count();
                //Paging     
                var data = PaymentData.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception)
            {

                throw;
            }

        }

        public ActionResult LoadSupplierData()
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
                DALSupplier obj = new DALSupplier();
                var SupplierData = obj.DalGetAllSuplier();
                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {

                    SupplierData = SupplierData.OrderBy(sortColumn + " " + sortColumnDir).Skip(skip).Take(pageSize).ToList();
                }
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    SupplierData = SupplierData.Where(m => m.Name.ToLower().Contains(searchValue) || m.Address.Contains(searchValue) || m.Type.ToLower().Contains(searchValue)).ToList();
                }

                //total number of rows count     
                recordsTotal = SupplierData.Count();
                //Paging     
                var data = SupplierData.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion AjaxLoad

    }
}