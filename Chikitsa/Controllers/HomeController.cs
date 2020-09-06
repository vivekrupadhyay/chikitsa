using Chikitsa.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chikitsa.Controllers
{
    public class HomeController : Controller
    {
       
        // GET: Home
        public Layout layoutModel = new Layout();
        public ActionResult Index()
        {
            HomeViewModel objModel = new HomeViewModel();
            return View(objModel);
        }

    }
}