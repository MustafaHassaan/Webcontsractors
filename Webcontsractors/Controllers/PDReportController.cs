using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repo.Unitofwork;
using Webcontsractors.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Webcontsractors.Controllers
{
    public class PDReportController : Rolecontroller
    {
        IUnitofwork _IUW;
        public PDReportController(IUnitofwork IUW)
        {
            _IUW = IUW;
        }
        public IActionResult Index(DateTime? fromDate, DateTime? toDate,int[] projectId, int? ownerId)
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
            // التصفية حسب اسم المشروع (استخدام Contains للمصفوفة)
            if (projectId != null && projectId.Length > 0)
            {
                filteredList = filteredList.Where(x => projectId.Contains(x.Pro?.Id ?? 0));
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
                Note = x.Note,
            }).ToList();

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
                worksheet.Cell(1, 3).Value = "الوصف";
                worksheet.Cell(1, 4).Value = "دائن";
                worksheet.Cell(1, 5).Value = "مدين";
                worksheet.Cell(1, 6).Value = "الضريبه";
                worksheet.Cell(1, 7).Value = "الفرق";
                worksheet.Cell(1, 8).Value = "ملاحظات";

                // إضافة البيانات من الـ List
                for (int i = 0; i < data.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = data[i].Partnername;
                    worksheet.Cell(i + 2, 2).Value = data[i].Projectname;
                    worksheet.Cell(i + 2, 3).Value = data[i].Tdate.ToString();
                    worksheet.Cell(i + 2, 4).Value = data[i].Creditor;
                    worksheet.Cell(i + 2, 5).Value = data[i].Debitor;
                    worksheet.Cell(i + 2, 6).Value = data[i].Balance;
                    worksheet.Cell(i + 2, 7).Value = data[i].Vatamount;
                    worksheet.Cell(i + 2, 8).Value = data[i].Note;
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
