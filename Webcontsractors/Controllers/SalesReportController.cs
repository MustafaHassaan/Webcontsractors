using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using Repo.Unitofwork;
using System.Linq;
using Webcontsractors.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Webcontsractors.Controllers
{
    public class SalesReportController : Rolecontroller
    {
        IUnitofwork _IUW;
        public SalesReportController(IUnitofwork IUW)
        {
            _IUW = IUW;
        }
        public IActionResult Index(DateTime? fromDate, DateTime? toDate, int[] projectId, int? ownerId)
        {
            var Prolist = _IUW.Projects.GetAll();
            var Prtlist = _IUW.Partners.GetAll();
            ViewBag.Pro = Prolist.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Projectname?.ToString()
            }).ToList();
            ViewBag.PartList = Prtlist.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Partnername?.ToString()
            }).ToList();
            // جلب البيانات
            var Protlist = _IUW.TblTransactions.GetAllwithmaster("Pro")
                .Select(x => new Transactionmodel
                {
                    Projectname = x.Pro?.Projectname,
                    Detailes = x.Detailes,
                    Tdate = x.Tdate.HasValue ? x.Tdate.Value.ToString("yyyy-MM-dd") : null, // تحويل التاريخ إلى نص
                    Creditor = x.Creditor,
                    Prtid = x.Pro?.Prtid,
                    Proid = x.Proid,
                    Vatamount = Math.Round((x.Creditor ?? 0) - ((x.Creditor ?? 0) / 1.15m), 2),
                    Note = x.Note,
                }).Where(x => x.Creditor != 0).AsQueryable();
            // تطبيق الفلاتر بناءً على القيم المدخلة
            if (fromDate.HasValue)
            {
                var fromDateString = fromDate.Value.ToString("yyyy-MM-dd"); // تنسيق التاريخ إلى نص
                ViewBag.DTF = fromDate.Value.ToString("yyyy-MM-dd");
                Protlist = Protlist.Where(x => x.Tdate.CompareTo(fromDateString) >= 0); // مقارنة النصوص
            }
            if (toDate.HasValue)
            {
                var toDateString = toDate.Value.ToString("yyyy-MM-dd"); // تنسيق التاريخ إلى نص
                ViewBag.DTT = toDate.Value.ToString("yyyy-MM-dd");
                Protlist = Protlist.Where(x => x.Tdate.CompareTo(toDateString) <= 0); // مقارنة النصوص
            }
            if (fromDate.HasValue && toDate.HasValue)
            {
                ViewBag.Reptitle = "من تاريخ : " + fromDate.Value.ToString("dd-MM-yyyy") + " الى تاريخ : " + toDate.Value.ToString("dd-MM-yyyy");
            }
            else
            {
                ViewBag.Reptitle = "الكل";
            }
            // التصفية حسب المشاريع، إذا كانت هناك قيم تم تحديدها
            if (projectId != null && projectId.Length > 0)
            {
                Protlist = Protlist.Where(x => x.Proid.HasValue && projectId.Contains(x.Proid.Value)).AsQueryable();
                ViewBag.Pro = Prolist.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Projectname,
                    Selected = projectId.Contains(p.Id)
                }).ToList();
            }

            if (ownerId.HasValue)
            {
                Protlist = Protlist.Where(x => x.Prtid == ownerId.Value);  // assuming the ownerId matches Prtid
                ViewBag.PartnerList = Prtlist.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Partnername?.ToString(),
                    Selected = ownerId.Equals(p.Id)
                }).ToList();
            }
            Protlist = Protlist.OrderBy(x => x.Tdate);
            return View(Protlist.ToList());
        }

        [HttpPost]
        public IActionResult ExportToExcel([FromBody] List<Transactionmodel> data)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Report");

                // إضافة عناوين الأعمدة
                worksheet.Cell(1, 1).Value = "المشروع";
                worksheet.Cell(1, 2).Value = "الوصف";
                worksheet.Cell(1, 3).Value = "الاجمالي";
                worksheet.Cell(1, 4).Value = "الضريبه";
                worksheet.Cell(1, 5).Value = "التاريخ";
                worksheet.Cell(1, 6).Value = "ملاحظات";

                // إضافة البيانات من الـ List
                for (int i = 0; i < data.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = data[i].Projectname;
                    worksheet.Cell(i + 2, 2).Value = data[i].Detailes;
                    worksheet.Cell(i + 2, 3).Value = data[i].Creditor;
                    worksheet.Cell(i + 2, 4).Value = data[i].Vatamount;
                    worksheet.Cell(i + 2, 5).Value = data[i].Tdate?.ToString();
                    worksheet.Cell(i + 2, 6).Value = data[i].Note;
                }

                // تحديد نوع الملف (Excel) واسم الملف
                using (var memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "تقرير المبيعات.xlsx");
                }
            }
        }
    }
}
