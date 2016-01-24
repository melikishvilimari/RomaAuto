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
    public class TestSellerController : Controller
    {
        private GreenBox_GreenBoxEntities db = new GreenBox_GreenBoxEntities();

        // GET: TestSeller
        public ActionResult Index()
        {
            var salers = db.Salers.Include(s => s.City);
            return View(salers.ToList());
        }

        // GET: TestSeller/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Saler saler = db.Salers.Find(id);
            if (saler == null)
            {
                return HttpNotFound();
            }
            return View(saler);
        }

        // GET: TestSeller/Create
        public ActionResult Create()
        {
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "Name");
            return View();
        }

        // POST: TestSeller/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SalerID,Name,Lastname,Address,Phone,CityID,IsActive")] Saler saler)
        {
            if (ModelState.IsValid)
            {
                db.Salers.Add(saler);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CityID = new SelectList(db.Cities, "CityID", "Name", saler.CityID);
            return View(saler);
        }

        // GET: TestSeller/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Saler saler = db.Salers.Find(id);
            if (saler == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "Name", saler.CityID);
            return View(saler);
        }

        // POST: TestSeller/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SalerID,Name,Lastname,Address,Phone,CityID,IsActive")] Saler saler)
        {
            if (ModelState.IsValid)
            {
                db.Entry(saler).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "Name", saler.CityID);
            return View(saler);
        }

        // GET: TestSeller/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Saler saler = db.Salers.Find(id);
            if (saler == null)
            {
                return HttpNotFound();
            }
            return View(saler);
        }

        // POST: TestSeller/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Saler saler = db.Salers.Find(id);
            db.Salers.Remove(saler);
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
    }
}
