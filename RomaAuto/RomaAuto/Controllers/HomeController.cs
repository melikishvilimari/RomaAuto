using RomaAuto.Filters;
using RomaAuto.Helpers;
using RomaAuto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GemBox.Spreadsheet;

namespace RomaAuto.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CountryList()
        {
            //var manufacturers = _db.Manufacturers.ToList();

            //if (HttpContext.Request.IsAjaxRequest())
            //{
            //    return Json(new SelectList(
            //                manufacturers,
            //                "",
            //                ""), JsonRequestBehavior.AllowGet
            //                );
            //}

            return View();
        }

        //public ActionResult Contact()
        //{
        //    SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");

        //    ExcelFile ef = ExcelFile.Load(Server.MapPath("~/Excel/12-03-2015-Orders.xlsx"));
        //    string content = "";
        //    ExcelWorksheet ws = ef.Worksheets[0];

        //    for (int i = 0; ; i++)
        //    {
        //        if (ws.Cells[0, i].Value == null) break;
        //        var name = ws.Cells[0, i].Value.ToString();
        //        var result = _db.Manufacturers.FirstOrDefault(item => item.Name == name).ManufacturerID;
        //        content += "id = " + result + "\t";
        //        for (int j = 1; ; j++)
        //        {
        //            if (ws.Cells[j, i].Value == null) break;
        //            var model = ws.Cells[j, i].Value.ToString().Trim();
                    
        //            if (!_db.CarModels.Any(e => e.ManufacturerID == result && e.Name == model))
        //            {
        //                content += model + ":";
        //                _db.CarModels.Add(new CarModel() { Name = model, ManufacturerID = result });
        //                _db.SaveChanges();
        //            }
        //        }
                
        //    }

        //    return Content(content);
        //}
    }
}