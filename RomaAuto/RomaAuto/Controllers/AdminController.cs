using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RomaAuto.Models;
using RomaAuto.Filters;
using System.Net;
using System.Data.Entity;
using PagedList;

namespace RomaAuto.Controllers
{
    [LoginFilter]
    [AccessFilter]
    public class AdminController : Controller
    {
        RomaDBEntities _db = new RomaDBEntities();
        // GET: Admin
        public ActionResult Index(string name = "", string lastname = "", int page = 1)
        {
            ViewBag.Name = name;
            ViewBag.Lastname = lastname;
            var result = _db.Salers.Where(e => e.Name.Contains(name) && e.Lastname.Contains(lastname)).ToList();
            return View(result.ToPagedList(page, 10));
        }

        // GET: Admin/Details/5
        public ActionResult Details(int id)
        {
            var model = _db.Salers.FirstOrDefault(e => e.SalerID == id);
            ViewBag.SalerParts = _db.SalersParts.Where(item => item.SalerID == id).ToList();
            return View(model);
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            //ViewBag.CarModelID = new SelectList(_db.CarModels.Where(item => item.ManufacturerID == _db.Manufacturers.FirstOrDefault().ManufacturerID), "ModelID", "Name"); ;
            //ViewBag.ManufacturerID = new SelectList(_db.Manufacturers, "ManufacturerID", "Name"); ;
            //ViewBag.CarCategoryID = new SelectList(_db.CarCategories, "CarCategoryID", "Name");
            //ViewBag.TransmisionID = new SelectList(_db.Transmisions, "TransmisionID", "Name");
            //ViewBag.CityID = new SelectList(_db.Cities, "CityID", "Name");
            ViewBag.CityID = new SelectList(_db.Cities, "CityID", "Name");
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SalerID,Name,Lastname,Address,Phone,CityID,IsActive")] Saler saler)
        {
            if (ModelState.IsValid)
            {
                _db.Salers.Add(saler);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CityID = new SelectList(_db.Cities, "CityID", "Name", saler.CityID);
            //var user = (MainUser)Session["user"];
            //if (ModelState.IsValid && user != null)
            //{
            //    //order.OpenDate = DateTime.Now;
            //   // order.CloseOperatorID = user.Id;
            //    Saler saler = new Saler();
            //    saler.Name = model.Name;
            //    saler.Lastname = model.Lastname;
            //    saler.Phone = model.Phone;
            //    saler.CityID = model.CityID;
            //    saler.Address = model.Address;
            //    saler.IsActive = true;
            //    _db.Salers.Add(saler);
            //    _db.SaveChanges();
            //    SalersPart sp = new SalersPart();
            //    //sp.SalerID = model.SellerID;
            //    sp.SalerID = _db.Salers.Where(e => e.Name == model.Name && e.Lastname == model.Lastname && e.Phone == model.Phone).FirstOrDefault().SalerID;
            //    sp.ManufacturerID = model.ManufacturerID;
            //    sp.Note = model.Note;
            //    sp.CarModelsID = model.CarCategoryID;
            //    sp.ManufacturerID = model.ManufacturerID;
            //    sp.Helped = 0;
            //    sp.DontHelped = 0;
            //    sp.CarTransmissionID = 1;


                
            //    _db.SalersParts.Add(sp);
            //    _db.SaveChanges();
            //    return RedirectToAction("Create");
            //}

            //ViewBag.CarModelID = new SelectList(_db.CarModels.Where(item => item.ManufacturerID == model.ManufacturerID), "ModelID", "Name", model.ManufacturerID);
            //ViewBag.ManufacturerID = new SelectList(_db.Manufacturers, "ManufacturerID", "Name", model.ManufacturerID);
            //ViewBag.CarCategoryID = new SelectList(_db.CarCategories, "CarCategoryID", "Name", model.CarCategoryID);
            //ViewBag.TransmisonID = new SelectList(_db.Transmisions, "TransmisionID", "Name", model.TransmisionID);
            //ViewBag.CityID = new SelectList(_db.Cities, "CityID", "Name", model.CityID);
            return View(saler);
        }
        public ActionResult FilterCarModel(int id)
        {
            var result = from item in _db.CarModels.Where(item => item.ModelID == id)
                         select new { item.ModelID, item.Name };
            return Json(result.ToArray(), JsonRequestBehavior.AllowGet);
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
        // GET: Admin/Edit/5
        public ActionResult EditSaler(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Saler saler = _db.Salers.Find(id);
            if (saler == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityID = new SelectList(_db.Cities, "CityID", "Name", saler.CityID);
            return View(saler);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult EditSaler([Bind(Include = "SalerID,Name,Lastname,Address,Phone,CityID,IsActive")] Saler saler)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(saler).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityID = new SelectList(_db.Cities, "CityID", "Name", saler.CityID);
            return View(saler);
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Saler saler = _db.Salers.Find(id);
            if (saler == null)
            {
                return HttpNotFound();
            }
            return View(saler);





            //var result = _db.Salers.FirstOrDefault(s => s.SalerID == id);
            //result.IsActive = false;
            //_db.SaveChanges();
            //return RedirectToAction("Index", "Admin");
            //return RedirectToAction("Index");

           // return View();
        }

        // POST: Admin/Delete/5
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {

            Saler saler = _db.Salers.Find(id);
            saler.IsActive = false;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }


  

      




    }
}
