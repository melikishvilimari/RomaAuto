using RomaAuto.Filters;
using RomaAuto.Helpers;
using RomaAuto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RomaAuto.Controllers
{
    [LoginFilter]
    public class HomeController : Controller
    {
        RomaDBEntities _db = new RomaDBEntities();
        public ActionResult Index(string manufacturer = "", string carModel = "")
        {
            var user = LoginHelper.CurrentUser();
            if ( user != null)
            {
                ViewBag.Message = user.Id + " " + user.Name + " " + user.LastName;
            }
            ViewBag.Manufacturer = new SelectList(_db.Manufacturers, "ManufacturerID", "Name");
            ViewBag.CarModelID = new SelectList(_db.CarModels, "ModelID", "Name").ToList();
            return View();
        }

        public ActionResult CountryList()
        {
            var manufacturers = _db.Manufacturers.ToList();

            if (HttpContext.Request.IsAjaxRequest())
            {
                return Json(new SelectList(
                            manufacturers,
                            "",
                            ""), JsonRequestBehavior.AllowGet
                            );
            }

            return View(manufacturers);
        }

        public ActionResult StateList(string CountryCode)
        {
           // IQueryable states = State.GetStates().Where(x => x.CountryCode == CountryCode);

            //if (HttpContext.Request.IsAjaxRequest())
            //    return Json(new SelectList(
            //                    states,
            //                    "StateID",
            //                    "StateName"), JsonRequestBehavior.AllowGet
            //                );

            return View();
        }



        [AccessFilter]
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
    }
}