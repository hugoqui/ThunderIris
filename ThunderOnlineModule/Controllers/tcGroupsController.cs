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
    public class tcGroupsController : Controller
    {
        private ZionEntities db = new ZionEntities();

        // GET: tcGroups
        public ActionResult Index()
        {
            if (IsAllowed() == false) return RedirectToAction("Index", "Login");
            var tcGroups = db.tcGroups;
            return View(tcGroups.ToList());
        }

        // GET: tcGroups/Details/5
        public ActionResult Details(int? id)
        {
            if (IsAllowed() == false) return RedirectToAction("Index", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tcGroup tcGroup = db.tcGroups.Find(id);
            if (tcGroup == null)
            {
                return HttpNotFound();
            }
            return View(tcGroup);
        }

        // GET: tcGroups/Create
        public ActionResult Create()
        {
            if (IsAllowed() == false) return RedirectToAction("Index", "Login");
            ViewBag.Id = new SelectList(db.tcClients, "Id", "Name");
            return View();
        }

        // POST: tcGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] tcGroup tcGroup)
        {
            if (ModelState.IsValid)
            {
                db.tcGroups.Add(tcGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.tcClients, "Id", "Name", tcGroup.Id);
            return View(tcGroup);
        }

        // GET: tcGroups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (IsAllowed() == false) return RedirectToAction("Index", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tcGroup tcGroup = db.tcGroups.Find(id);
            if (tcGroup == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.tcClients, "Id", "Name", tcGroup.Id);
            return View(tcGroup);
        }

        // POST: tcGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] tcGroup tcGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tcGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.tcClients, "Id", "Name", tcGroup.Id);
            return View(tcGroup);
        }

        // GET: tcGroups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (IsAllowed() == false) return RedirectToAction("Index", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tcGroup tcGroup = db.tcGroups.Find(id);
            if (tcGroup == null)
            {
                return HttpNotFound();
            }
            return View(tcGroup);
        }

        // POST: tcGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tcGroup tcGroup = db.tcGroups.Find(id);
            db.tcGroups.Remove(tcGroup);
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
