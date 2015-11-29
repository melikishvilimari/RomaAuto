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
    [AccessFilter]
    public class SalersPartsController : Controller
    {
        private RomaDBEntities db = new RomaDBEntities();

        // GET: SalersParts
        public ActionResult Index()
        {
            var salersParts = db.SalersParts.Include(s => s.CarModel).Include(s => s.CarModel1).Include(s => s.Manufacturer).Include(s => s.Saler).Include(s => s.CarCategory).Include(s => s.Transmision);
            return View(salersParts.ToList());
        }

        // GET: SalersParts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalersPart salersPart = db.SalersParts.Find(id);
            if (salersPart == null)
            {
                return HttpNotFound();
            }
            return View(salersPart);
        }

        // GET: SalersParts/Create
        public ActionResult Create(int salerID)
        {
            ViewBag.SalerID = salerID;
            var manufacturers = db.Manufacturers.ToList();
            var carModels = db.CarModels.Where(item => item.ManufacturerID == db.Manufacturers.FirstOrDefault().ManufacturerID).ToList();
            ViewBag.CarModelsID = new SelectList(carModels, "ModelID", "Name");
            ViewBag.ManufacturerID = new SelectList(manufacturers, "ManufacturerID", "Name");
            ViewBag.CarCategoryID = new SelectList(db.CarCategories, "CarCategoryID", "Name");
            ViewBag.CarTransmissionID = new SelectList(db.Transmisions, "TransmisionID", "Name");
            return View();
        }

        // POST: SalersParts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SalersPartID,Helped,DontHelped,SalerID,CarModelsID,ManufacturerID,CarCategoryID,CarTransmissionID,Note")] SalersPart salersPart)
        {
            if (ModelState.IsValid)
            {
                salersPart.Helped = 0;
                salersPart.DontHelped = 0;
                db.SalersParts.Add(salersPart);
                db.SaveChanges();
                return RedirectToAction("Details", "Admin", new { id = salersPart.SalerID });
            }

            ViewBag.CarModelsID = new SelectList(db.CarModels.Where(item => item.ManufacturerID == salersPart.ManufacturerID), "ModelID", "Name", salersPart.ManufacturerID);
            ViewBag.ManufacturerID = new SelectList(db.Manufacturers, "ManufacturerID", "Name", salersPart.ManufacturerID);
            ViewBag.CarCategoryID = new SelectList(db.CarCategories, "CarCategoryID", "Name", salersPart.CarCategoryID);
            ViewBag.CarTransmissionID = new SelectList(db.Transmisions, "TransmisionID", "Name", salersPart.CarTransmissionID);
            return View(salersPart);
        }

        // GET: SalersParts/Edit/5
        public ActionResult Edit(int? id, int salerId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalersPart salersPart = db.SalersParts.Find(id);
            if (salersPart == null)
            {
                return HttpNotFound();
            }
            ViewBag.SalerID = salerId;
            ViewBag.CarModelsID = new SelectList(db.CarModels.Where(item => item.ManufacturerID == salersPart.ManufacturerID), "ModelID", "Name", salersPart.CarModelsID);
            ViewBag.ManufacturerID = new SelectList(db.Manufacturers, "ManufacturerID", "Name", salersPart.ManufacturerID);
            ViewBag.CarCategoryID = new SelectList(db.CarCategories, "CarCategoryID", "Name", salersPart.CarCategoryID);
            ViewBag.CarTransmissionID = new SelectList(db.Transmisions, "TransmisionID", "Name", salersPart.CarTransmissionID);
            return View(salersPart);
        }

        // POST: SalersParts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SalersPartID,Helped,DontHelped,SalerID,CarModelsID,ManufacturerID,CarCategoryID,CarTransmissionID,Note")] SalersPart salersPart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(salersPart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Admin", new { id = salersPart.SalerID });
            }
            ViewBag.CarModelsID = new SelectList(db.CarModels, "ModelID", "Name", salersPart.CarModelsID);
            ViewBag.CarModelsID = new SelectList(db.CarModels, "ModelID", "Name", salersPart.CarModelsID);
            ViewBag.ManufacturerID = new SelectList(db.Manufacturers, "ManufacturerID", "Name", salersPart.ManufacturerID);
            ViewBag.SalerID = new SelectList(db.Salers, "SalerID", "Name", salersPart.SalerID);
            ViewBag.CarCategoryID = new SelectList(db.CarCategories, "CarCategoryID", "Name", salersPart.CarCategoryID);
            ViewBag.CarTransmissionID = new SelectList(db.Transmisions, "TransmisionID", "Name", salersPart.CarTransmissionID);
            return View(salersPart);
        }

        // GET: SalersParts/Delete/5
        public ActionResult Delete(int? id, int salerId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.SalerID = salerId;
            SalersPart salersPart = db.SalersParts.Find(id);
            if (salersPart == null)
            {
                return HttpNotFound();
            }
            return View(salersPart);
        }

        // POST: SalersParts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int SalerID)
        {
            SalersPart salersPart = db.SalersParts.Find(id);
            db.SalersParts.Remove(salersPart);
            db.SaveChanges();
            return RedirectToAction("Details", "Admin", new { id = SalerID });
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
