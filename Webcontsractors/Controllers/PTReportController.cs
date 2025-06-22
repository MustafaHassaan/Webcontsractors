using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repo.Unitofwork;
using Webcontsractors.Domain.Models;
using Webcontsractors.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Webcontsractors.Controllers
{
    public class PTReportController : Rolecontroller
    {
        IUnitofwork _IUW;
        public PTReportController(IUnitofwork IUW)
        {
            _IUW = IUW;
        }
        public IActionResult Index(DateTime? fromDate, DateTime? toDate, int[] projectId, int? ownerId)
        {
            var projects = _IUW.Projects.GetAll();
            var partners = _IUW.Partners.GetAll();

            ViewBag.Pro = projects.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Projectname
            }).ToList();

            ViewBag.PartnerList = partners.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Partnername
            }).ToList();

            var transactions = _IUW.TblTransactions.GetAllwithmaster("Pro")
                .Select(x => new TblTransaction
                {
                    Tdate = x.Tdate,
                    Detailes = x.Detailes,
                    Creditor = x.Creditor ?? 0,
                    Debitor = x.Debitor ?? 0,
                    Vatamount = x.Vatamount,
                    Proid = x.Proid,
                    Pro = x.Pro
                })
                //.Where(x => x.Debitor != 0 && x.Vatamount != 0)
                .ToList();

            if (fromDate.HasValue)
            {
                var fromDateOnly = DateOnly.FromDateTime(fromDate.Value);
                ViewBag.DTF = fromDate.Value.ToString("yyyy-MM-dd");
            }

            if (toDate.HasValue)
            {
                var toDateOnly = DateOnly.FromDateTime(toDate.Value);
                ViewBag.DTT = toDate.Value.ToString("yyyy-MM-dd");
            }

            ViewBag.Reptitle = fromDate.HasValue && toDate.HasValue
                ? $"من تاريخ : {fromDate.Value:dd-MM-yyyy} الى تاريخ : {toDate.Value:dd-MM-yyyy}"
                : "الكل";

            if (projectId?.Length > 0)
            {
                transactions = transactions.Where(x => x.Proid.HasValue && projectId.Contains(x.Proid.Value)).ToList();
                ViewBag.Pro = projects.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Projectname,
                    Selected = projectId.Contains(p.Id)
                }).ToList();
            }

            if (ownerId.HasValue)
            {
                transactions = transactions.Where(x => x.Pro?.Prtid == ownerId.Value).ToList();
                ViewBag.PartnerList = partners.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Partnername,
                    Selected = ownerId.Equals(p.Id)
                }).ToList();
            }

            var viewModel = new List<PTRepviewmodel>();

            var selectedProjects = (projectId?.Length > 0)
                ? projects.Where(p => projectId.Contains(p.Id)).ToList()
                : projects;

            foreach (var project in selectedProjects)
            {
                // حركات هذا المشروع فقط
                var projectTransactions = transactions.Where(t => t.Proid == project.Id).ToList();
                // الترتيب حسب التاريخ
                projectTransactions = projectTransactions.OrderBy(x => x.Tdate).ToList();

                // الرصيد الافتتاحي قبل fromDate فقط
                decimal openingBalance = project.Opningbalance ?? 0; // استخدام الرصيد الافتتاحي من جدول المشاريع
                if (fromDate.HasValue)
                {
                    var fromDateOnly = DateOnly.FromDateTime(fromDate.Value);
                    openingBalance += projectTransactions
                        .Where(x => x.Tdate < fromDateOnly)
                        .Sum(x => (x.Creditor ?? 0) - (x.Debitor ?? 0));
                }

                // الفلترة حسب التاريخ
                if (fromDate.HasValue)
                {
                    var fromDateOnly = DateOnly.FromDateTime(fromDate.Value);
                    projectTransactions = projectTransactions.Where(x => x.Tdate >= fromDateOnly).ToList();
                }

                if (toDate.HasValue)
                {
                    var toDateOnly = DateOnly.FromDateTime(toDate.Value);
                    projectTransactions = projectTransactions.Where(x => x.Tdate <= toDateOnly).ToList();
                }

                viewModel.Add(new PTRepviewmodel
                {
                    Project = project,
                    OpeningBalance = openingBalance,
                    Transactions = projectTransactions
                });
            }


            return View(viewModel);
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
                worksheet.Cell(1, 8).Value = "التاريخ";

                // إضافة البيانات من الـ List
                for (int i = 0; i < data.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = data[i].Partnername;
                    worksheet.Cell(i + 2, 2).Value = data[i].Projectname;
                    worksheet.Cell(i + 2, 3).Value = data[i].Detailes;
                    worksheet.Cell(i + 2, 4).Value = data[i].Creditor;
                    worksheet.Cell(i + 2, 5).Value = data[i].Debitor;
                    worksheet.Cell(i + 2, 6).Value = data[i].Balance;
                    worksheet.Cell(i + 2, 7).Value = data[i].Vatamount;
                    worksheet.Cell(i + 2, 8).Value = data[i].Tdate.ToString();
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
