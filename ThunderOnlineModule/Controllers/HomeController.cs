using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThunderOnlineModule.Models;

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
                string connetionString = null;
                SqlConnection cnn;
                connetionString = "Data Source=TCESbase.db.10029297.hostedresource.com;Initial Catalog=TCESbase;User ID=TCESbase;Password=Salmo119@165";
                cnn = new SqlConnection(connetionString);

                SqlCommand testCMD = new SqlCommand("sp_spaceused", cnn);
                testCMD.CommandType = CommandType.StoredProcedure; cnn.Open();

                SqlDataReader reader = testCMD.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var msg = reader["database_size"];
                        ViewBag.SpaceUsed = msg;
                    }
                }
                ViewBag.UserName = System.Web.HttpContext.Current.Session["logged"].ToString();
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