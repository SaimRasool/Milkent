using Milkent.AccessSpecifier;
using Milkent.DAL;
using Milkent.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;

namespace Milkent.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        #region Customer
        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddCustomer()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult AddCustomer(MdlCustomer mdl, HttpPostedFileBase file)
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
                        return PartialView();
                    }
                    DALCustomer obj = new DALCustomer();
                    obj.Add(mdl);
                }
                else
                {
                    return PartialView(mdl);
                }
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }

        }
        [CheckRole(AllowedRoles = "admin")]
        public ActionResult EditCustomer(int ID)
        {
            DALCustomer obj = new DALCustomer();
            MdlCustomer mdl = obj.DAL_Read_Customer(ID);
            return View(mdl);
        }
        [HttpPost]
        [CheckRole(AllowedRoles = "admin")]
        public ActionResult EditCustomer(MdlCustomer mdl, HttpPostedFileBase file)
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
                        var newFileName = Guid.NewGuid()+ext;
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
                    DALCustomer obj = new DALCustomer();
                    obj.DAL_Update_Customer(mdl);
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
        public ActionResult DeleteCustomer(int ID)
        {
            try
            {
                DALCustomer obj = new DALCustomer();
                MdlCustomer mdl = obj.DAL_Read_Customer(ID);
                mdl.Image = mdl.Image.Replace("/../", "~/");
                string path = Server.MapPath(mdl.Image);
                if(System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                if(obj.DAL_Delete_Customer(ID)==1)
                    return Json(true);
                else
                    return Json(false);
            }
            catch (Exception)
            {

                throw;
            }
          
        }
        #endregion

        #region CustomerReceipt
        public ActionResult CustomerReceipt()
        {
            MdlSales mdl = new MdlSales();
            DALCustomer obj = new DALCustomer();
            mdl.CustomerList = obj.DalGetAllCustomer();
            return View(mdl);
        }

        public ActionResult ViewReceipt(int ID /*Bill_ID*/)
        {
            DALCustomer obj = new DALCustomer();
            MdlSales mdl = obj.DAL_Get_SaleReceipt(ID/*Bill_ID*/);
            DALAccount obj1 = new DALAccount();
            TempData["User"] = obj1.DAL_Read_Account(mdl.UserID);
            DALSales obj2 = new DALSales();
            TempData["modelList"] = obj2.DAL_GetAllSalesOf_OneReceipt(ID/*Bill_ID*/);
            TempData["Customer"] = obj.DAL_Read_Customer(mdl.CustomerID/*Customer_ID*/);
            return View(mdl);
        }
        [CheckRole(AllowedRoles = "admin")]
        public ActionResult DeleteSaleReceipt(int ID /*Bill_ID*/)
        {
            DALCustomer obj = new DALCustomer();
            MdlSales mdl = obj.DAL_Get_SaleReceipt(ID);
            if (mdl.CashDebit == 0)
            {
                if (obj.Dal_Delete_CustomerSalesReceipt(ID) >= 1)
                    return Json(true);
                else
                    return Json(false);
            }
            else
                return Json(false);
        }
        #endregion CustomerReceipt

        #region CustomerPayment
        public ActionResult CustomerPayment(int ID/*Bill ID*/)
        {
            Session["SaleBill_ID"] = ID;
            return View();
        }

        public ActionResult Addpayment()
        {
            int ID = 0;
            if (System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["SaleBill_ID"] != null)
            {
                ID = (int)Session["SaleBill_ID"];
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
            DALCustomer obj = new DALCustomer();
            MdlSales mdl = obj.DAL_Get_SaleReceipt(ID);
            MdlPayment mdlPayment = new MdlPayment();
            mdlPayment.Credit_Debit = mdl.CashDebit;
            mdlPayment.Total = mdl.Total;
            mdlPayment.Bill_ID = mdl.ID;
            return View(mdlPayment);
        }
        [HttpPost]
        public ActionResult Addpayment(MdlPayment mdl)
        {
            if (ModelState.IsValid)
            {
                DALCustomer obj = new DALCustomer();
                MdlSales mdlReceipt = obj.DAL_Get_SaleReceipt(mdl.Bill_ID);
                if (mdlReceipt.Total != mdlReceipt.CashDebit)
                {
                    if (mdlReceipt.Total - (mdlReceipt.CashDebit + mdl.Credit_Debit) < 1)// if remaining balance is less than 1 than Receipt will be mark as cleared
                        mdl.Total = mdlReceipt.Total;//in this total new debit value is store that update in receipt table debit
                    else//else new debit added to perivios
                        mdl.Total = mdlReceipt.CashDebit + mdl.Credit_Debit;
                    mdl.Date = DateTime.Today.Date;
                    obj.DAL_AddCustomerPayment(mdl);
                    return RedirectToAction("CustomerPayment", new { id = mdl.Bill_ID });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Bill is Cleared now you dont add further payment");
                    return View(mdl);
                }
            }
            return PartialView(mdl);
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
                DALCustomer obj = new DALCustomer();
                MdlPayment mdlPrevious = obj.DAL_Get_CustomerPayment(mdlNew.ID);
                MdlSales mdlReceipt = obj.DAL_Get_SaleReceipt(mdlNew.Bill_ID);
                if (mdlPrevious.Credit_Debit < mdlNew.Credit_Debit)
                {//if new value is greater than perivious than difference added to Debit of SaleReceipt
                    double TotalDebit = mdlNew.Total + mdlPrevious.Credit_Debit;
                    if ((TotalDebit - mdlNew.Credit_Debit)<1)//if new value ie equal to remaing balnce than receipt will be cleared
                        mdlNew.Total = mdlReceipt.Total;
                    else
                        mdlNew.Total = mdlReceipt.CashDebit + (mdlNew.Credit_Debit - mdlPrevious.Credit_Debit);
                }
                else if (mdlPrevious.Credit_Debit > mdlNew.Credit_Debit)
                {//if new value is less than perivious than difference subtract from to Debit in SaleReceipt
                    mdlNew.Total = mdlReceipt.CashDebit - (mdlPrevious.Credit_Debit - mdlNew.Credit_Debit);
                }
                obj.DAL_EditCustomerPayment(mdlNew);
                return RedirectToAction("CustomerPayment", new { id= mdlNew.Bill_ID });
            }
            return View(mdlNew);
        }
        [CheckRole(AllowedRoles = "admin")]
        public ActionResult DeletePayment(int ID)
        {
            DALCustomer obj = new DALCustomer();
            if (obj.DAL_Delete_CustomerPayment(ID) == 1)
                return Json(true);
            else
                return Json(false);
        }
        #endregion CustomerPayment

        #region AjaxLoad
        public ActionResult LoadCustomerData()
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
                DALCustomer obj = new DALCustomer();
                var CustomerData = obj.DalGetAllCustomer();
                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {

                    CustomerData = CustomerData.OrderBy(sortColumn + " " + sortColumnDir).Skip(skip).Take(pageSize).ToList();
                }
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    CustomerData = CustomerData.Where(m => m.Name.ToLower().Contains(searchValue) || m.Address.ToLower().Contains(searchValue)).ToList();
                }

                //total number of rows count     
                recordsTotal = CustomerData.Count();
                //Paging     
                var data = CustomerData.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception)
            {

                throw;
            }

        }

        public ActionResult LoadSalesReceipt(int ID/*CustomerID*/)
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
                DALCustomer obj = new DALCustomer();
                var SalesData = obj.DAL_GetAll_CustomerSalesReceipt(ID);
                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {

                    SalesData = SalesData.OrderBy(sortColumn + " " + sortColumnDir).Skip(skip).Take(pageSize).ToList();
                }
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    SalesData = SalesData.Where(m => m.Date.Date.ToString().Contains(searchValue)).ToList();
                }
                if (!string.IsNullOrEmpty(Clear))
                {
                    if (Clear == "Cleared")
                        SalesData = SalesData.Where(m => m.Total == m.CashDebit).ToList();
                    else if (Clear == "UnCleared")
                        SalesData = SalesData.Where(m => m.Total != m.CashDebit).ToList();
                }
                //total number of rows count     
                recordsTotal = SalesData.Count();
                //Paging     
                var data = SalesData.Skip(skip).Take(pageSize).ToList();
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
            if (System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["SaleBill_ID"] != null)
            {
                ID = (int)Session["SaleBill_ID"];
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
                DALCustomer obj = new DALCustomer();
                var PaymentData = obj.DAL_Get_PaymentOfSaleReceipt(ID /*CustomerID*/);
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
        #endregion AjaxLoad
    }
}