using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using Repo.Unitofwork;
using System.IO;
using System.Linq;
using Webcontsractors.Domain.Models;
using Webcontsractors.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Webcontsractors.Controllers
{
    public class Partnerscontroller : Rolecontroller
    {
        IUnitofwork _IUW;
        public Partnerscontroller(IUnitofwork IUW)
        {
            _IUW = IUW;
        }
        public IActionResult Index()
        {
            var Partlist = _IUW.Partners.GetAll().Select(x => new
            {
                x.Id,
                x.Partnername,
                x.Description,
            });
            List<Partnermodel> LPM = new List<Partnermodel>();
            foreach (var part in Partlist) {
                LPM.Add(new Partnermodel
                {
                    Id = part.Id,
                    Partnername = part.Partnername,
                    Description = part.Description,
                });
            }
            return View(LPM);
        }
		[HttpGet]
		public IActionResult Prtsave()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Prtsave(Partnermodel Prt)
		{
			Partner Parto = new Partner();
			Parto.Partnername = Prt.Partnername;
			Parto.Description = Prt.Description;
			Parto.Amount = Prt.Amount;
			Parto.Percentage = Prt.Percentage;
			_IUW.Partners.Insert(Parto);
			_IUW.Complete();
            return Redirect("Index");
		}
        [HttpGet]
        public ActionResult Prtedit(int id)
        {
            var MPrt = _IUW.Partners.Get(id);
            if (MPrt != null)
            {
                Partnermodel PM = new Partnermodel();
                PM.Id = MPrt.Id;
                PM.Partnername = MPrt.Partnername;
                PM.Description = MPrt.Description;
                PM.Amount = MPrt.Amount;
                PM.Percentage = Convert.ToInt32(MPrt.Percentage);
                return View(PM);
            }
            return View();
        }
        [HttpPost]
        public IActionResult Prtedit(Partnermodel Prt)
        {
            Partner Parto = new Partner();
            Parto.Id = Prt.Id;
            Parto.Partnername = Prt.Partnername;
            Parto.Description = Prt.Description;
            Parto.Amount = Prt.Amount;
            Parto.Percentage = Prt.Percentage;
            _IUW.Partners.Update(Parto);
            _IUW.Complete();
            return Json("Ok");
        }
        public IActionResult Prtdete(Partnermodel Prt)
        {
            Partner Parto = new Partner();
            Parto.Id = Prt.Id;
            _IUW.Partners.Delbyid(Parto.Id);
            _IUW.Complete();
            return Json("Ok");
        }

        [HttpGet]
        public JsonResult CalcProfitJson(int id)
        {
            //var Blcdata = GetFinancialSummaryAsync(id);
            var Blcdata = GetProjectFinancialSummary(id);
           return Json(Blcdata);
        }
        //public List<dynamic> GetProjectFinancialSummary(int prtid)
        //{
        //    var result = _IUW.TblTransactions.GetAll()
        //        .Where(t => t.Prtid == prtid)
        //        .Join(_IUW.Projects.GetAll().Where(p => p.Status == "Close"),
        //              t => t.Proid,
        //              p => p.Id,
        //              (t, p) => new { t, p })
        //         .GroupBy(x => new { x.t.Proid, x.t.Prtid }) // تجميع حسب معرف المشروع والشريك
        //        .Select(g => new
        //        {
        //            Proid = g.Key.Proid, // كل مشروع على حدة
        //            Prtid = g.Key.Prtid, // معرف الشريك
        //            //Creditor = g.Sum(x => x.t.Creditor ?? 0),
        //            //Debitor = g.Sum(x => x.t.Debitor ?? 0),
        //            Total = g.Sum(x => x.t.Creditor ?? 0) - g.Sum(x => x.t.Debitor ?? 0)
        //        }).ToList<dynamic>(); // تحويل القائمة إلى Dynamic

        //    // معالجة حالة عدم وجود بيانات
        //    return result.Count > 0 ? result : new List<dynamic> { new { Message = "لا توجد بيانات متاحة" } };
        //}
        public List<dynamic> GetProjectFinancialSummary(int prtid)
        {
            // خطوة 1: احصل على كل المشاريع المقفولة
            var closedProjects = _IUW.Projects.GetAll()
                .Where(p => p.Status == "Close")
                .Select(p => p.Id)
                .ToList();

            // خطوة 2: احصل على المعاملات الخاصة بالشريك ومشاريعه المقفولة
            var transactions = _IUW.TblTransactions.GetAll()
                .Where(t => t.Prtid == prtid)
                .ToList();

            // خطوة 3: عمل GroupBy يدوي ومعالجة كل مجموعة
            var result = transactions
                .GroupBy(t => new { t.Proid, t.Prtid })
                .Select(g =>
                {
                    var creditorTotal = g.Sum(x => x.Creditor ?? 0);
                    var debitorTotal = g.Sum(x => x.Debitor ?? 0);
                    var creditorVat = g.Where(x => (x.Creditor ?? 0) > 0).Sum(x => x.Vatamount ?? 0);
                    var debitorVat = g.Where(x => (x.Debitor ?? 0) > 0).Sum(x => x.Vatamount ?? 0);

                    var total = Math.Round(
                        creditorTotal - debitorTotal - creditorVat + debitorVat,
                        2
                    );

                    return new
                    {
                        Proid = g.Key.Proid,
                        Prtid = g.Key.Prtid,
                        Creditor = creditorTotal,
                        Debitor = debitorTotal,
                        CreditorVat = creditorVat,
                        DebitorVat = debitorVat,
                        Total = total
                    };
                })
                .ToList<dynamic>();

            // في حالة عدم وجود بيانات
            return result.Any() ? result : new List<dynamic> { new { Message = "لا توجد بيانات متاحة" } };
        }



        //public object GetFinancialSummaryAsync(int prtid)
        //{
        //    var result = _IUW.TblTransactions.GetAll()
        //        .Where(t => t.Prtid == prtid)
        //        .Join(_IUW.Projects.GetAll().Where(p => p.Status == "Close"),
        //              t => t.Proid,
        //              p => p.Id,
        //              (t, p) => new { t, p })
        //        .GroupBy(x => x.t.Prtid)
        //        .Select(g => new
        //        {
        //            Creditor = g.Sum(x => x.t.Creditor ?? 0),
        //            Debitor = g.Sum(x => x.t.Debitor ?? 0),
        //            Vatamount = g.Sum(x => x.t.Vatamount ?? 0),
        //            Total = g.Sum(x => x.t.Creditor ?? 0) - g.Sum(x => x.t.Debitor ?? 0)
        //        }).FirstOrDefault();

        //    return result;
        //}


        //Receivepayment
        [HttpPost]
        public IActionResult Receivepayment([FromBody] DataModel data)
        {
            for (int i = 0; i < data.Receivepayment.Count; i++)
            {
                var Proid = data.Receivepayment[i].Proid;
                var Pid = _IUW.Payments.Get(Proid);
                Payment NP = new Payment();
                if (Pid == null)
                {
                    NP.Proid = data.Receivepayment[i].Proid;
                    NP.Prtid = data.Receivepayment[i].Prtid;
                    NP.Creditor = data.Receivepayment[i].Total;
                    NP.Date = DateOnly.FromDateTime(DateTime.Now);
                    _IUW.Payments.Insert(NP);
                }
                else
                {
                    Pid.Id = Pid.Id;
                    Pid.Proid = data.Receivepayment[i].Proid;
                    Pid.Prtid = data.Receivepayment[i].Prtid;
                    Pid.Creditor = data.Receivepayment[i].Total;
                    Pid.Date = DateOnly.FromDateTime(DateTime.Now);
                    _IUW.Payments.Update(NP);
                }
                _IUW.Complete();
            }
            return Json(new { Message = "تم استقبال البيانات بنجاح!", ReceivedData = data });
        }

        [HttpGet]
        public JsonResult Calcprtprofits(int id)
        {
            var totalProfit = GetProjectFinancialprt(id);
            return Json(totalProfit);
        }
        //public object GetProjectFinancialprt(int prtid)
        //{
        //    var result = _IUW.TblTransactions.GetAll()
        //        .Where(t => t.Prtid == prtid)
        //        .Join(_IUW.Projects.GetAll().Where(p => p.Status == "Close"),
        //              t => t.Proid,
        //              p => p.Id,
        //              (t, p) => new { t, p })
        //        .GroupBy(x => new { x.t.Proid, x.t.Prtid })
        //        .Select(g => new
        //        {
        //            Creditor = g.Sum(x => x.t.Creditor ?? 0),
        //            Debitor = g.Sum(x => x.t.Debitor ?? 0),
        //            Total = g.Sum(x => x.t.Creditor ?? 0) - g.Sum(x => x.t.Debitor ?? 0)
        //        }).ToList();

        //    if (result.Count > 0)
        //    {
        //        var grandTotal = result.Sum(x => (decimal)x.Total);
        //        return new { TotalSum = grandTotal };
        //    }

        //    return new { Message = "No Data" };
        //}
        public object GetProjectFinancialprt(int prtid)
        {
            // جلب المشاريع المغلقة فقط
            var closedProjects = _IUW.Projects.GetAll()
                .Where(p => p.Status == "Close")
                .Select(p => p.Id)
                .ToList();

            // جلب المعاملات المرتبطة بالشريك والمشاريع المقفولة
            var transactions = _IUW.TblTransactions.GetAll()
                .Where(t => t.Prtid == prtid)
                .ToList();

            // تجميع البيانات وحساب القيم
            var result = transactions
                .GroupBy(t => new { t.Proid, t.Prtid })
                .Select(g =>
                {
                    var creditorTotal = g.Sum(x => x.Creditor ?? 0);
                    var debitorTotal = g.Sum(x => x.Debitor ?? 0);
                    var creditorVat = g.Where(x => (x.Creditor ?? 0) > 0).Sum(x => x.Vatamount ?? 0);
                    var debitorVat = g.Where(x => (x.Debitor ?? 0) > 0).Sum(x => x.Vatamount ?? 0);

                    var total = Math.Round(
                        creditorTotal - debitorTotal - creditorVat + debitorVat,
                        2
                    );

                    return new
                    {
                        Total = total
                    };
                })
                .ToList();

            if (result.Count > 0)
            {
                var grandTotal = result.Sum(x => (decimal)x.Total);
                return new { TotalSum = grandTotal };
            }

            return new { Message = "No Data" };
        }

    }
}
