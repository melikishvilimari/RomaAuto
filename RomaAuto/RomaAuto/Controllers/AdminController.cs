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
using GemBox.Spreadsheet;
using System.Drawing;

namespace RomaAuto.Controllers
{
    [LoginFilter]
    [AccessFilter]
    public class AdminController : Controller
    {
        RomaDBEntities _db = new RomaDBEntities();
        // GET: Admin
        public ActionResult Index(string name = "", string lastname = "", string phone = "", int page = 1)
        {
            ViewBag.Name = name;
            ViewBag.Lastname = lastname;
            ViewBag.Phone = phone;
            var result = _db.Salers
                .Where(e => e.Name.Contains(name) && e.Lastname.Contains(lastname) && e.Phone.Contains(phone))
                .OrderByDescending(e => e.SalerID)
                .ToList();
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

        public FileResult Excel()
        {
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");

            ExcelFile ef = new ExcelFile();
            ExcelWorksheet ws = ef.Worksheets.Add("მომწოდებლები");

            var cellNames = new string[6]
            {
                "სახელი",
                "გვარი",
                "ქალაქი",
                "მისამართი",
                "ტელეფონი",
                "აქტიური"
            };

            for (int i = 1; i < 7; i++)
            {
                ws.Cells[1, i].Value = cellNames[i - 1];
                ws.Cells[1, i].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                ws.Cells[1, i].Style.Font.Weight = ExcelFont.BoldWeight;
                ws.Cells[1, i].Style.Font.Color = Color.White;
                ws.Cells[1, i].Style.FillPattern.SetSolid(Color.LightGreen);
            }

            for (int i = 1; i < 7; i++)
            {
                ws.Columns[i].Width = 30 * 256;
            }

            var result = _db.Salers.OrderByDescending(e => e.SalerID).ToList();
            for (int i = 0; i < result.Count(); i++)
            {
                ws.Cells[2 + i, 1].Value = result[i].Name;
                ws.Cells[2 + i, 2].Value = result[i].Lastname;
                ws.Cells[2 + i, 3].Value = result[i].City.Name;
                ws.Cells[2 + i, 4].Value = result[i].Address;
                ws.Cells[2 + i, 5].Value = result[i].Phone;
                ws.Cells[2 + i, 6].Value = result[i].IsActive;
            }

            string filename = DateTime.Now.ToString("MM-dd-yyyy") + "-Sellers.xlsx";
            string path = Server.MapPath("~/Excel/" + filename);
            ef.Save(path);

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = filename,
                Inline = true,
            };
            byte[] filedata = System.IO.File.ReadAllBytes(path);
            string contentType = MimeMapping.GetMimeMapping(path);
            Response.AppendHeader("Content-Disposition", cd.ToString());

            return File(filedata, contentType);
        }
    }
}
