using ClosedXML.Excel;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repo.Unitofwork;
using Webcontsractors.Domain.Models;
using Webcontsractors.Models;

namespace Webcontsractors.Controllers
{
    public class PrtacountController : Rolecontroller
    {
        IUnitofwork _IUW;
        public PrtacountController(IUnitofwork IUW)
        {
            _IUW = IUW;
        }
        public IActionResult Index(DateTime? fromDate, DateTime? toDate, int[] projectId, int? ownerId)
        {
            var fromDateOnly = fromDate.HasValue ? DateOnly.FromDateTime(fromDate.Value) : DateOnly.MinValue;
            var toDateOnly = toDate.HasValue ? DateOnly.FromDateTime(toDate.Value) : DateOnly.MaxValue;

            var projects = _IUW.Projects.GetAll();
            var partners = _IUW.Partners.GetAll();

            var transactions = _IUW.Payments.GetAll()
                .Where(p => p.Pro == null || p.Pro.Status == "Close")
                .ToList();

            // فلترة بالتاريخ
            if (fromDate.HasValue)
                transactions = transactions.Where(p => p.Date >= fromDateOnly).ToList();

            if (toDate.HasValue)
                transactions = transactions.Where(p => p.Date <= toDateOnly).ToList();

            // فلترة حسب المشروع
            if (projectId?.Length > 0)
                transactions = transactions.Where(p => projectId.Contains(p.Proid ?? 0)).ToList();

            // فلترة حسب الشريك
            if (ownerId.HasValue)
                transactions = transactions.Where(p => p.Prt?.Id == ownerId).ToList();

            // الرصيد الافتتاحي العام (قبل fromDate)
            var allTransactions = _IUW.Payments.GetAll()
                .Where(p => p.Pro == null || p.Pro.Status == "Close");

            if (ownerId.HasValue)
                allTransactions = allTransactions.Where(p => p.Prtid == ownerId);

            var openingBalance = allTransactions
                .Where(t => t.Date < fromDateOnly)
                .Select(t => t.Creditor ?? 0)
                .Sum();

            ViewBag.OpeningBalance = openingBalance;

            var viewModel = transactions
                .GroupBy(t => new { t.Proid, t.Pro?.Projectname, t.Prt?.Partnername })
                .Select(g =>
                {
                    var orderedTransactions = g
                        .OrderByDescending(t => t.Date)
                        .ToList();

                    return new PARepviewmodel
                    {
                        Proid = g.Key.Proid ?? 0,
                        ProjectName = g.Key.Projectname ?? "غير معروف",
                        PartnerName = g.Key.Partnername ?? "غير معروف",

                        OpeningBalance = _IUW.Payments.GetAll()
                            .Where(p =>
                                p.Proid == g.Key.Proid &&
                                p.Date < fromDateOnly &&
                                (p.Pro == null || p.Pro?.Status == "Close") &&
                                (!ownerId.HasValue || p.Prtid == ownerId))
                            .Select(p => p.Creditor ?? 0)
                            .Sum(),

                        Transactions = orderedTransactions
                            .Select(t => new TransactionViewModel
                            {
                                Detailes = t.Note,
                                Tdate = t.Date.HasValue ? t.Date.Value.ToString("dd-MM-yyyy") : "بدون تاريخ",
                                Creditor = t.Creditor ?? 0,
                                Debitor = t.Debitor ?? 0,
                            }).ToList(),

                        LastTransactionDate = orderedTransactions.FirstOrDefault()?.Date ?? DateOnly.MinValue
                    };
                })
                .ToList();

            // ترتيب بعد التكوين النهائي
            viewModel = viewModel
                .OrderByDescending(x => x.LastTransactionDate)
                .ToList();

            ViewBag.Pro = projects
                .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Projectname }).ToList();

            ViewBag.PartList = partners
                .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Partnername }).ToList();

            if (fromDate.HasValue && toDate.HasValue)
            {
                ViewBag.Reptitle = "من تاريخ : " + fromDate.Value.ToString("dd-MM-yyyy") + " الى تاريخ : " + toDate.Value.ToString("dd-MM-yyyy");
            }
            else
            {
                ViewBag.Reptitle = "الكل";
            }

            if (ownerId.HasValue)
            {
                ViewBag.PartnerName = transactions.Select(x => x.Prt.Partnername).FirstOrDefault();
            }
            else
            {
                ViewBag.PartnerName = "الكل";
            }

            return View(viewModel);
        }


    }
}
