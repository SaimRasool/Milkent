using Milkent.DAL;
using Milkent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Milkent.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            DALAccount obj = new DALAccount();
            DALPurchase obj1 = new DALPurchase();
            DALSales obj2 = new DALSales();
            DALSupplier obj3 = new DALSupplier();
            DALCustomer obj4 = new DALCustomer();
            List<MdlPurchase> mdlPurchase = obj1.DalGetAllPurchases();
            List<MdlSales> mdlSales = obj2.DAL_GetAllSales();
            List<MdlSupplier> mdlSuppliers = obj3.DalGetAllSuplier();
            List<MdlCustomer> mdlCustomer = obj4.DalGetAllCustomer();

            mdlPurchase = mdlPurchase.Where(m => m.Date.Year == DateTime.Now.Year&& m.Date.Month == DateTime.Now.Month).ToList();
            mdlSales = mdlSales.Where(m => m.Date.Year == DateTime.Now.Year && m.Date.Month == DateTime.Now.Month).ToList();
            ViewBag.NoOfMonthSales = mdlSales.Count;
            ViewBag.MonthMilkSales = mdlSales.Sum(m => m.MilkCredit);
            ViewBag.MonthSales = mdlSales.Sum(m => m.Total);
            ViewBag.NoOfMonthPurchases = mdlPurchase.Count;
            ViewBag.MonthMilkPurchases = mdlPurchase.Sum(m => m.Milk);
            ViewBag.MonthPurchases = mdlPurchase.Sum(m => m.Total);
            ViewBag.MonthProfit = (mdlSales.Sum(m => m.Total)-mdlPurchase.Sum(m => m.Total));



            mdlPurchase = mdlPurchase.Where(m => m.Date==DateTime.Today.Date).ToList();
            mdlSales = mdlSales.Where(m => m.Date==DateTime.Today.Date).ToList();
            ViewBag.NoOfSales = mdlSales.Count;
            ViewBag.MilkSales = mdlSales.Sum(m=>m.MilkCredit);
            ViewBag.Sales = mdlSales.Sum(m=>m.Total);
            ViewBag.NoOfPurchases = mdlPurchase.Count;
            ViewBag.MilkPurchases = mdlPurchase.Sum(m => m.Milk);
            ViewBag.Purchases = mdlPurchase.Sum(m => m.Total);
            ViewBag.Profit = (mdlSales.Sum(m => m.Total) - mdlPurchase.Sum(m => m.Total));


            ViewBag.Milk = obj.DAL_Read_Milk();
            ViewBag.Supplier = mdlSuppliers.Count;
            ViewBag.Customer = mdlCustomer.Count;
            return View();
        }

        public ActionResult ErrorPage()
        {
            return View();
        }
    }
}
