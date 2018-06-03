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
    public class tcProductSalesController : Controller
    {
        private ZionEntities db = new ZionEntities();

        public ActionResult Sale()
        {
            if (IsAllowed() == false) return RedirectToAction("Index", "Login");
            return View();
        }
        // GET: tcProductSales
        public ActionResult Index()
        {
            if (IsAllowed() == false) return RedirectToAction("Index", "Login");
            var sl = db.tcProductSales;
            var tcProductSales = db.tcProductSales.Include(t => t.tcBill).Include(t => t.tcProduct);
            return View(tcProductSales);
        }

        // GET: tcProductSales/Details/5
        public ActionResult Details(int? id)
        {
            if (IsAllowed() == false) return RedirectToAction("Index", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tcBill tcBill = db.tcBills.Find(id);
            if (tcBill== null)
            {
                return HttpNotFound();
            }
            return View(tcBill);
        }

        // GET: tcProductSales/Create
        public ActionResult Create()
        {
            if (IsAllowed() == false) return RedirectToAction("Index", "Login");
            ViewBag.BillId = new SelectList(db.tcBills, "Id", "ClientId");
            ViewBag.ProductId = new SelectList(db.tcProducts, "Id", "Name");
            return View();
        }

        // POST: tcProductSales/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProductId,Quantity,Price,BillId")] tcProductSale tcProductSale)
        {
            if (ModelState.IsValid)
            {
                db.tcProductSales.Add(tcProductSale);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BillId = new SelectList(db.tcBills, "Id", "ClientId", tcProductSale.BillId);
            ViewBag.ProductId = new SelectList(db.tcProducts, "Id", "Name", tcProductSale.ProductId);
            return View(tcProductSale);
        }

        // GET: tcProductSales/Edit/5
        public ActionResult Edit(int? id)
        {
            if (IsAllowed() == false) return RedirectToAction("Index", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tcProductSale tcProductSale = db.tcProductSales.Find(id);
            if (tcProductSale == null)
            {
                return HttpNotFound();
            }
            ViewBag.BillId = new SelectList(db.tcBills, "Id", "ClientId", tcProductSale.BillId);
            ViewBag.ProductId = new SelectList(db.tcProducts, "Id", "Name", tcProductSale.ProductId);
            return View(tcProductSale);
        }

        // POST: tcProductSales/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProductId,Quantity,Price,BillId")] tcProductSale tcProductSale)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tcProductSale).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BillId = new SelectList(db.tcBills, "Id", "ClientId", tcProductSale.BillId);
            ViewBag.ProductId = new SelectList(db.tcProducts, "Id", "Name", tcProductSale.ProductId);
            return View(tcProductSale);
        }

        // GET: tcProductSales/Delete/5
        public ActionResult Delete(int? id)
        {
            if (IsAllowed() == false) return RedirectToAction("Index", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tcProductSale tcProductSale = db.tcProductSales.Find(id);
            if (tcProductSale == null)
            {
                return HttpNotFound();
            }
            return View(tcProductSale);
        }

        // POST: tcProductSales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tcProductSale tcProductSale = db.tcProductSales.Find(id);
            db.tcProductSales.Remove(tcProductSale);
            db.SaveChanges();
            return RedirectToAction("Index");
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
