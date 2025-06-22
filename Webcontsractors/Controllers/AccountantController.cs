using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repo.Unitofwork;
using Webcontsractors.Domain.Models;
using Webcontsractors.Models;

namespace Webcontsractors.Controllers
{
    public class AccountantController : Rolecontroller
    {
        IUnitofwork _IUW;
        public AccountantController(IUnitofwork IUW)
        {
            _IUW = IUW;
        }
        public IActionResult Accountlist()
        {
            _IUW.Projects.GetAll();
            _IUW.Partners.GetAll();
            IEnumerable<Payment> payments = _IUW.Payments.GetAll();

            // تحويل البيانات إلى النوع المطلوب
            IEnumerable<Paymentview> paymentViews = payments.Select(p => new Paymentview
            {
                Id = p.Id,
                Note = p.Note,
                Creditor = p.Creditor,
                Debitor = p.Debitor,
                Pro = p.Pro,
                Prt = p.Prt,
                Date = p.Date
            });

            return View(paymentViews);
        }
        public IActionResult AddAccount()
        {
            ViewBag.Prt = _IUW.Partners.GetAll()
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),   // أو الـ Prtid لو تحب
                    Text = x.Partnername
                }).ToList();
           ViewBag.Pro = _IUW.Projects.GetAll()
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),   // أو الـ Prtid لو تحب
                    Text = x.Projectname
                }).ToList();
            ViewBag.Date = DateTime.Now.ToString("yyyy-MM-dd");
            return View();
        }
        [HttpPost]
        public ActionResult AddAccount(Paymentview Pay)
        {
            Payment NPay = new Payment();
            NPay.Creditor = Pay.Creditor;
            NPay.Debitor = Pay.Debitor;
            NPay.Date = Pay.Date;
            NPay.Note = Pay.Note;
            NPay.Prtid = Pay.Prtid;
            NPay.Proid = Pay.Proid;
            _IUW.Payments.Insert(NPay);
            _IUW.Complete();
            return Redirect("Accountlist");
        }

        public ActionResult EditAccount(int id)
        {
            var Pay = _IUW.Payments.Get(id);

            ViewBag.Prt = _IUW.Partners.GetAll()
                            .Select(x => new SelectListItem
                            {
                                Value = x.Id.ToString(),   // أو الـ Prtid لو تحب
                                Text = x.Partnername
                            }).ToList();
            ViewBag.Pro = _IUW.Projects.GetAll()
 .Select(x => new SelectListItem
 {
     Value = x.Id.ToString(),   // أو الـ Prtid لو تحب
     Text = x.Projectname
 }).ToList();


            if (Pay != null)
            {
                Paymentview NPay = new Paymentview();
                NPay.Id = Pay.Id;
                NPay.Note = Pay.Note;
                NPay.Creditor = Pay.Creditor;
                NPay.Debitor = Pay.Debitor;
                NPay.Proid = Pay.Proid;
                NPay.Prtid = Pay.Prtid;
                if (Pay.Date == null)
                {
                    ViewBag.Date = DateTime.Now.ToString("yyyy-MM-dd");
                }
                else
                {
                    NPay.Date = Pay.Date;
                    ViewBag.Date = Pay.Date.Value.ToString("yyyy-MM-dd");
                }
                NPay.Note = Pay.Note;
                return View(NPay);
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult EditAccount(Paymentview Pay)
        {
            Payment NPay = new Payment();
            NPay.Id = Pay.Id;
            NPay.Creditor = Pay.Creditor;
            NPay.Debitor = Pay.Debitor;
            NPay.Date = Pay.Date;
            NPay.Note = Pay.Note;
            NPay.Prtid = Pay.Prtid;
            NPay.Proid = Pay.Proid;
            _IUW.Payments.Update(NPay);
            _IUW.Complete();
            return RedirectToAction("Accountlist");
        }

        public ActionResult DeleteAccount(Paymentview Pay)
        {
            _IUW.Payments.Delbyid(Pay.Id);
            _IUW.Complete();
            return RedirectToAction("Accountlist");
        }


        //public IActionResult Prodetailes(int id)
        //{
        //    var Prtlist = _IUW.Partners.GetAll();
        //    List<SelectListItem> SLI = new List<SelectListItem>();
        //    foreach (var Prt in Prtlist)
        //    {
        //        SelectListItem item = new SelectListItem();
        //        item.Value = Prt.Id.ToString();
        //        item.Text = Prt.Partnername;
        //        SLI.Add(item);
        //    }
        //    ViewBag.Prt = SLI;
        //    var Trn = _IUW.TblTransactions.GetAllwithmaster("Pro").Where(x => x.Proid == id);
        //    var TL = Trn.ToList();
        //    if (TL.Count > 0)
        //    {
        //        var TrnPro = Trn.Select(x => x.Pro).FirstOrDefault();
        //        Projectmodel NPro = new Projectmodel();
        //        NPro.Projectname = TrnPro?.Projectname;
        //        NPro.Amount = TrnPro?.Amount;
        //        NPro.Amountvat = TrnPro?.Amountvat;
        //        NPro.Prtid = TrnPro?.Prtid;
        //        var Blcdata = Trn.GroupBy(t => t.Proid == id).Select(t => new {
        //            Blc = Math.Round(t.Sum(d => Convert.ToDouble(d.Creditor)) -
        //                         t.Sum(d => Convert.ToDouble(d.Debitor)) - t.Sum(d => Convert.ToDouble(d.Vatamount)), 2),
        //        }).FirstOrDefault();
        //        NPro.Balance = decimal.Parse(Blcdata?.Blc.ToString());
        //        NPro.Partnername = TrnPro?.Prt?.Partnername;
        //        if (TrnPro?.Tdate == null)
        //        {
        //            ViewBag.Date = DateTime.Now.ToString("yyyy-MM-dd");
        //        }
        //        else
        //        {
        //            ViewBag.Date = TrnPro.Tdate?.ToString("yyyy-MM-dd");
        //        }
        //        NPro.Note = TrnPro?.Note;
        //        NPro.Status = TrnPro?.Status;
        //        foreach (var Trans in Trn)
        //        {
        //            NPro.Transactions.Add(new TblTransaction
        //            {
        //                Id = Trans.Id,
        //                Creditor = Trans.Creditor,
        //                Debitor = Trans.Debitor,
        //                Tdate = Trans.Tdate,
        //                Detailes = Trans.Detailes,
        //                Vatamount = Trans.Vatamount
        //            });
        //        }
        //        return View(NPro);
        //    }
        //    else
        //    {
        //        Projectmodel NPro = new Projectmodel();
        //        var Pro = _IUW.Projects.GetAllwithmaster("Prt").Where(x => x.Id == id).First();
        //        NPro.Projectname = Pro.Projectname;
        //        NPro.Amount = Pro.Amount;
        //        NPro.Amountvat = Pro.Amountvat;
        //        NPro.Prtid = Pro.Prtid;
        //        NPro.Partnername = Pro.Prt?.Partnername;
        //        NPro.Note = Pro.Note;
        //        NPro.Status = Pro.Status;
        //        return View(NPro);
        //    }
        //}

        public IActionResult Prodetailes(int id)
        {
            var Prtlist = _IUW.Partners.GetAll();
            List<SelectListItem> SLI = new List<SelectListItem>();

            foreach (var Prt in Prtlist)
            {
                SLI.Add(new SelectListItem
                {
                    Value = Prt.Id.ToString(),
                    Text = Prt.Partnername
                });
            }

            ViewBag.Prt = SLI;

            var Trn = _IUW.TblTransactions.GetAllwithmaster("Pro").Where(x => x.Proid == id);
            var TL = Trn.ToList();

            if (TL.Count > 0)
            {
                var TrnPro = Trn.Select(x => x.Pro).FirstOrDefault();
                if (TrnPro == null)
                {
                    return NotFound();
                }

                Projectmodel NPro = new Projectmodel
                {
                    Projectname = TrnPro.Projectname,
                    Amount = TrnPro.Amount ?? 0,
                    Amountvat = TrnPro.Amountvat ?? 0,
                    Prtid = TrnPro.Prtid,
                    Partnername = TrnPro.Prt?.Partnername,
                    Note = TrnPro.Note,
                    Status = TrnPro.Status
                };

                var Blcdata = Trn.GroupBy(t => t.Proid == id).Select(t => new
                {
                    Blc = Math.Round(
                        t.Sum(d => Convert.ToDecimal(d.Creditor ?? 0)) -
                        t.Sum(d => Convert.ToDecimal(d.Debitor ?? 0)) -
                        t.Sum(d => Convert.ToDecimal(d.Vatamount ?? 0)), 2)
                }).FirstOrDefault();

                NPro.Balance = Blcdata != null ? decimal.Parse(Blcdata.Blc.ToString()) : 0;

                ViewBag.Date = TrnPro.Tdate?.ToString("yyyy-MM-dd") ?? DateTime.Now.ToString("yyyy-MM-dd");

                foreach (var Trans in Trn)
                {
                    NPro.Transactions.Add(new TblTransaction
                    {
                        Id = Trans.Id,
                        Creditor = Trans.Creditor,
                        Debitor = Trans.Debitor,
                        Tdate = Trans.Tdate,
                        Detailes = Trans.Detailes,
                        Vatamount = Trans.Vatamount
                    });
                }

                return View(NPro);
            }
            else
            {
                var Pro = _IUW.Projects.GetAllwithmaster("Prt").Where(x => x.Id == id).FirstOrDefault();
                if (Pro == null)
                {
                    ViewBag.Data = null;
                    return View();
                }
                else
                {
                    Projectmodel NPro = new Projectmodel
                    {
                        Projectname = Pro.Projectname,
                        Amount = Pro.Amount ?? 0,
                        Amountvat = Pro.Amountvat ?? 0,
                        Prtid = Pro.Prtid,
                        Partnername = Pro.Prt?.Partnername,
                        Note = Pro.Note,
                        Status = Pro.Status
                    };
                    ViewBag.Data = NPro;
                    return View(NPro);
                }
            }
        }

        [HttpPost]
        public IActionResult Saveadd(Paymentview Pay)
        {
            Payment NPay = new Payment();
            NPay.Creditor = Pay.Creditor;
            NPay.Debitor = Pay.Debitor;
            NPay.Date = Pay.Date;
            NPay.Note = Pay.Note;
            NPay.Prtid = Pay.Prtid;
            NPay.Proid = Pay.Proid;
            _IUW.Payments.Insert(NPay);
            _IUW.Complete();
            ViewBag.Prt = _IUW.Partners.GetAll()
    .Select(x => new SelectListItem
    {
        Value = x.Id.ToString(),   // أو الـ Prtid لو تحب
        Text = x.Partnername
    }).ToList();
            ViewBag.Pro = _IUW.Projects.GetAll()
                 .Select(x => new SelectListItem
                 {
                     Value = x.Id.ToString(),   // أو الـ Prtid لو تحب
                     Text = x.Projectname
                 }).ToList();
            ViewBag.Date = DateTime.Now.ToString("yyyy-MM-dd");
            return Json("Ok");
        }
    }
}
