using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KJ.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Content()
        {
            return PartialView();
        }

        public ActionResult MenuLeft()
        {
            return PartialView();
        }

        public ActionResult Head()
        {
            return PartialView();
        }

        public ActionResult Footer()
        {
            return PartialView();
        }

        public ActionResult Error(string message="")
        {
            ViewBag.Meaasga = message;
            return PartialView();
        }


    }
}