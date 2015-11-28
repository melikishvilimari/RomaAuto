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
        public ActionResult Index(string operatorLastname = "", string sellerLastname = "",string openDate = "",string closeDate = "",int page = 1)
        {
            ViewBag.OperatorLastname = operatorLastname;
            ViewBag.SellerLastname = sellerLastname;
            ViewBag.OpenDate = openDate;
            ViewBag.CloseDate = closeDate;
            var result = db.Orders
                .ToList()
                .Where(e => e.OpenDate.ToString("dd/MM/yyyy").Contains(openDate) 
                    && (e.CloseDate == null || e.CloseDate.Value.ToString("dd/MM/yyyy").Contains(closeDate)) 
                    && (e.Operator.Lastname.Contains(operatorLastname) || e.Operator1.Lastname.Contains(operatorLastname))
                    && (sellerLastname == "" || e.Seller_Order.Any(m => m.Saler.Lastname.Contains(sellerLastname))))
                                               .ToList();
            return View(result.ToPagedList(page, 10));
        }
    }
}
