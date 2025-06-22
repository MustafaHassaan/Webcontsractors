using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Evaluation;
using Newtonsoft.Json;
using Repo.Unitofwork;
using Rotativa.AspNetCore;
using System.Net;
using Webcontsractors.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Webcontsractors.Controllers
{
    public class PSReportController : Rolecontroller
    {
        IUnitofwork _IUW;
        public PSReportController(IUnitofwork IUW)
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
            var tax = 1.15;
            var transactions = _IUW.TblTransactions.GetAllwithmaster("Pro").Select(x => new
            {
                x.Pro,
                x.Proid,
                x.Pro?.Prt?.Id,
                x.Pro?.Prt?.Partnername,
                x.Pro?.Projectname,
                x.Creditor,
                x.Debitor,
                x.Tdate,
                x.Note
            });
            // Apply filters
            if (fromDate.HasValue)
            {
                var fromDateOnly = DateOnly.FromDateTime(fromDate.Value);
                transactions = transactions.Where(x => x.Tdate >= fromDateOnly);
                ViewBag.DTF = fromDate.Value.ToString("yyyy-MM-dd");

            }
            if (toDate.HasValue)
            {
                var toDateOnly = DateOnly.FromDateTime(toDate.Value);
                transactions = transactions.Where(x => x.Tdate <= toDateOnly);
                ViewBag.DTT = toDate.Value.ToString("yyyy-MM-dd");
            }
            if (fromDate.HasValue && toDate.HasValue)
            {
                ViewBag.Reptitle = "من تاريخ : " + fromDate.Value.ToString("dd-MM-yyyy") + " الى تاريخ : " + toDate.Value.ToString("dd-MM-yyyy");
            }
            else
            {
                ViewBag.Reptitle = "الكل";
            }

            // تصفية بناءً على projectId (مصفوفة) باستخدام Contains
            if (projectId != null && projectId.Length > 0)
            {
                transactions = transactions.Where(x => x.Proid.HasValue && projectId.Contains(x.Proid.Value)).ToList();
                ViewBag.Pro = Prolist.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Projectname?.ToString(),
                    Selected = projectId.Contains(p.Id)
                }).ToList();
            }

            if (ownerId.HasValue)
            {
                transactions = transactions.Where(x => x.Pro?.Prt?.Id == ownerId.Value);
                ViewBag.PartnerList = Prtlist.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Partnername?.ToString(),
                    Selected = ownerId.Equals(p.Id)
                }).ToList();
            }
            // ترتيب العمليات حسب التاريخ قبل التجميع
            transactions = transactions.OrderBy(t => t.Tdate);
            var report = transactions
                    .GroupBy(t => t.Proid)
                    .Select(g => new ProjectReportModel
                    {
                        //ProjectId = g.Select(d => d.Pro?.Id).FirstOrDefault(),
                        //PartnerId = g.Select(d => d.Pro?.Prt?.Id).FirstOrDefault(),
                        PartnerName = g.Select(d => d.Partnername).LastOrDefault(),
                        ProjectName = g.Select(d => d.Projectname).LastOrDefault(),
                        LastTransactionDate = g.Select(d => d.Tdate?.ToString("dd-MM-yyyy")).LastOrDefault(),
                        CreditorSum = Math.Round(g.Sum(x => Convert.ToDouble(x.Creditor) / tax), 2),
                        DebitorSum = Math.Round(g.Sum(x => Convert.ToDouble(x.Debitor) / tax), 2),
                        Balance = Math.Round(
                            g.Sum(x => Convert.ToDouble(x.Creditor) / tax) -
                            g.Sum(x => Convert.ToDouble(x.Debitor) / tax), 2),
                        Note = g.Select(t => t.Note).LastOrDefault(),
                    }).ToList();
            return View(report); // View عادي فيه Partial View
        }

        [HttpPost]
        public IActionResult ExportToExcel([FromBody] List<ProjectReportModel> data)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Report");

                // إضافة عناوين الأعمدة
                worksheet.Cell(1, 1).Value = "المشروع";
                worksheet.Cell(1, 2).Value = "التاريخ";
                worksheet.Cell(1, 3).Value = "اجمالي الدائن بدون ضريبه";
                worksheet.Cell(1, 4).Value = "اجمالي المدين بدون صريبه";
                worksheet.Cell(1, 5).Value = "الفرق";
                worksheet.Cell(1, 6).Value = "الشريك";
                worksheet.Cell(1, 7).Value = "الشريك";

                // إضافة البيانات من الـ List
                for (int i = 0; i < data.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = data[i].ProjectName;
                    worksheet.Cell(i + 2, 2).Value = data[i].LastTransactionDate;
                    worksheet.Cell(i + 2, 3).Value = data[i].CreditorSum;
                    worksheet.Cell(i + 2, 4).Value = data[i].DebitorSum;
                    worksheet.Cell(i + 2, 5).Value = data[i].Balance;
                    worksheet.Cell(i + 2, 6).Value = data[i].PartnerName;
                    worksheet.Cell(i + 2, 7).Value = data[i].Note;
                }

                // تحديد نوع الملف (Excel) واسم الملف
                using (var memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "تقرير مختصر المشروعات.xlsx");
                }
            }
        }

    }
}
