using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThunderOnlineModule.Models;
namespace ThunderOnlineModule.Controllers
{
    public class ReportsController : Controller
    {
        private ZionEntities db = new ZionEntities();
        
        public ActionResult Index()
        {
            if (IsAllowed() == false) return RedirectToAction("Index", "Login");
            return View();
        }
        
        public ActionResult SalesReport(DateTime? fromDate, DateTime? toDate)
        {
            if (IsAllowed() == false) return RedirectToAction("Index", "Login");
            var list = from b in db.tcBills
                       where b.Date >= fromDate && b.Date <= toDate
                       select b;

            ViewBag.fromDate = fromDate.Value.ToShortDateString();
            ViewBag.toDate = toDate.Value.ToShortDateString();
            return View(list.ToList());
        }

        public ActionResult ServiceIdex()
        {
            if (IsAllowed() == false) return RedirectToAction("Index", "Login");
            return View();
        }

        public ActionResult ServiceSalesReport(DateTime? fromDate, DateTime? toDate)
        {
            if (IsAllowed() == false) return RedirectToAction("Index", "Login");
            var list = from b in db.tcBills
                       where b.Date >= fromDate && b.Date <= toDate
                       select b;

            ViewBag.fromDate = fromDate.Value.ToShortDateString();
            ViewBag.toDate = toDate.Value.ToShortDateString();
            return View(list.ToList());
        }

        public ActionResult FinancialReport(DateTime? fromDate, DateTime? toDate)
        {
            if (IsAllowed() == false) return RedirectToAction("Index", "Login");
            var list = from b in db.tcBills
                       where b.Date >= fromDate && b.Date <= toDate
                       select b;

            if (fromDate == null || toDate==null)
            {
                fromDate = DateTime.Today;
                toDate = DateTime.Today;
            }

            ViewBag.fromYear = fromDate.Value.Year;
            ViewBag.fromMonth = fromDate.Value.Month;
            ViewBag.fromDay = fromDate.Value.Day;

            ViewBag.toYear = toDate.Value.Year;
            ViewBag.toMonth= toDate.Value.Month;
            ViewBag.toDay = toDate.Value.Day;

            var expenses = from e in db.tcExpenses
                           where e.Date >= fromDate && e.Date <= toDate
                           select e;

            ViewBag.Expenses = expenses;

            return View(list.ToList());
        }

        public ActionResult ClientsByGroup(int? id)
        {
            IEnumerable<tcClient> list = new List<tcClient>();
            ViewBag.Groups = db.tcGroups.ToList();
            if (id ==null)
                return View(list);
                        
            ViewBag.GroupName = db.tcGroups.Find(id).Name;

            var group = db.tcGroups.Find(id);
            return View(group.tcClients);
        }

        public ActionResult ExpenseReport(DateTime? fromDate, DateTime? toDate)
        {
            if (IsAllowed() == false) return RedirectToAction("Index", "Login");
            var list = from b in db.tcExpenses
                       where b.Date >= fromDate && b.Date <= toDate
                       select b;

            if (fromDate == null || toDate == null)
            {
                fromDate = DateTime.Today;
                toDate = DateTime.Today;
            }

            ViewBag.fromYear = fromDate.Value.Year;
            ViewBag.fromMonth = fromDate.Value.Month;
            ViewBag.fromDay = fromDate.Value.Day;

            ViewBag.toYear = toDate.Value.Year;
            ViewBag.toMonth = toDate.Value.Month;
            ViewBag.toDay = toDate.Value.Day;

            return View(list.ToList());

        }

        bool IsAllowed()
        {
            try
            {
                var access = System.Web.HttpContext.Current.Session["logged"].ToString();
                if (access != null) return true;
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }
    }
}