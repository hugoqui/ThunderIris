using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ThunderOnlineModule.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            if (IsAllowed() == false)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                ViewBag.UserName =  System.Web.HttpContext.Current.Session["logged"].ToString();
            }

            return View();
        }

        public ActionResult Hugo()
        {
            return View();
        }

        public ActionResult Sin()
        {
            return View();
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