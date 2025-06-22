using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
using Repo.Unitofwork;
using System.Diagnostics;
using System.Transactions;
using Webcontsractors.Domain.Models;
using Webcontsractors.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Webcontsractors.Controllers
{
    public class HomeController : Rolecontroller
    {
        IUnitofwork _IUW;
        public HomeController(IUnitofwork IUW)
        {
            _IUW = IUW;
        }
        public ActionResult Index()
        {
            //var Username = HttpContext.Request.Cookies["Username"];
            //var Password = HttpContext.Request.Cookies["Password"];
            //var Id = HttpContext.Request.Cookies["Id"];
            return View();
        }

        public IActionResult ExportrptExcel()
        {
            var projects = _IUW.Projects.GetAll();
            var partners = _IUW.Partners.GetAll();
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
                .ToList();

            var viewModel = new List<PTRepviewmodel>();
            var workbook = new XLWorkbook();
            int rowall = 0;
            var worksheetall = workbook.Worksheets.Add("All"); // إضافة Sheet لكل مشروع
            worksheetall.Cell(1, 1).Value = "Projects";
            worksheetall.Cell(1, 2).Value = "Amount";
            rowall = 2;
            foreach (var project in projects)
            {
                decimal openingBalance = project.Opningbalance ?? 0;
                var projectTransactions = transactions.Where(t => t.Proid == project.Id).ToList();
                projectTransactions = projectTransactions.OrderBy(x => x.Tdate).ToList();

                openingBalance += projectTransactions.Sum(x => (x.Creditor ?? 0) - (x.Debitor ?? 0));
                viewModel.Add(new PTRepviewmodel
                {
                    Project = project,
                    OpeningBalance = openingBalance,
                    Transactions = projectTransactions
                });
                worksheetall.Cell(rowall, 1).Value = project.Projectname;
                worksheetall.Cell(rowall, 2).Value = openingBalance;
                rowall++;
            }
            int rowpro = 0;
            foreach (var project in viewModel)
            {
                var worksheet = workbook.Worksheets.Add(project.Project.Projectname); // إضافة Sheet لكل مشروع
                worksheet.Cell(1, 1).Value = "Credit";
                worksheet.Cell(1, 2).Value = "Debit";
                worksheet.Cell(1, 3).Value = "Balance";
                worksheet.Cell(1, 4).Value = "Date";
                worksheet.Cell(1, 5).Value = "VAT";
                rowpro = 2;
                foreach (var items in project.Transactions)
                {
                    worksheet.Cell(rowpro, 1).Value = items.Creditor;
                    worksheet.Cell(rowpro, 2).Value = items.Debitor;
                    worksheet.Cell(rowpro, 3).Value = "";
                    worksheet.Cell(rowpro, 4).Value = items.Tdate.ToString();
                    worksheet.Cell(rowpro, 5).Value = items.Vatamount;
                    rowpro++;
                }
            }
            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                var fileName = $"AllProjects_{DateTime.Now:yyyyMMdd}.xlsx";
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
    }
}
