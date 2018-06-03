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
    public class tcExpensesController : Controller
    {
        private ZionEntities db = new ZionEntities();

        // GET: tcExpenses
        public ActionResult Index()
        {
            var list = db.tcExpenses.OrderByDescending(e => e.Id).Take(10);
            return View(list.ToList());
        }

        // GET: tcExpenses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tcExpense tcExpense = db.tcExpenses.Find(id);
            if (tcExpense == null)
            {
                return HttpNotFound();
            }
            return View(tcExpense);
        }

        // GET: tcExpenses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: tcExpenses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Comments,Amount,Date")] tcExpense tcExpense)
        {
            if (ModelState.IsValid)
            {
                db.tcExpenses.Add(tcExpense);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tcExpense);
        }

        // GET: tcExpenses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tcExpense tcExpense = db.tcExpenses.Find(id);
            if (tcExpense == null)
            {
                return HttpNotFound();
            }
            return View(tcExpense);
        }

        // POST: tcExpenses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Comments,Amount,Date")] tcExpense tcExpense)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tcExpense).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tcExpense);
        }

        // GET: tcExpenses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tcExpense tcExpense = db.tcExpenses.Find(id);
            if (tcExpense == null)
            {
                return HttpNotFound();
            }
            return View(tcExpense);
        }

        // POST: tcExpenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tcExpense tcExpense = db.tcExpenses.Find(id);
            db.tcExpenses.Remove(tcExpense);
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
    }
}
