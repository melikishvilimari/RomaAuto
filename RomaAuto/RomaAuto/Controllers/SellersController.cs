﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RomaAuto.Models;
namespace RomaAuto.Controllers
{
    public class SellersController : Controller
    {
        RomaDBEntities _db = new RomaDBEntities();
        // GET: Sellers
        public ActionResult Index(int OrderID, int? ManufacturerId = null, int? CarModelId = null, int? CarCategoryId = null, string part = "")
        {
            ViewBag.OrderID = OrderID;
            ViewBag.Part = part;
            ViewBag.ManufacturerId = ManufacturerId;
            ViewBag.CarModelId = CarModelId;
            ViewBag.CarCategoryId = CarCategoryId;
            // ყველა როცა არის მონიშნული როგორ წამოიღოს ინფორმაცია ბაზიდან
            var sellers = _db.Salers.ToList();

            var sellersList = (from item in _db.Salers
                               let sellerparts = item.SalersParts.Where(
                                 s => ((s.CarModelsID == CarModelId || CarModelId == null) &&
                                     (s.CarCategoryID == CarCategoryId || CarCategoryId == null) &&
                                     (s.ManufacturerID == ManufacturerId || ManufacturerId == null))
                               ).ToList().FirstOrDefault()
                               where sellerparts != null && sellerparts.Note.Contains(part)
                               select new OrdersList()
                               {
                                   SalerID = item.SalerID,
                                   Name = item.Name,
                                   Lastname = item.Lastname,
                                   Phone = item.Phone,
                                   SellerPartID = sellerparts.SalersPartID,
                                   Address = item.Address,
                                   Helped = sellerparts.Helped,
                                   NotHelped = sellerparts.DontHelped,
                                   Note = sellerparts.Note,
                                   City = item.City.Name
                               }).ToList<OrdersList>();
            return View(sellersList);
        }

        public ActionResult NotHelped(int id)
        {
            var seller = _db.SalersParts.Where(e => e.SalersPartID == id).FirstOrDefault();
            seller.DontHelped++;
            _db.SaveChanges();
            return Json(new { success = true });
        }
        public ActionResult Helped(int id)
        {
            var seller = _db.SalersParts.Where(e => e.SalersPartID == id).FirstOrDefault();
            seller.Helped++;
            _db.SaveChanges();
            return Json(new { success = true });
        }

        
    }
}