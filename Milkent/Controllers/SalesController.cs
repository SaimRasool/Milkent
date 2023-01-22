using Milkent.AccessSpecifier;
using Milkent.DAL;
using Milkent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;

namespace Milkent.Controllers
{
    [Authorize]
    public class SalesController : Controller
    {
        // GET: Sales
        public ActionResult Index()
        {
            MdlSales mdl = new MdlSales();
            DALCustomer obj = new DALCustomer();
            mdl.CustomerList = obj.DalGetAllCustomer();
            return View(mdl);
        }

        public ActionResult NewSale()
        {
            MdlSales mdl = new MdlSales();
            DALCustomer obj = new DALCustomer();
            DALAccount obj2 = new DALAccount();
            ViewBag.Milk = obj2.DAL_Read_Milk();
            mdl.CustomerList = obj.DalGetAllCustomer();
            return View(mdl);
        }
        [HttpPost]
        public ActionResult NewSale(MdlSales mdl)
        {
            if (System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["UserID"] != null)
            {
                mdl.UserID = (int)Session["UserID"];
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
            DALCustomer obj1 = new DALCustomer();
            if (ModelState.IsValid)
            {
                DALAccount obj2 = new DALAccount();
                double chiler = obj2.DAL_Read_Milk();
                if (chiler >= mdl.MilkCredit)
                {
                    if (mdl.Fat == 0 && mdl.LR == 0)
                    {
                        ModelState.AddModelError(string.Empty, "Fat or LR is not be a Zero(0)");
                        mdl.CustomerList = obj1.DalGetAllCustomer();
                        return View(mdl);
                    }
                    MdlCustomer mdlCustomer = obj1.DAL_Read_Customer(mdl.CustomerID);
                    mdl.SalePrice = mdlCustomer.SalePrice;
                    DALSales obj = new DALSales();
                    double fat = mdl.Fat * 0.22;
                    double lr = (mdl.LR / 4) + 0.72;
                    double result = fat + lr;
                    double tl = result + mdl.Fat;
                    double ts = (mdl.MilkCredit * tl) / 13;
                    mdl.Total = mdl.SalePrice * ts;
                    mdl.TS = Math.Round(ts, 2);
                    mdl.Date = DateTime.Today.Date;
                    obj.Add(mdl);
                    return RedirectToAction("NewSale");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "You have Not Enough Milk to Sale Please First Purchase It");
                    mdl.CustomerList = obj1.DalGetAllCustomer();
                    return View(mdl);
                }

            }
            else
            {
                mdl.CustomerList = obj1.DalGetAllCustomer();
                return View(mdl);
            }
        }

        [CheckRole(AllowedRoles = "admin")]
        public ActionResult EditSale(int ID)
        {
            DALSales obj = new DALSales();
            DALAccount obj2 = new DALAccount();
            ViewBag.Milk = obj2.DAL_Read_Milk();
            MdlSales mdl = obj.DAL_GetSale(ID);
            return View(mdl);
        }

        public ActionResult EditSale(MdlSales mdl)
        {
            DALCustomer obj1 = new DALCustomer();
            if (ModelState.IsValid)
            {
                DALSales obj = new DALSales();
                DALAccount obj2 = new DALAccount();
                double chiler = obj2.DAL_Read_Milk();
                MdlSales PreviousSale = obj.DAL_GetSale(mdl.ID);
                if (chiler >= (mdl.MilkCredit-PreviousSale.MilkCredit))
                {
                    if (mdl.Fat == 0 && mdl.LR == 0)
                    {
                        ModelState.AddModelError(string.Empty, "Fat or LR is not be a Zero(0)");
                        mdl.CustomerList = obj1.DalGetAllCustomer();
                        return View(mdl);
                    }
                    MdlCustomer mdlCustomer = obj1.DAL_Read_Customer(mdl.CustomerID);
                    mdl.SalePrice = mdlCustomer.SalePrice;
                    double fat = mdl.Fat * 0.22;
                    double lr = (mdl.LR / 4) + 0.72;
                    double result = fat + lr;
                    double tl = result + mdl.Fat;
                    double ts = (mdl.MilkCredit * tl) / 13;
                    mdl.Total = mdl.SalePrice * ts;
                    mdl.TS = ts;
                    obj.DAL_EditSale(mdl);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "You have Not Enough Milk to Sale Please First Purchase It");
                    mdl.CustomerList = obj1.DalGetAllCustomer();
                    return View(mdl);
                }
            }
            else
            {
                mdl.CustomerList = obj1.DalGetAllCustomer();
                return View(mdl);
            }

        }
        [CheckRole(AllowedRoles = "admin")]
        public ActionResult DeleteSale(int ID)
        {
            DALSales obj = new DALSales();
            MdlSales mdl = obj.DAL_GetSale(ID);
            if (mdl.Bill_ID == 0 && mdl.Date == DateTime.Today.Date)
            {
                if (obj.DAL_DeleteSales(ID) == 1)
                    return Json(true);
                else
                    return Json(false);
            }
            else
                return Json(false);

        }

        #region Receipt
        public ActionResult NewSaleReceipt()
        {
            MdlSales mdl = new MdlSales();
            DALCustomer obj = new DALCustomer();
            mdl.CustomerList = obj.DalGetAllCustomer();
            return View(mdl);
        }
        [HttpPost]
        public ActionResult NewSaleReceipt(MdlSales mdl)
        {
            if (System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["UserID"] != null)
            {
                mdl.UserID = (int)Session["UserID"];
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
            if (mdl.ToDate != DateTime.MinValue && mdl.FromDate != DateTime.MinValue && mdl.CustomerID != 0)
            {
                mdl.Date = DateTime.Today.Date;
                DALSales obj1 = new DALSales();
                DALCustomer obj = new DALCustomer();
                List<MdlSales> SalesData = obj1.DAL_Get_UnpaidCustomerSales(mdl.CustomerID);
                SalesData = SalesData.Where(m => m.Date.Date >= mdl.FromDate && m.Date.Date <= mdl.ToDate).ToList();
                if (SalesData.Count == 0)
                {
                    mdl.CustomerList = obj.DalGetAllCustomer();
                    ModelState.AddModelError(string.Empty, "There is No Sales in between these days");
                    return View(mdl);
                }
                foreach (MdlSales item in SalesData)
                {
                    mdl.Total += item.Total - item.CashDebit;
                }
                mdl.ID = obj1.DAL_AddSales_Bill(mdl);//store a bill ID
                MdlCustomer mdlCustomer = obj.DAL_Read_Customer(mdl.CustomerID);
                foreach (MdlSales item in SalesData)
                {
                    obj1.DAL_Insert_Bill_ID_InSalesTbl(item.ID, mdl.ID);
                }
                return RedirectToAction("ViewReceipt", "Customer", new { mdl.ID });
            }
            else
            {
                DALCustomer obj = new DALCustomer();
                mdl.CustomerList = obj.DalGetAllCustomer();
                ModelState.AddModelError(string.Empty, "Please Select a Dates");
                return View(mdl);
            }
        }
        #endregion


        #region AjaxLoaD
        public ActionResult LoadUnpaidSalesData(int ID)
        {
            try
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var Fromdate = Request["searchByFromdate"].ToString();
                var Todate = Request["searchByTodate"].ToString();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                DALSales obj = new DALSales();
                var SalesData = obj.DAL_Get_UnpaidCustomerSales(ID);
                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {

                    SalesData = SalesData.OrderBy(sortColumn + " " + sortColumnDir).Skip(skip).Take(pageSize).ToList();
                }
                //Search    
                if (!string.IsNullOrEmpty(Fromdate))
                {
                    DateTime fromD = Convert.ToDateTime(Fromdate).Date;
                    if (!string.IsNullOrEmpty(Todate))
                    {
                        DateTime toD = Convert.ToDateTime(Todate).Date;
                        SalesData = SalesData.Where(m => m.Date.Date >= fromD && m.Date <= toD).ToList();
                    }
                    else
                    {
                        SalesData = SalesData.Where(m => m.Date.Date >= fromD).ToList();
                    }
                }
                else
                 if (!string.IsNullOrEmpty(Todate))
                {
                    DateTime toD = Convert.ToDateTime(Todate).Date;
                    SalesData = SalesData.Where(m => m.Date.Date <= toD).ToList();
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

        public ActionResult LoadCustomerSalesData(int ID)
        {
            try
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var Fromdate = Request["searchByFromdate"].ToString();
                var Todate = Request["searchByTodate"].ToString();
                var DayPart = Request["searchByDayPart"].ToString();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                DALSales obj = new DALSales();
                var CustomerSalesData = obj.DAL_GetAllCustomerSales(ID);
                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {

                    CustomerSalesData = CustomerSalesData.OrderBy(sortColumn + " " + sortColumnDir).Skip(skip).Take(pageSize).ToList();
                }
                //Search    
                //Search    
                if (!string.IsNullOrEmpty(Fromdate))
                {
                    DateTime fromD = Convert.ToDateTime(Fromdate).Date;
                    if (!string.IsNullOrEmpty(Todate))
                    {
                        DateTime toD = Convert.ToDateTime(Todate).Date;
                        CustomerSalesData = CustomerSalesData.Where(m => m.Date.Date >= fromD && m.Date <= toD).ToList();
                    }
                    else
                    {
                        CustomerSalesData = CustomerSalesData.Where(m => m.Date.Date >= fromD).ToList();
                    }
                }
                else if (!string.IsNullOrEmpty(Todate))
                {
                    DateTime toD = Convert.ToDateTime(Todate).Date;
                    CustomerSalesData = CustomerSalesData.Where(m => m.Date.Date <= toD).ToList();
                }
                if (!string.IsNullOrEmpty(DayPart))
                {
                    CustomerSalesData = CustomerSalesData.Where(m => m.PartOfDay.Contains(DayPart)).ToList();
                }
                //total number of rows count     
                recordsTotal = CustomerSalesData.Count();
                //Paging     
                var data = CustomerSalesData.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception)
            {

                throw;
            }

        }


        #endregion
    }
}