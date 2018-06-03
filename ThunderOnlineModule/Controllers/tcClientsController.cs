using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ThunderOnlineModule.Models;
using PagedList;

namespace ThunderOnlineModule.Controllers
{
    public class tcClientsController : Controller
    {
        private ZionEntities db = new ZionEntities();

        // GET: tcClients
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
            var clients = from c in db.tcClients select c;
            if (!String.IsNullOrEmpty(searchString))
            {
                clients = clients.Where(c => c.Name.Contains(searchString) || c.Parent.Contains(searchString) || c.Id.Contains(searchString));
            }
            clients = clients.OrderBy(c => c.Id);
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(clients.ToPagedList(pageNumber, pageSize));
        }

        // GET: tcClients/Details/5
        public ActionResult Details(string id)
        {
            if (IsAllowed() == false) return RedirectToAction("Index", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tcClient tcClient = db.tcClients.Find(id.Trim());
            if (tcClient == null)
            {
                return HttpNotFound();
            }

            return View(tcClient);
        }

        public ActionResult PrintingDetails(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tcClient tcClient = db.tcClients.Find(id.Trim());
            if (tcClient == null)
            {
                return HttpNotFound();
            }

            return View(tcClient);
        }


        // GET: tcClients/Create
        public ActionResult Create()
        {
            if (IsAllowed() == false) return RedirectToAction("Index", "Login");
            ViewBag.Group = new SelectList(db.tcGroups, "Id", "Name");
            return View();
        }

        // POST: tcClients/Create
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

            ViewBag.Id = new SelectList(db.tcGroups, "Id", "Name", tcClient.Id);
            return View(tcClient);
        }

        // GET: tcClients/Edit/5
        public ActionResult Edit(string id)
        {
            if (IsAllowed() == false) return RedirectToAction("Index", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tcClient tcClient = db.tcClients.Find(id);
            if (tcClient == null)
            {
                return HttpNotFound();
            }
            //ViewBag.Id = new SelectList(db.tcGroups, "Id", "Name", tcClient.Id);
            ViewBag.Group = new SelectList(db.tcGroups, "Id", "Name", tcClient.Group);
            return View(tcClient);
        }

        // POST: tcClients/Edit/5
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
            ViewBag.Id = new SelectList(db.tcGroups, "Id", "Name", tcClient.Id);
            return View(tcClient);
        }

        // GET: tcClients/Delete/5
        public ActionResult Delete(string id)
        {
            if (IsAllowed() == false) return RedirectToAction("Index", "Login");
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

        // POST: tcClients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
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
