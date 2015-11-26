using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RomaAuto.Models;

namespace RomaAuto.Controllers
{
    public class AllOrdersListController : Controller
    {
        private RomaDBEntities db = new RomaDBEntities();

        // GET: Orders1
        public ActionResult Index()
        {
            AllOrder allOrders = new AllOrder();
            allOrders.orders = db.Orders.ToList();

            allOrders.operators = db.Operators.ToList();
            allOrders.sellers = db.Salers.ToList();
            allOrders.salerOrders = db.Seller_Order.ToList();

            return View(allOrders);
        }

      

     
     
    }
}
