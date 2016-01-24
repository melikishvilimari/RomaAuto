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
using RomaAuto.Filters;
using GemBox.Spreadsheet;
using System.Drawing;

namespace RomaAuto.Controllers
{
    [LoginFilter]
    public class AllOrdersListController : Controller
    {
        GreenBox_GreenBoxEntities db = new GreenBox_GreenBoxEntities();
        public ActionResult Index(string operatorLastname = "", string sellerLastname = "", string openDateStart = "", string openDateFinish = "", string closeDateStart = "", string closeDateFinish = "", int page = 1)
        {
            //e.OpenDate.ToString("MM/dd/yyyy").Contains(openDate)
            ViewBag.OperatorLastname = operatorLastname;
            ViewBag.SellerLastname = sellerLastname;
            ViewBag.OpenDateStart = openDateStart;
            ViewBag.OpenDateFinish = openDateFinish;
            ViewBag.CloseDateStart = closeDateStart;
            ViewBag.CloseDateFinish = closeDateFinish;
            ViewBag.Page = page;
            var result = db.Orders
                .ToList()
                .Where(e => (openDateStart == "" || openDateFinish == "" ||
                (e.OpenDate >= Convert.ToDateTime(openDateStart) && e.OpenDate <= Convert.ToDateTime(openDateFinish).AddDays(1)))
                    && 
                    (closeDateStart == "" || closeDateFinish == "" || (e.CloseDate != null &&
                    Convert.ToDateTime((e.CloseDate.Value.ToShortDateString())) >= Convert.ToDateTime(closeDateStart) && Convert.ToDateTime((e.CloseDate.Value.ToShortDateString())) <= Convert.ToDateTime(closeDateFinish)))
                    && (e.Operator.Lastname.Contains(operatorLastname) || (e.Operator1 != null && e.Operator1.Lastname.Contains(operatorLastname)))
                    && (sellerLastname == "" || e.Seller_Order.Any(m => m.Saler.Lastname.Contains(sellerLastname))))
                                               .ToList();
            //foreach (var e in result)
            //{
            //    DateTime a = Convert.ToDateTime(closeDateFinish).AddDays(1);
            //    DateTime aawd = e.CloseDate.Value;
            //    DateTime awdawdawd = Convert.ToDateTime((e.CloseDate.Value.ToShortDateString()));
            //    if (e.CloseDate.Value >= Convert.ToDateTime(closeDateStart) && e.CloseDate.Value <= Convert.ToDateTime(closeDateFinish).AddDays(1))
            //    {
            //        //awdaw
            //    }
            //}
                //                                var result = db.Orders
                //.ToList()
                //.Where(e => (openDateStart == "" || openDateFinish == "" ||
                //(e.OpenDate >= Convert.ToDateTime(openDateStart) && e.OpenDate <= Convert.ToDateTime(openDateFinish).AddDays(1)))
                //    && (closeDateStart == "" || closeDateFinish == "" || (e.CloseDate != null &&
                //    e.CloseDate.Value >= Convert.ToDateTime(closeDateStart) && e.CloseDate.Value <= Convert.ToDateTime(closeDateFinish).AddDays(1)))
                //    && (e.Operator.Lastname.Contains(operatorLastname) || (e.Operator1 != null && e.Operator1.Lastname.Contains(operatorLastname)))
                //    && (sellerLastname == "" || e.Seller_Order.Any(m => m.Saler.Lastname.Contains(sellerLastname))))
                //                               .ToList();

            return View(result.ToPagedList(page, 10));
        }

        public FileResult Excel()
        {
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            ExcelFile ef = new ExcelFile();
            ExcelWorksheet ws = ef.Worksheets.Add("შეკვეთები");

            var cellNames = new string[15]
            {
                "მწარმოებელი",
                "მოდელი",
                "კატეგორია",
                "წელი",
                "ტრანსმისია",
                "მდებარეობა",
                "საკონტაქტო ნომერი",
                "ნაწილი",
                "შენიშვნა",
                "მომწოდებლები",
                "შეკვეთა მიიღო",
                "შეკვეთა დახურა",
                "შეკვეთის თარიღი",
                "დახურვის თარიღი",
                "კუბატურა"
            };

            for (int i = 1; i < 16; i++)
            {
                ws.Cells[1, i].Value = cellNames[i - 1];
                ws.Cells[1, i].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                ws.Cells[1, i].Style.Font.Weight = ExcelFont.BoldWeight;
                ws.Cells[1, i].Style.Font.Color = Color.White;
                ws.Cells[1, i].Style.FillPattern.SetSolid(Color.LightGreen);
            }

            for(int i = 1; i < 16; i++)
            {
                ws.Columns[i].Width = 30 * 256;
            }

            var result = db.Orders.OrderByDescending(e => e.OrderID).ToList();
            string noInfo = "";
            for (int i = 0; i < result.Count(); i++)
            {
                ws.Cells[2 + i, 1].Value = result[i].ManufacturerID == null ? noInfo : result[i].Manufacturer.Name;
                ws.Cells[2 + i, 2].Value = result[i].CarModelID == null ? noInfo : result[i].CarModel.Name;
                ws.Cells[2 + i, 3].Value = result[i].CarCategoryID == null ? noInfo : result[i].CarCategory.Name;
                ws.Cells[2 + i, 4].Value = result[i].OutputDate;
                ws.Cells[2 + i, 5].Value = result[i].TransmisionID == null ? noInfo : result[i].Transmision.Name;
                ws.Cells[2 + i, 6].Value = result[i].CityID == null ? noInfo : result[i].City.Name;
                ws.Cells[2 + i, 7].Value = result[i].Phone;
                ws.Cells[2 + i, 8].Value = result[i].Part;
                ws.Cells[2 + i, 9].Value = result[i].Note;
                var sellers = "";
                foreach (var item in result[i].Seller_Order)
                {
                    sellers += item.Saler.Lastname + "\n";
                }
                ws.Cells[2 + i, 10].Value = sellers;
                string creator = result[i].Operator == null ? noInfo : result[i].Operator.Name;
                creator += result[i].Operator == null ? "" : (" " + result[i].Operator.Lastname);
                ws.Cells[2 + i, 11].Value = creator;
                string closer = result[i].Operator1 == null ? noInfo : result[i].Operator1.Name;
                closer += result[i].Operator1 == null ? "" : (" " + result[i].Operator1.Lastname);
                ws.Cells[2 + i, 12].Value = closer;
                ws.Cells[2 + i, 13].Value = result[i].OpenDate.ToShortDateString();
                ws.Cells[2 + i, 14].Value = result[i].CloseDate == null ? "" : result[i].CloseDate.Value.ToShortDateString();
                ws.Cells[2 + i, 15].Value = result[i].Kubatura;
            }

            string filename = DateTime.Now.ToString("MM-dd-yyyy") + "-Orders.xlsx";
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
