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
    public class tcServicesController : Controller
    {
        private ZionEntities db = new ZionEntities();

        // GET: tcServices
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
            var services = from s in db.tcServices select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                services = services.Where(p => p.Name.Contains(searchString));
            }
            services = services.OrderBy(p => p.Id);
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(services.ToPagedList(pageNumber, pageSize));
        }

        // GET: tcServices/Details/5
        public ActionResult Details(int? id)
        {
            if (IsAllowed() == false) return RedirectToAction("Index", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tcService tcService = db.tcServices.Find(id);
            if (tcService == null)
            {
                return HttpNotFound();
            }
            return View(tcService);
        }

        // GET: tcServices/Create
        public ActionResult Create()
        {
            if (IsAllowed() == false) return RedirectToAction("Index", "Login");
            return View();
        }

        // POST: tcServices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Price")] tcService tcService)
        {
            if (ModelState.IsValid)
            {
                db.tcServices.Add(tcService);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tcService);
        }

        // GET: tcServices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (IsAllowed() == false) return RedirectToAction("Index", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tcService tcService = db.tcServices.Find(id);
            if (tcService == null)
            {
                return HttpNotFound();
            }
            return View(tcService);
        }

        // POST: tcServices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Price")] tcService tcService)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tcService).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tcService);
        }

        // GET: tcServices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (IsAllowed() == false) return RedirectToAction("Index", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tcService tcService = db.tcServices.Find(id);
            if (tcService == null)
            {
                return HttpNotFound();
            }
            return View(tcService);
        }

        // POST: tcServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tcService tcService = db.tcServices.Find(id);
            db.tcServices.Remove(tcService);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ServiceSale()
        {
            if (IsAllowed() == false) return RedirectToAction("Index", "Login");
            return View();
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
