using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repo.Unitofwork;
using System;
using Webcontsractors.Domain.Models;
using Webcontsractors.Models;

namespace Webcontsractors.Controllers
{
    public class DTReportController : Rolecontroller
    {
        IUnitofwork _IUW;
        public DTReportController(IUnitofwork IUW)
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
            // احصل على الشركاء
            var Prt = _IUW.Partners.GetAll();

            // احصل على قائمة المعاملات مع بيانات المشروع والشريك
            var Protlist = (_IUW.TblTransactions.GetAllwithmaster("Pro"))
                           .Select(x => new
                           {
                               x.Pro,
                               x.Detailes,
                               x.Pro?.Prt?.Partnername,
                               x.Pro?.Projectname,
                               x.Creditor,
                               x.Debitor,
                               x.Vatamount,
                               blance = x.Creditor - x.Debitor,
                               x.Tdate,
                               x.Note
                           })
                           .ToList(); // تحويل إلى قائمة لتقليل العمليات الحسابية المتكررة

            // تحديد الضريبة
            var tax = 1.15;
            // تصفية Protlist بناءً على المدخلات:
            var filteredList = Protlist.AsEnumerable();

            // التصفية حسب اسم الشريك
            if (ownerId.HasValue)
            {
                filteredList = filteredList.Where(x => x.Pro?.Prtid == ownerId.Value);
                ViewBag.PartnerList = Prtlist.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Partnername?.ToString(),
                    Selected = ownerId.Equals(p.Id)
                }).ToList();
            }

            // التصفية حسب اسم المشروع
            if (projectId != null && projectId.Any())
            {
                filteredList = filteredList.Where(x => x.Pro != null && projectId.Contains(x.Pro.Id));
                ViewBag.Pro = Prolist.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Projectname?.ToString(),
                    Selected = projectId.Contains(p.Id)
                }).ToList();
            }

            if (fromDate.HasValue)
            {
                var fromDateOnly = DateOnly.FromDateTime(fromDate.Value);
                ViewBag.DTF = fromDate.Value.ToString("yyyy-MM-dd");
                filteredList = filteredList.Where(x => x.Tdate >= fromDateOnly);
            }
            if (toDate.HasValue)
            {
                var toDateOnly = DateOnly.FromDateTime(toDate.Value);
                ViewBag.DTT = toDate.Value.ToString("yyyy-MM-dd");
                filteredList = filteredList.Where(x => x.Tdate <= toDateOnly);
            }
            if (fromDate.HasValue && toDate.HasValue)
            {
                ViewBag.Reptitle = "من تاريخ : " + fromDate.Value.ToString("dd-MM-yyyy") + " الى تاريخ : " + toDate.Value.ToString("dd-MM-yyyy");
            }
            else
            {
                ViewBag.Reptitle = "الكل";
            }
            // تحويل الفلاتر إلى نموذج (Model) جاهز للاستخدام في العرض
            var resultList = filteredList.Select(x => new TransactionViewModel
            {
                Projectname = x.Projectname,
                Partnername = x.Partnername,
                Creditor = x.Creditor,
                Debitor = x.Debitor,
                Vatamount = x.Vatamount ?? 0,
                Tdate = x.Tdate?.ToString("dd-MM-yyyy"),
                Detailes = x.Detailes,
                Balance = x.blance,
                Remaining = x.blance + x.Vatamount,
                Note = x.Note
            }).ToList();
            resultList = resultList.OrderBy(r => r.Tdate).ToList();
            // إرسال النتيجة إلى العرض
            return View(resultList);
        }

        [HttpPost]
        public IActionResult ExportToExcel([FromBody] List<TransactionViewModel> data)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Report");

                // إضافة عناوين الأعمدة
                worksheet.Cell(1, 1).Value = "الشريك";
                worksheet.Cell(1, 2).Value = "المشروع";
                worksheet.Cell(1, 3).Value = "مدين";
                worksheet.Cell(1, 4).Value = "الضريبه";
                worksheet.Cell(1, 5).Value = "التاريخ";
                worksheet.Cell(1, 6).Value = "ملاحظات";

                // إضافة البيانات من الـ List
                for (int i = 0; i < data.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = data[i].Partnername;
                    worksheet.Cell(i + 2, 2).Value = data[i].Projectname;
                    worksheet.Cell(i + 2, 3).Value = data[i].Debitor;
                    worksheet.Cell(i + 2, 4).Value = data[i].Vatamount;
                    worksheet.Cell(i + 2, 5).Value = data[i].Tdate.ToString();
                    worksheet.Cell(i + 2, 6).Value = data[i].Note;
                }

                // تحديد نوع الملف (Excel) واسم الملف
                using (var memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "تقرير تفصيلي المشروعات.xlsx");
                }
            }
        }
    }
}
