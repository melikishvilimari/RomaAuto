using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RomaAuto.Models;
using RomaAuto.Filters;
using PagedList;
using PagedList.Mvc;

namespace RomaAuto.Controllers
{
    [LoginFilter]
    public class OrderListController : Controller
    {
         GreenBox_GreenBoxEntities _db = new GreenBox_GreenBoxEntities();
        // GET: OrderList
        public ActionResult Index(int page = 1)
        { 
            var orders = _db.Orders.Where(e => e.IsClosed == false).ToList();
            return View(orders.ToPagedList(page, 10));
        }

        // GET: OrderList/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OrderList/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OrderList/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: OrderList/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OrderList/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: OrderList/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OrderList/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
