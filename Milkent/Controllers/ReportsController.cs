using Milkent.DAL;
using Milkent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;

namespace Milkent.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        // GET: Reports
        public ActionResult SupplierPurchaseReport()
        {
            MdlPurchase mdl = new MdlPurchase();
            DALSupplier obj = new DALSupplier();
            mdl.SupplierList = obj.DalGetAllSuplier();
            return View(mdl);
        }


        public ActionResult CustomerSalesReport()
        {
            MdlSales mdl = new MdlSales();
            DALCustomer obj = new DALCustomer();
            mdl.CustomerList = obj.DalGetAllCustomer();
            return View(mdl);
        }

        public ActionResult IncomeStatement()
        {
           return View();
        }
        [HttpPost]
        public ActionResult ViewIncomeStatement(MdlPurchase mdl)
        {
            ViewBag.Todate = mdl.ToDate;
            ViewBag.Fromdate = mdl.FromDate;
            DALPurchase obj1 = new DALPurchase();
            DALSales obj2 = new DALSales();
            List<MdlPurchase> mdlPurchase = obj1.DalGetAllPurchases();
            List<MdlSales> mdlSales = obj2.DAL_GetAllSales();
            if (mdl.FromDate!=DateTime.MinValue)
            {
                if (mdl.ToDate != DateTime.MinValue)
                {
                    mdlPurchase = mdlPurchase.Where(m => m.Date.Date >= mdl.FromDate && m.Date <= mdl.ToDate).ToList();
                    mdlSales = mdlSales.Where(m => m.Date.Date >= mdl.FromDate && m.Date <= mdl.ToDate).ToList();
                }
                else
                {
                    mdlPurchase = mdlPurchase.Where(m => m.Date.Date >= mdl.FromDate).ToList();
                    mdlSales = mdlSales.Where(m => m.Date.Date >= mdl.FromDate).ToList();
                }
            }
            else if (mdl.ToDate != DateTime.MinValue)
            {
                mdlSales = mdlSales.Where(m => m.Date.Date <= mdl.ToDate).ToList();
                mdlSales = mdlSales.Where(m => m.Date.Date <= mdl.ToDate).ToList();
            }
            ViewBag.Profit = (mdlSales.Sum(m => m.Total) - mdlPurchase.Sum(m => m.Total));
            ViewBag.Sale = mdlSales.Sum(m => m.Total);
            ViewBag.Purchase =  mdlPurchase.Sum(m => m.Total);

            return View();
        }
        public ActionResult LoadPurchasesData()
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
                var Supplier = Request["searchBySupplier"].ToString();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                DALPurchase obj = new DALPurchase();
                var PurchaseData = obj.DalGetAllPurchases();
                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {

                    PurchaseData = PurchaseData.OrderBy(sortColumn + " " + sortColumnDir).Skip(skip).Take(pageSize).ToList();
                }
                //Search    
                if (!string.IsNullOrEmpty(Fromdate))
                {
                    DateTime fromD = Convert.ToDateTime(Fromdate).Date;
                    if (!string.IsNullOrEmpty(Todate))
                    {
                        DateTime toD = Convert.ToDateTime(Todate).Date;
                        PurchaseData = PurchaseData.Where(m => m.Date.Date >= fromD && m.Date <= toD).ToList();
                    }
                    else
                    {
                        PurchaseData = PurchaseData.Where(m => m.Date.Date >= fromD).ToList();
                    }
                }
                else if (!string.IsNullOrEmpty(Todate))
                {
                    DateTime toD = Convert.ToDateTime(Todate).Date;
                    PurchaseData = PurchaseData.Where(m => m.Date.Date <= toD).ToList();
                }
                if (!string.IsNullOrEmpty(DayPart))
                {
                    PurchaseData = PurchaseData.Where(m => m.PartOfDay.Contains(DayPart)).ToList();
                }
                if (!string.IsNullOrEmpty(Supplier))
                {
                    PurchaseData = PurchaseData.Where(m => m.SupplierID == Convert.ToInt32(Supplier)).ToList();
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

        public ActionResult LoadSalesData()
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
                var Customer = Request["searchByCustomer"].ToString();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                DALSales obj = new DALSales();
                var CustomerSalesData = obj.DAL_GetAllSales();
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
                if (!string.IsNullOrEmpty(Customer))
                {
                    CustomerSalesData = CustomerSalesData.Where(m => m.CustomerID==Convert.ToInt32(Customer)).ToList();
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

    }
}