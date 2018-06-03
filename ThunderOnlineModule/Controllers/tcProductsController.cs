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
    public class tcProductsController : Controller
    {
        private ZionEntities db = new ZionEntities();

        // GET: tcProducts
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            if (IsAllowed() == false) return RedirectToAction("Index", "Login");
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            var products = from p in db.tcProducts select p;
            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.Name.Contains(searchString));
            }
            products = products.OrderBy(p => p.Id);
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(products.ToPagedList(pageNumber, pageSize));
        }

        // GET: tcProducts/Details/5
        public ActionResult Details(int? id)
        {
            if (IsAllowed() == false) return RedirectToAction("Index", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tcProduct tcProduct = db.tcProducts.Find(id);
            if (tcProduct == null)
            {
                return HttpNotFound();
            }
            return View(tcProduct);
        }

        // GET: tcProducts/Create
        public ActionResult Create()
        {
            if (IsAllowed() == false) return RedirectToAction("Index", "Login");
            return View();
        }

        // POST: tcProducts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Cost,Price,Stock")] tcProduct tcProduct)
        {
            if (ModelState.IsValid)
            {
                db.tcProducts.Add(tcProduct);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tcProduct);
        }

        // GET: tcProducts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (IsAllowed() == false) return RedirectToAction("Index", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tcProduct tcProduct = db.tcProducts.Find(id);
            if (tcProduct == null)
            {
                return HttpNotFound();
            }
            return View(tcProduct);
        }

        // POST: tcProducts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Cost,Price,Stock")] tcProduct tcProduct)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tcProduct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tcProduct);
        }

        // GET: tcProducts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (IsAllowed() == false) return RedirectToAction("Index", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tcProduct tcProduct = db.tcProducts.Find(id);
            if (tcProduct == null)
            {
                return HttpNotFound();
            }
            return View(tcProduct);
        }

        // POST: tcProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tcProduct tcProduct = db.tcProducts.Find(id);
            db.tcProducts.Remove(tcProduct);
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
