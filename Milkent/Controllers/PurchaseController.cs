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
    public class PurchaseController : Controller
    {
        // GET: Purchase
        public ActionResult Index()
        {
            MdlPurchase mdl = new MdlPurchase();
            DALSupplier obj = new DALSupplier();
            mdl.SupplierList = obj.DalGetAllSuplier();
            return View(mdl);
        }

        public ActionResult NewPurchases()
        {
            MdlPurchase mdl = new MdlPurchase();
            DALSupplier obj = new DALSupplier();
            mdl.SupplierList = obj.DalGetAllSuplier();
            return View(mdl);
        }
        [HttpPost]
        public ActionResult NewPurchases(MdlPurchase mdl)
        {
            if (System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["UserID"] != null)
            {
                mdl.UserID = (int)Session["UserID"];
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
            DALSupplier obj1 = new DALSupplier();
            if (ModelState.IsValid)
            {
                DALPurchase obj = new DALPurchase();
                MdlSupplier mdlSupllier = obj1.DAL_Read_Supplier(mdl.SupplierID);
                mdl.PurchasePrice = mdlSupllier.PurchasePrice;
                if (mdlSupllier.Type == "Simple")
                {
                    mdl.Total = mdlSupllier.PurchasePrice * mdl.Milk;
                    mdl.TS = mdl.Milk;
                }
                else if (mdlSupllier.Type == "Doodh Wala")
                {
                    if (mdl.Fat == 0 && mdl.LR == 0)
                    {
                        ModelState.AddModelError(string.Empty, "Fat or LR is not be a Zero(0)");
                        mdl.SupplierList = obj1.DalGetAllSuplier();
                        return View(mdl);
                    }

                    double fat = mdl.Fat * 0.22;
                    double lr = (mdl.LR / 4) + 0.72;
                    double result = fat + lr;
                    double tl = result + mdl.Fat;
                    double ts = (mdl.Milk * tl) / 13;
                    mdl.Total = mdlSupllier.PurchasePrice * ts;
                    mdl.TS = Math.Round(ts, 2);
                }
                mdl.Date = DateTime.Today.Date;
                obj.Add(mdl);
                return RedirectToAction("NewPurchases");
            }
            mdl.SupplierList = obj1.DalGetAllSuplier();
            return View(mdl);
        }
        [CheckRole(AllowedRoles = "admin")]
        public ActionResult EditPurchase(int ID)
        {
            MdlPurchase mdl = new MdlPurchase();
            DALPurchase obj = new DALPurchase();
            DALSupplier obj1 = new DALSupplier();
            mdl = obj.DalGetPurchase(ID);
            MdlSupplier mdlSupllier = obj1.DAL_Read_Supplier(mdl.SupplierID);
            ViewBag.Type = mdlSupllier.Type;
            if (mdl.Date != DateTime.Today.Date|| mdl.Bill_ID!=0)
            {
                ModelState.AddModelError(string.Empty, "You Cant Edit This at that time");
                return View(mdl);
            }
            else
                return View(mdl);
        }
        [CheckRole(AllowedRoles = "admin")]
        public ActionResult EditPurchase(MdlPurchase mdl)
        {
            if (mdl.Date != DateTime.Today.Date || mdl.Bill_ID != 0)
            {
                    ModelState.AddModelError(string.Empty, "You Cant Edit This at that time");
                    return View(mdl);
            }
            DALSupplier obj1 = new DALSupplier();
            if (ModelState.IsValid)
            {
                DALPurchase obj = new DALPurchase();
                MdlSupplier mdlSupllier = obj1.DAL_Read_Supplier(mdl.SupplierID);
                mdl.PurchasePrice = mdlSupllier.PurchasePrice;
                if (mdlSupllier.Type == "Simple")
                {
                    mdl.Total = mdlSupllier.PurchasePrice * mdl.Milk;
                    mdl.TS = mdl.Milk;
                }
                else if (mdlSupllier.Type == "Doodh Wala")
                {
                    if (mdl.Fat == 0 && mdl.LR == 0)
                    {
                        ModelState.AddModelError(string.Empty, "Fat or LR is not be a Zero(0)");
                        mdl.SupplierList = obj1.DalGetAllSuplier();
                        return View(mdl);
                    }
                    double fat = mdl.Fat * 0.22;
                    double lr = (mdl.LR / 4) + 0.72;
                    double result = fat + lr;
                    double tl = result + mdl.Fat;
                    double ts = (mdl.Milk * tl) / 13;
                    mdl.Total = mdlSupllier.PurchasePrice * ts;
                    mdl.TS = Math.Round(ts, 2);
                }
                obj.EditPurchase(mdl);
                return RedirectToAction("Index");
            }
            else
            {
                return View(mdl);
            }
            
        }
        [CheckRole(AllowedRoles = "admin")]
        public ActionResult DeletePurchase(int ID)
        {
            DALPurchase obj = new DALPurchase();
            MdlPurchase mdl = obj.DalGetPurchase(ID);
            if(mdl.Bill_ID==0&& mdl.Date == DateTime.Today.Date)
            {
                if (obj.DAL_Delete_Purchase(ID) == 1)
                    return Json(true);
                else
                    return Json(false);
            }
            else
                return Json(false);
        }

        #region Receipt

        public ActionResult NewPurchaseBill()
        {
            MdlPurchase mdl = new MdlPurchase();
            DALSupplier obj = new DALSupplier();
            mdl.SupplierList = obj.DalGetAllSuplier();
            return View(mdl);
        }
        [HttpPost]
        public ActionResult NewPurchaseBill(MdlPurchase mdl)
        {
            if (System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["UserID"] != null)
            {
                mdl.UserID = (int)Session["UserID"];
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
            if (mdl.ToDate!= DateTime.MinValue && mdl.FromDate!= DateTime.MinValue && mdl.SupplierID!=0)
            {
                mdl.Date = DateTime.Today.Date;
                DALPurchase obj1 = new DALPurchase();
                DALSupplier obj = new DALSupplier();
                List<MdlPurchase> PurchaseData = obj1.DAL_Get_UnpaidSupplierPurchases(mdl.SupplierID);
                PurchaseData = PurchaseData.Where(m => m.Date.Date >= mdl.FromDate && m.Date.Date <= mdl.ToDate).ToList();
                if(PurchaseData.Count==0)
                {
                    mdl.SupplierList = obj.DalGetAllSuplier();
                    ModelState.AddModelError(string.Empty, "There is no Purchases in between these days");
                    return View(mdl);
                }
                foreach (MdlPurchase item in PurchaseData)
                {
                    mdl.Total += item.Total - item.Credit;
                }
                mdl.ID = obj1.DAL_AddPurchase_Bill(mdl);//store a bill ID
                MdlSupplier mdlSupplier = obj.DAL_Read_Supplier(mdl.SupplierID);
                foreach (MdlPurchase item in PurchaseData)
                {
                    obj1.DAL_Insert_Bill_ID_InPurchaseTbl(item.ID, mdl.ID);
                }
                return RedirectToAction("ViewReceipt", "Supplier",new { mdl.ID});
            }
            else
            {
                DALSupplier obj = new DALSupplier();
                mdl.SupplierList = obj.DalGetAllSuplier();
                ModelState.AddModelError(string.Empty, "Please Select a Dates");
                return View(mdl);
            }
        }

        #endregion


        #region AjaxLoad
        public ActionResult LoadUnpaidPurchasesData(int ID)
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
                DALPurchase obj = new DALPurchase();
                var PurchaseData = obj.DAL_Get_UnpaidSupplierPurchases(ID);
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
                else
                 if (!string.IsNullOrEmpty(Todate))
                {
                    DateTime toD = Convert.ToDateTime(Todate).Date;
                    PurchaseData = PurchaseData.Where(m => m.Date.Date <= toD).ToList();
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

        public ActionResult LoadPurchasesData(int ID)
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
                DALPurchase obj = new DALPurchase();
                var PurchaseData = obj.DalGetAllSupplierPurchases(ID);
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

        #endregion
    }
}