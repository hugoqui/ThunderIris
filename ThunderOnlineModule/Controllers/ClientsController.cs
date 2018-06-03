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
    public class ClientsController : Controller
    {
        private ZionEntities db = new ZionEntities();

        // GET: Clients
        public ActionResult Index()
        {
            var tcClients = db.tcClients.Include(t => t.tcGroup);
            return View(tcClients.ToList());
        }

        // GET: Clients/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tcClient tcClient = db.tcClients.Find(id);
            if (tcClient == null)
            {
                return HttpNotFound();
            }
            return View(tcClient);
        }

        // GET: Clients/Create
        public ActionResult Create()
        {
            ViewBag.Group = new SelectList(db.tcGroups, "Id", "Name");
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,BirthDate,Group,Email,Parent,Phone,Comments")] tcClient tcClient)
        {
            if (ModelState.IsValid)
            {
                db.tcClients.Add(tcClient);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Group = new SelectList(db.tcGroups, "Id", "Name", tcClient.Group);
            return View(tcClient);
        }

        // GET: Clients/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tcClient tcClient = db.tcClients.Find(id);
            if (tcClient == null)
            {
                return HttpNotFound();
            }
            ViewBag.Group = new SelectList(db.tcGroups, "Id", "Name", tcClient.Group);
            return View(tcClient);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,BirthDate,Group,Email,Parent,Phone,Comments")] tcClient tcClient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tcClient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Group = new SelectList(db.tcGroups, "Id", "Name", tcClient.Group);
            return View(tcClient);
        }

        // GET: Clients/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tcClient tcClient = db.tcClients.Find(id);
            if (tcClient == null)
            {
                return HttpNotFound();
            }
            return View(tcClient);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            tcClient tcClient = db.tcClients.Find(id);
            db.tcClients.Remove(tcClient);
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
