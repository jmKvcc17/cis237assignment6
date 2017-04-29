using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cis237Assignment6.Controllers
{
    public class HomeController : Controller
    {
        // Method for index page, returns the 
        // page view
        public ActionResult Index()
        {
            return View();
        }

        // Method for the about page, displays a title,
        // returns the page view
        public ActionResult About()
        {
            ViewBag.Message = "How to use this website.";

            return View();
        }

        // Method for contact page, returns the 
        // page view
        public ActionResult Contact()
        {
            return View();
        }
    }
}