using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThunderOnlineModule.Models;

namespace ThunderOnlineModule.Controllers
{
    public class LoginController : Controller
    {
        private ZionEntities db = new ZionEntities();

        // GET: Login
        public ActionResult Index(string msg = "")
        {
            if (msg != "") ViewBag.ErrorMessage = msg;
            return View();
        }

        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string UserName, string Password)
        {
            var user = db.tcUsers.Find(UserName.Trim());
            if (user != null)
            {
                if (user.Password == Password.Trim())
                {
                    System.Web.HttpContext.Current.Session["logged"] = UserName;
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Index", "Login" , new { msg = "Datos incorrectos." });
        }

        public ActionResult LogOut()
        {
            System.Web.HttpContext.Current.Session["logged"] = null;
            return RedirectToAction("Index");
        }
    }
}