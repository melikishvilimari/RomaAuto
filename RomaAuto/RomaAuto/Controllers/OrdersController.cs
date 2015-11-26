using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RomaAuto.Models;
using RomaAuto.Filters;

namespace RomaAuto.Controllers
{
    [LoginFilter]
    public class OrdersController : Controller
    {
        private RomaDBEntities db = new RomaDBEntities();

        // GET: Orders
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.CarModel).Include(o => o.Manufacturer);
            return View(orders.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {

            var manufacturers = db.Manufacturers.ToList();
            manufacturers.Insert(0, new Manufacturer() { ManufacturerID = 0, Name = "ყველა" });
            var carModels = db.CarModels.ToList();
            carModels.Insert(0, new CarModel() { ModelID = 0, Name = "ყველა" });
            var carCategorys = db.CarCategories.ToList();
            carCategorys.Insert(0, new CarCategory() { CarCategoryID = 0, Name = "ყველა" });
            var transmisions = db.Transmisions.ToList();
            transmisions.Insert(0, new Transmision() { TransmisionID = 0, Name = "ყველა" });
            var cities = db.Cities.ToList();
            cities.Insert(0, new City() { CityID = 0, Name = "ყველა" });

            ViewBag.CarModelID = new SelectList(carModels, "ModelID", "Name");
            ViewBag.ManufacturerID = new SelectList(manufacturers, "ManufacturerID", "Name");
            ViewBag.CarCategoryID = new SelectList(carCategorys, "CarCategoryID", "Name");
            ViewBag.TransmisionID = new SelectList(transmisions, "TransmisionID", "Name");
            ViewBag.CityID = new SelectList(cities, "CityID", "Name");

            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderID,ManufacturerID,CarModelID,CarCategoryID,OutputDate,TransmisionID,CityID,Phone,Part,Note")] Order order)
        {
            var user = (MainUser)Session["user"];
            if (ModelState.IsValid && user != null)
            {
                if (order.ManufacturerID == 0)
                {
                    order.ManufacturerID = null;
                }
                if (order.CarCategoryID == 0)
                {
                    order.CarCategoryID = null;
                }
                if (order.TransmisionID == 0)
                {
                    order.TransmisionID = null;
                }
                if (order.CarModelID == 0)
                {
                    order.CarModelID = null;
                }
                if (order.CityID == 0)
                {
                    order.CityID = null;
                }
                order.OpenDate = DateTime.Now;
                order.OpenOperatorID = user.Id;
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Create");
            }

            ViewBag.CarModelID = new SelectList(db.CarModels.Where(item => item.ManufacturerID == order.ManufacturerID), "ModelID", "Name", order.ManufacturerID);
            ViewBag.ManufacturerID = new SelectList(db.Manufacturers, "ManufacturerID", "Name", order.ManufacturerID);
            ViewBag.CarCategoryID = new SelectList(db.CarCategories, "CarCategoryID", "Name", order.CarCategoryID);
            ViewBag.TransmisonID = new SelectList(db.Transmisions, "TransmisionID", "Name", order.TransmisionID);
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "Name", order.CityID);
            return View(order);
        }

        public ActionResult FilterCarModel(int id)
        {
            var result =  (from item in db.CarModels.Where(item => item.ModelID == id || id == 0)
                          select new { item.ModelID, item.Name }).ToList();
            result.Insert(0, new { ModelID = 0, Name = "ყველა" });
            return Json(result.ToArray(), JsonRequestBehavior.AllowGet);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.CarModelID = new SelectList(db.CarModels, "ModelID", "Name", order.CarModelID);
            ViewBag.ManufacturerID = new SelectList(db.Manufacturers, "ManufacturerID", "Name", order.ManufacturerID);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,ManufacturerID,CarModelID,OpenDate,Category,BirthDate,Transmision,Address,Phone,Part,Note,CloseDate,OpenOperatorID,CloseOperatorID")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CarModelID = new SelectList(db.CarModels, "ModelID", "Name", order.CarModelID);
            ViewBag.ManufacturerID = new SelectList(db.Manufacturers, "ManufacturerID", "Name", order.ManufacturerID);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult CloseOrder(int OrderID, int[] SellerIDs)
        {
            var user = (MainUser)Session["user"];
            var order = db.Orders.FirstOrDefault(item => item.OrderID == OrderID);
            order.CloseDate = DateTime.Now;
            order.CloseOperatorID = user.Id;
            order.IsClosed = true;

            foreach (var seller in SellerIDs)
            {
                db.Seller_Order.Add(new Seller_Order() { OrderID = OrderID, SellerID = seller });
            }

            db.SaveChanges();
            return RedirectToAction("Index", "OrderList");
        }
    }
}
