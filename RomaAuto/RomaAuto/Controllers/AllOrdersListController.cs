using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RomaAuto.Models;
using PagedList;
using PagedList.Mvc;

namespace RomaAuto.Controllers
{
    public class AllOrdersListController : Controller
    {
        RomaDBEntities db = new RomaDBEntities();
        public ActionResult Index(int page = 1)
        {
            var result = db.Orders.ToList();
            return View(result.ToPagedList(page, 10));
        }
    }
}
