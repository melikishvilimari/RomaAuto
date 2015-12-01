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

namespace RomaAuto.Controllers
{
    [LoginFilter]
    public class AllOrdersListController : Controller
    {
        RomaDBEntities db = new RomaDBEntities();
        public ActionResult Index(string operatorLastname = "", string sellerLastname = "",string openDate = "",string closeDate = "",int page = 1)
        {
            ViewBag.OperatorLastname = operatorLastname;
            ViewBag.SellerLastname = sellerLastname;
            ViewBag.OpenDate = openDate;
            ViewBag.CloseDate = closeDate;
            ViewBag.Page = page;
            var result = db.Orders
                .ToList()
                .Where(e => (openDate == "" || e.OpenDate.ToString("MM/dd/yyyy").Contains(openDate))
                    && (closeDate == "" || (e.CloseDate != null && e.CloseDate.Value.ToString("MM/dd/yyyy").Contains(closeDate)))
                    && (e.Operator.Lastname.Contains(operatorLastname) || (e.Operator1 != null && e.Operator1.Lastname.Contains(operatorLastname)))
                    && (sellerLastname == "" || e.Seller_Order.Any(m => m.Saler.Lastname.Contains(sellerLastname))))
                                               .ToList();
            
            return View(result.ToPagedList(page, 10));
        }

        public FileResult Excel()
        {
            // If using Professional version, put your serial key below.
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");

            ExcelFile ef = new ExcelFile();
            ExcelWorksheet ws = ef.Worksheets.Add("Hello World");

            ws.Cells[1, 1].Value = "მწარმოებელი";
            ws.Cells[1, 2].Value = "მოდელი";
            ws.Cells[1, 3].Value = "კატეგორია";
            ws.Cells[1, 4].Value = "წელი";
            ws.Cells[1, 5].Value = "ტრანსმისია";
            ws.Cells[1, 6].Value = "მდებარეობა";
            ws.Cells[1, 7].Value = "საკონტაქტო ნომერი";
            ws.Cells[1, 8].Value = "ნაწილი";
            ws.Cells[1, 9].Value = "შენიშვნა";
            ws.Cells[1, 10].Value = "მომწოდებლები";
            ws.Cells[1, 11].Value = "შეკვეთა მიიღო";
            ws.Cells[1, 12].Value = "შეკვეთა დახურა";
            ws.Cells[1, 13].Value = "შეკვეთის თარიღი";
            ws.Cells[1, 14].Value = "დახურვის თარიღი";

            ws.Columns[1].Width = 30 * 256;
            ws.Columns[2].Width = 30 * 256;
            ws.Columns[3].Width = 30 * 256;
            ws.Columns[4].Width = 30 * 256;
            ws.Columns[5].Width = 30 * 256;
            ws.Columns[6].Width = 30 * 256;
            ws.Columns[7].Width = 30 * 256;
            ws.Columns[8].Width = 30 * 256;
            ws.Columns[9].Width = 30 * 256;
            ws.Columns[10].Width = 30 * 256;
            ws.Columns[11].Width = 30 * 256;
            ws.Columns[12].Width = 30 * 256;
            ws.Columns[13].Width = 30 * 256;
            ws.Columns[14].Width = 30 * 256;

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
            }
            //// Using UNICODE string.
            //ws.Cells[1, 1].Value = new string(new char[] { '\u0417', '\u0434', '\u0440', '\u0430', '\u0432', '\u0441', '\u0442', '\u0432', '\u0443', '\u0439', '\u0442', '\u0435' });

            //ws.Cells[2, 0].Value = "Chinese:";
            //// Using UNICODE string.
            //ws.Cells[2, 1].Value = new string(new char[] { '\u4f60', '\u597d' });

            //ws.Cells[4, 0].Value = "In order to see Russian and Chinese characters you need to have appropriate fonts on your PC.";
            //ws.Cells.GetSubrangeAbsolute(4, 0, 4, 7).Merged = true;


            string filename = "awdawdawdawd.xlsx";
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
