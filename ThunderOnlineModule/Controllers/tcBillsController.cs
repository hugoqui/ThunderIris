using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ThunderOnlineModule.Models;

namespace ThunderOnlineModule.Controllers
{
    public class tcBillsController : Controller
    {
        private ZionEntities db = new ZionEntities();

        // GET: tcBills
        public ActionResult Index(string currentFilter, int? page = 1)
        {
            if (IsAllowed() == false) return RedirectToAction("Index", "Login");
            var bills = from b in db.tcBills.Include(t => t.tcClient).Include(t => t.tcUser) select b;
            bills = bills.OrderByDescending(b => b.Id);
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(bills.ToPagedList(pageNumber, pageSize));
        }

        // GET: tcBills/Details/5
        public ActionResult Details(int? id)
        {
            if (IsAllowed() == false) return RedirectToAction("Index", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tcBill tcBill = db.tcBills.Find(id);
            if (tcBill == null)
            {
                return HttpNotFound();
            }
            return View(tcBill);
        }

        public ActionResult Delete(int? id)
        {
            if (IsAllowed() == false) return RedirectToAction("Index", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tcBill tcBill = db.tcBills.Find(id);
            if (tcBill == null)
            {
                return HttpNotFound();
            }
            return View(tcBill);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tcBill bill = db.tcBills.Find(id);
            //first step = return the products stock            
            foreach (var product in bill.tcProductSales)
            {
                UpdateProductStock(product.ProductId, product.Quantity);                
            }
            //second step = delete all productSales, serviceSales, clientDetails
            var list = new List<tcProductSale>();
            foreach (var item in bill.tcProductSales){list.Add(item);}
            for (int i = 0; i < list.Count; i++){DeleteProductSale(list[i]);}

            var saleList = new List<tcServiceSale>();
            foreach (var sale in bill.tcServiceSales){saleList.Add(sale);}
            for (int i = 0; i < saleList.Count; i++){db.tcServiceSales.Remove(saleList[i]);}

            var detailList = new List<tcClientDetail>();
            foreach (var detail in bill.tcClientDetails){detailList.Add(detail);}
            for (int i = 0; i < detailList.Count; i++){db.tcClientDetails.Remove(detailList[i]);}

            //now delete the bill
            db.tcBills.Remove(bill);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        void UpdateProductStock(int? id, int? quantity)
        {
            var product = db.tcProducts.Find(id);
            product.Stock = product.Stock + quantity;
            db.Entry(product).State = EntityState.Modified;
            //db.SaveChanges(); //saved in caller
        }

        void DeleteProductSale(tcProductSale item)
        {   
            db.tcProductSales.Remove(item);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
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
