using ClosedXML.Excel;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Repo.Unitofwork;
using System.Diagnostics;
using Webcontsractors.Domain.Models;
using Webcontsractors.Models;

namespace Webcontsractors.Controllers
{
    public class Transactionscontroller : Rolecontroller
    {
        IUnitofwork _IUW;
        public Transactionscontroller(IUnitofwork IUW)
        {
            _IUW = IUW;
        }
        public IActionResult Index()
        {
            var Prolist = _IUW.Projects.GetAll();
            var Prt = _IUW.Partners.GetAll();
            List<SelectListItem> SLI = new List<SelectListItem>();
            foreach (var Pro in Prolist)
            {
                SelectListItem item = new SelectListItem();
                item.Value = Pro.Id.ToString();
                item.Text = Pro.Projectname;
                SLI.Add(item);
            }
            ViewBag.Pro = SLI;
            //var Trnlist = _IUW.TblTransactions.GetAllwithmaster("Pro").Select(x => new
            //{
            //    x.Id,
            //    x.Creditor,
            //    x.Debitor,
            //    x.Tdate,
            //    x.Detailes,
            //    x.Vatamount,
            //    x.Note,
            //    x.Pro?.Projectname,
            //    x.Pro,
            //    x.Prtid,
            //}).OrderByDescending(x => x.Tdate);
            //List<Transactionmodel> LTM = new List<Transactionmodel>();
            //foreach (var Trn in Trnlist)
            //{
            //    var Date = Convert.ToDateTime(Trn.Tdate?.ToString()).ToString("yyyy-MM-dd");
            //    var Prtname = "";
            //    if (Trn.Prtid != null)
            //    {
            //        var GP = Prt.Where(x => Trn.Prtid == x.Id).FirstOrDefault();
            //        Prtname = GP?.Partnername;
            //    }
            //    LTM.Add(new Transactionmodel
            //    {
            //        Id = Trn.Id,
            //        Projectname = Trn.Projectname,
            //        Creditor = Trn.Creditor,
            //        Debitor = Trn.Debitor,
            //        Balance = Trn.Creditor - Trn.Debitor,
            //        Detailes = Trn.Detailes,
            //        Vatamount = Trn.Vatamount ?? 0,
            //        Note = Trn.Note,
            //        Tdate = Date,
            //        Proid = Trn.Pro?.Id,
            //        Prtname = Prtname,
            //    });
            //}
            //int chunkSize = 500; // تقسيم كل مجموعة إلى 500 عنصر

            //var chunks = LTM.Select((x, i) => new { Index = i, Value = x })
            //                   .GroupBy(x => x.Index / chunkSize)
            //                   .Select(g => g.Select(x => x.Value).ToList())
            //                   .ToList();
            //ViewBag.chunks = chunks.Count;
            //var i = 0;
            //var Data = chunks.Skip(i).FirstOrDefault();
            return View(/*Data*/);
        }
        public IActionResult Top()
        {
            var Trnlist = _IUW.TblTransactions.GetAllwithmaster("Pro").Select(x => new
            {
                x.Id,
                x.Creditor,
                x.Debitor,
                x.Tdate,
                x.Detailes,
                x.Vatamount,
                x.Note,
                x.Pro?.Projectname,
            }).Where(x => x.Projectname == "" || x.Projectname == null);
            List<Transactionmodel> LTM = new List<Transactionmodel>();
            foreach (var Trn in Trnlist)
            {
                var Date = Convert.ToDateTime(Trn.Tdate?.ToString()).ToString("yyyy-MM-dd");
                LTM.Add(new Transactionmodel
                {
                    Id = Trn.Id,
                    Projectname = Trn.Projectname,
                    Creditor = Trn.Creditor,
                    Debitor = Trn.Debitor,
                    Balance = Trn.Creditor - Trn.Debitor,
                    Detailes = Trn.Detailes,
                    Vatamount = Trn.Vatamount ?? 0,
                    Note = Trn.Note,
                    Tdate = Date,
                });
            }
            return View(LTM);
        }
        [HttpPost]
        public IActionResult Index(string DTF, string DTT, string Dept, string Cred, List<int> Pro)
        {
            // تحميل المشاريع في ViewBag
            var Prolist = _IUW.Projects.GetAll();
            var Prt = _IUW.Partners.GetAll();
            ViewBag.Pro = Prolist.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Projectname
            }).ToList();
            // استرجاع القائمة بعد الفلترة
            ViewBag.Pro = Prolist.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Projectname,
                Selected = Pro.Contains(p.Id)
            }).ToList();

            // جلب المعاملات
            var Trnlist = _IUW.TblTransactions.GetAllwithmaster("Pro").Select(x => new
            {
                x.Id,
                x.Creditor,
                x.Debitor,
                x.Tdate,
                x.Detailes,
                x.Vatamount,
                x.Note,
                x.Pro?.Projectname,
                x.Proid,
                x.Prtid,
                x.Prt,
            });

            // فلترة البيانات
            if (!string.IsNullOrEmpty(DTF) && DateOnly.TryParse(DTF, out var dtf))
            {
                Trnlist = Trnlist.Where(t => t.Tdate >= dtf);
            }

            if (!string.IsNullOrEmpty(DTT) && DateOnly.TryParse(DTT, out var dtt))
            {
                Trnlist = Trnlist.Where(t => t.Tdate <= dtt);
            }

            if (!string.IsNullOrEmpty(Dept) && decimal.TryParse(Dept, out var deptVal))
            {
                Trnlist = Trnlist.Where(t => t.Debitor >= deptVal && t.Debitor <= deptVal);
            }

            if (!string.IsNullOrEmpty(Cred) && decimal.TryParse(Cred, out var credVal))
            {
                Trnlist = Trnlist.Where(t => t.Creditor >= credVal && t.Creditor <= credVal);
            }

            if (Pro != null && Pro.Any())
            {
                Trnlist = Trnlist.Where(t => t.Proid.HasValue && Pro.Contains(t.Proid.Value));
            }
            List<Transactionmodel> LTM = new List<Transactionmodel>();
            foreach (var Trn in Trnlist)
            {
                var Date = Convert.ToDateTime(Trn.Tdate?.ToString()).ToString("yyyy-MM-dd");
                var Prtname = "";
                if (Trn.Prtid != null)
                {
                    var GP = Prt.Where(x => Trn.Prtid == x.Id).FirstOrDefault();
                    Prtname = GP?.Partnername;
                }
                LTM.Add(new Transactionmodel
                {
                    Id = Trn.Id,
                    Projectname = Trn.Projectname,
                    Creditor = Trn.Creditor,
                    Debitor = Trn.Debitor,
                    Balance = Trn.Creditor - Trn.Debitor,
                    Detailes = Trn.Detailes,
                    Vatamount = Trn.Vatamount ?? 0,
                    Tdate = Date,
                    Proid = Trn.Proid,
                    Prtname = Prtname,
                    Note = Trn.Note,
                });
            }
            ViewBag.DTF = DTF ?? null;
            ViewBag.DTT = DTT ?? null;
            ViewBag.Cred = Cred ?? null;
            ViewBag.Dept = Dept ?? null;
            //ViewBag.Pro = Pro ?? null;
            return View(LTM);
        }
        [HttpGet]
        public IActionResult Trnsave()
        {
            ViewBag.PartnerList = _IUW.Partners.GetAll()
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),   // أو الـ Prtid لو تحب
                    Text = x.Partnername
                }).ToList();
            ViewBag.Pro = _IUW.Projects.GetAll()
                .Where(x => string.IsNullOrWhiteSpace(x.Status) || x.Status.Trim() != "Close")
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Projectname
                }).ToList();

            ViewBag.Date = DateTime.Now.ToString("yyyy-MM-dd");
            return View();
        }
        [HttpPost]
        public ActionResult Trnsave(TblTransaction Trn)
        {
            TblTransaction Trans = new TblTransaction();
            Trans.Creditor = Trn.Creditor;
            Trans.Debitor = Trn.Debitor;
            Trans.Tdate = Trn.Tdate;
            Trans.Detailes = Trn.Detailes;
            Trans.Vatamount = Trn.Vatamount;
            Trans.Note = Trn.Note;
            Trans.Proid = Trn.Proid;
            Trans.Prtid = Trn.Prtid;
            _IUW.TblTransactions.Insert(Trans);
            _IUW.Complete();
            ViewBag.Date = DateTime.Now.ToString("yyyy-MM-dd");
            return Json("Ok");
        }
        [HttpGet]
        public ActionResult Trnedit(int id)
        {
            ViewBag.PartnerList = _IUW.Partners.GetAll()
.Select(x => new SelectListItem
{
    Value = x.Id.ToString(),   // أو الـ Prtid لو تحب
    Text = x.Partnername
}).ToList();
            ViewBag.Pro = _IUW.Projects.GetAll().Where(x => x.Status != "Close")
    .Select(x => new SelectListItem
    {
        Value = x.Id.ToString(),   // أو الـ Prtid لو تحب
        Text = x.Projectname
    }).ToList();
            var NTrn = _IUW.TblTransactions.Get(id);
            if (NTrn != null)
            {
                Transactionmodel NTM = new Transactionmodel();
                NTM.Id = NTrn.Id;
                NTM.Creditor = NTrn.Creditor;
                NTM.Debitor = NTrn.Debitor;
                NTM.Tdate = NTrn.Tdate?.ToString("yyyy-MM-dd");
                NTM.Detailes = NTrn.Detailes;
                NTM.Note = NTrn.Note;
                NTM.Vatamount = NTrn.Vatamount ?? 0;
                NTM.Proid = NTrn.Proid;
                NTM.Prtid = NTrn.Prtid;
                return View(NTM);
            }
            return View();
        }
        [HttpPost]
        public ActionResult Trnedit(TblTransaction Trn)
        {
            TblTransaction Trans = new TblTransaction();
            Trans.Id = Trn.Id;
            Trans.Creditor = Trn.Creditor;
            Trans.Debitor = Trn.Debitor;
            Trans.Tdate = Trn.Tdate;
            Trans.Detailes = Trn.Detailes;
            Trans.Vatamount = Trn.Vatamount;
            Trans.Note = Trn.Note;
            Trans.Proid = Trn.Proid;
            Trans.Prtid = Trn.Prtid;
            _IUW.TblTransactions.Update(Trans);
            _IUW.Complete();
            Oprationsviewmodel OC = new Oprationsviewmodel(_IUW);
            var Uid = int.Parse(HttpContext.Request.Cookies["Id"]);
            OC.Edit(Trn.Id, "Tbl_Transaction", Uid);
            return Json("Ok");

        }
        public IActionResult UpdateMultiple(List<int> ids) // أو List<string> لو كان الـ ID نوعه string
        {
            if (ids != null && ids.Any())
            {
                foreach (var id in ids)
                {
                    // هات الـ Model من قاعدة البيانات بناءً على الـ ID
                    var modelToUpdate = _IUW.TblTransactions.Find(x => x.Id == id); // استبدل YourTable باسم الجدول الفعلي

                    if (modelToUpdate != null)
                    {
                        if (modelToUpdate.Creditor != 0)
                        {
                            var AV = Convert.ToDouble(modelToUpdate.Creditor) - (Convert.ToDouble(modelToUpdate.Creditor) / 1.15);
                            var GAV = Math.Round(AV, 2);
                            modelToUpdate.Vatamount = decimal.Parse(GAV.ToString());
                        }
                        if (modelToUpdate.Debitor != 0)
                        {
                            var AV = Convert.ToDouble(modelToUpdate.Debitor) - (Convert.ToDouble(modelToUpdate.Debitor) / 1.15);
                            var GAV = Math.Round(AV, 2);
                            modelToUpdate.Vatamount = decimal.Parse(GAV.ToString());
                        }
                        // قم بتعديل الـ properties بتاعة الـ modelToUpdate زي ما عايز
                        // _context.SaveChanges(); // احفظ التغييرات في قاعدة البيانات لكل تعديل أو بعد اللوب
                    }
                    _IUW.TblTransactions.Update(modelToUpdate);
                    _IUW.Complete();
                    Oprationsviewmodel OC = new Oprationsviewmodel(_IUW);
                    var Uid = int.Parse(HttpContext.Request.Cookies["Id"]);
                    var mid = modelToUpdate.Id;
                    OC.Edit(modelToUpdate.Id, "Tbl_Transaction", Uid);

                }
                _IUW.Dispose();
                return Json(new { success = true, message = "تم تحديث العناصر المحددة بنجاح" }); // أرجع رد للـ AJAX
            }
            else
            {
                return Json(new { success = false, message = "لم يتم تحديد أي عناصر" });
            }
        }
        //Trndetailes
        [HttpGet]
        public ActionResult Trndetailes(int id)
        {
            var Trnlist = _IUW.Projects.GetAll();
            List<SelectListItem> SLI = new List<SelectListItem>();
            foreach (var Trn in Trnlist)
            {
                SelectListItem item = new SelectListItem();
                item.Value = Trn.Id.ToString();
                item.Text = Trn.Projectname;
                SLI.Add(item);
            }
            ViewBag.Pro = SLI;
            var NTrn = _IUW.TblTransactions.Get(id);
            if (NTrn != null)
            {
                Transactionmodel NTM = new Transactionmodel();
                NTM.Id = NTrn.Id;
                NTM.Creditor = NTrn.Creditor;
                NTM.Debitor = NTrn.Debitor;
                NTM.Tdate = NTrn.Tdate?.ToString("yyyy-MM-dd");
                NTM.Detailes = NTrn.Detailes;
                NTM.Vatamount = NTrn.Vatamount ?? 0;
                NTM.Note = NTrn.Note;
                NTM.Proid = NTrn.Proid;
                return View(NTM);
            }
            return View();
        }
        public IActionResult Trndete(Transactionmodel Trn)
        {
            TblTransaction MPro = new TblTransaction();
            MPro.Id = Trn.Id;
            _IUW.TblTransactions.Delbyid(MPro.Id);
            _IUW.Complete();
            Oprationsviewmodel OC = new Oprationsviewmodel(_IUW);
            OC.Delete(Trn.Id, "Tbl_Transaction");
            return Json("Ok");
        }
        [HttpPost]
        public IActionResult Exportdata([FromBody] List<Transactionmodel> transactionmodel)
        {
            using (var workbook = new XLWorkbook())
            {
                var Proname = transactionmodel.FirstOrDefault();
                var worksheet = workbook.Worksheets.Add("تقرير الحركات"); // يمكنك تغيير اسم ورقة العمل

                // إضافة ترويسات الأعمدة (Headers)
                worksheet.Cell(1, 1).Value = "رقم الحركه";
                worksheet.Cell(1, 2).Value = "الدائن";
                worksheet.Cell(1, 3).Value = "المدين";
                worksheet.Cell(1, 4).Value = "الضريبه";
                worksheet.Cell(1, 5).Value = "اسم المشروع";
                worksheet.Cell(1, 6).Value = "التفاصيل";
                worksheet.Cell(1, 8).Value = "المستلم";
                worksheet.Cell(1, 7).Value = "التاريخ";
                worksheet.Cell(1, 8).Value = "ملاحظات";
                // ... أضف المزيد من الترويسات حسب الحاجة

                // إضافة البيانات
                for (int i = 0; i < transactionmodel.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = transactionmodel[i].Id;
                    worksheet.Cell(i + 2, 2).Value = transactionmodel[i].Creditor;
                    worksheet.Cell(i + 2, 3).Value = transactionmodel[i].Debitor;
                    worksheet.Cell(i + 2, 4).Value = transactionmodel[i].Vatamount;
                    worksheet.Cell(i + 2, 5).Value = transactionmodel[i].Projectname;
                    worksheet.Cell(i + 2, 6).Value = transactionmodel[i].Detailes;
                    worksheet.Cell(i + 2, 8).Value = transactionmodel[i].Prtname;
                    worksheet.Cell(i + 2, 7).Value = transactionmodel[i].Tdate;
                    worksheet.Cell(i + 2, 8).Value = transactionmodel[i].Note;
                }

                // حفظ المصنف في الذاكرة كـ MemoryStream
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        Proname.Projectname + ".xlsx");
                }
            }
        }
        public IActionResult Alltrn()
        {
            var Prolist = _IUW.Projects.GetAll();
            var Prt = _IUW.Partners.GetAll();
            List<SelectListItem> SLI = new List<SelectListItem>();
            foreach (var Pro in Prolist)
            {
                SelectListItem item = new SelectListItem();
                item.Value = Pro.Id.ToString();
                item.Text = Pro.Projectname;
                SLI.Add(item);
            }
            ViewBag.Pro = SLI;
            //var Trnlist = _IUW.TblTransactions.GetAllwithmaster("Pro").Select(x => new
            //{
            //    x.Id,
            //    x.Creditor,
            //    x.Debitor,
            //    x.Tdate,
            //    x.Detailes,
            //    x.Vatamount,
            //    x.Note,
            //    x.Pro?.Projectname,
            //    x.Pro,
            //    x.Prtid,
            //}).OrderByDescending(x => x.Id);
            //List<Transactionmodel> LTM = new List<Transactionmodel>();
            //foreach (var Trn in Trnlist)
            //{
            //    var Date = Convert.ToDateTime(Trn.Tdate?.ToString()).ToString("yyyy-MM-dd");
            //    var Prtname = "";
            //    if (Trn.Prtid != null)
            //    {
            //        var GP = Prt.Where(x => Trn.Prtid == x.Id).FirstOrDefault();
            //        Prtname = GP?.Partnername;
            //    }
            //    LTM.Add(new Transactionmodel
            //    {
            //        Id = Trn.Id,
            //        Projectname = Trn.Projectname,
            //        Creditor = Trn.Creditor,
            //        Debitor = Trn.Debitor,
            //        Balance = Trn.Creditor - Trn.Debitor,
            //        Detailes = Trn.Detailes,
            //        Vatamount = Trn.Vatamount ?? 0,
            //        Note = Trn.Note,
            //        Tdate = Date,
            //        Proid = Trn.Pro?.Id,
            //        Prtname = Prtname,
            //    });
            //}
            var result = _IUW.TblTransactions.GetQueryable()
.Include(t => t.Pro)
.Include(t => t.Prt)
.OrderByDescending(t => t.Tdate)
.Select(t => new Transactionmodel
{
Id = t.Id,
Creditor = t.Creditor,
Debitor = t.Debitor,
Vatamount = t.Vatamount ?? 0,
Projectname = t.Pro != null ? t.Pro.Projectname : null,
    Detailes = t.Detailes,
    Tdate = t.Tdate.ToString(),
Prtname = t.Prt != null ? t.Prt.Partnername : null,
    Proid = t.Proid,
    Note = t.Note
})
.ToList();
            return View("Index", result);
        }
        public IActionResult last100()
        {
            var Prolist = _IUW.Projects.GetAll();
            var Prt = _IUW.Partners.GetAll();
            List<SelectListItem> SLI = new List<SelectListItem>();
            foreach (var Pro in Prolist)
            {
                SelectListItem item = new SelectListItem();
                item.Value = Pro.Id.ToString();
                item.Text = Pro.Projectname;
                SLI.Add(item);
            }
            ViewBag.Pro = SLI;

            //var Trnlist = _IUW.TblTransactions.GetAllwithmaster("Pro").Select(x => new
            //{
            //    x.Id,
            //    x.Creditor,
            //    x.Debitor,
            //    x.Tdate,
            //    x.Detailes,
            //    x.Vatamount,
            //    x.Note,
            //    x.Pro?.Projectname,
            //    x.Pro,
            //    x.Prtid,
            //}).OrderByDescending(x => x.Id);
            //List<Transactionmodel> LTM = new List<Transactionmodel>();
            //foreach (var Trn in Trnlist)
            //{
            //    var Date = Convert.ToDateTime(Trn.Tdate?.ToString()).ToString("yyyy-MM-dd");
            //    var Prtname = "";
            //    if (Trn.Prtid != null)
            //    {
            //        var GP = Prt.Where(x => Trn.Prtid == x.Id).FirstOrDefault();
            //        Prtname = GP?.Partnername;
            //    }
            //    LTM.Add(new Transactionmodel
            //    {
            //        Id = Trn.Id,
            //        Projectname = Trn.Projectname,
            //        Creditor = Trn.Creditor,
            //        Debitor = Trn.Debitor,
            //        Balance = Trn.Creditor - Trn.Debitor,
            //        Detailes = Trn.Detailes,
            //        Vatamount = Trn.Vatamount ?? 0,
            //        Note = Trn.Note,
            //        Tdate = Date,
            //        Proid = Trn.Pro?.Id,
            //        Prtname = Prtname,
            //    });
            //}
            //var Data = LTM.Take(100);
            var result = _IUW.TblTransactions.GetQueryable()
.Include(t => t.Pro)
.Include(t => t.Prt)
.OrderByDescending(t => t.Tdate)
.Take(100)
.Select(t => new Transactionmodel
{
Id = t.Id,
Creditor = t.Creditor,
Debitor = t.Debitor,
Vatamount = t.Vatamount ?? 0,
Projectname = t.Pro != null ? t.Pro.Projectname : null,
    Detailes = t.Detailes,
    Tdate = t.Tdate.ToString(),
Prtname = t.Prt != null ? t.Prt.Partnername : null,
    Proid = t.Proid,
    Note = t.Note
})
.ToList();
            return View("Index", result);
        }
        public IActionResult last50()
        {
            var Prolist = _IUW.Projects.GetAll();
            var Prt = _IUW.Partners.GetAll();
            List<SelectListItem> SLI = new List<SelectListItem>();
            foreach (var Pro in Prolist)
            {
                SelectListItem item = new SelectListItem();
                item.Value = Pro.Id.ToString();
                item.Text = Pro.Projectname;
                SLI.Add(item);
            }
            ViewBag.Pro = SLI;


            //var Trnlist = _IUW.TblTransactions.GetAllwithmaster("Pro").Select(x => new
            //{
            //    x.Id,
            //    x.Creditor,
            //    x.Debitor,
            //    x.Tdate,
            //    x.Detailes,
            //    x.Vatamount,
            //    x.Note,
            //    x.Pro?.Projectname,
            //    x.Pro,
            //    x.Prtid,
            //}).OrderByDescending(x => x.Id);
            //List<Transactionmodel> LTM = new List<Transactionmodel>();
            //foreach (var Trn in Trnlist)
            //{
            //    var Date = Convert.ToDateTime(Trn.Tdate?.ToString()).ToString("yyyy-MM-dd");
            //    var Prtname = "";
            //    if (Trn.Prtid != null)
            //    {
            //        var GP = Prt.Where(x => Trn.Prtid == x.Id).FirstOrDefault();
            //        Prtname = GP?.Partnername;
            //    }
            //    LTM.Add(new Transactionmodel
            //    {
            //        Id = Trn.Id,
            //        Projectname = Trn.Projectname,
            //        Creditor = Trn.Creditor,
            //        Debitor = Trn.Debitor,
            //        Balance = Trn.Creditor - Trn.Debitor,
            //        Detailes = Trn.Detailes,
            //        Vatamount = Trn.Vatamount ?? 0,
            //        Note = Trn.Note,
            //        Tdate = Date,
            //        Proid = Trn.Pro?.Id,
            //        Prtname = Prtname,
            //    });
            //}
            //var Data = LTM.Take(50);
            var result = _IUW.TblTransactions.GetQueryable()
    .Include(t => t.Pro)
    .Include(t => t.Prt)
    .OrderByDescending(t => t.Tdate)
    .Take(50)
    .Select(t => new Transactionmodel
    {
        Id = t.Id,
        Creditor = t.Creditor,
        Debitor = t.Debitor,
        Vatamount = t.Vatamount ?? 0,
        Projectname = t.Pro != null ? t.Pro.Projectname : null,
        Detailes = t.Detailes,
        Tdate = t.Tdate.ToString(),
        Prtname = t.Prt != null ? t.Prt.Partnername : null,
        Proid = t.Proid,
        Note = t.Note
    })
    .ToList();
            return View("Index",result);
        }
        public IActionResult last10()
        {
            var Prolist = _IUW.Projects.GetAll();
            var Prt = _IUW.Partners.GetAll();
            List<SelectListItem> SLI = new List<SelectListItem>();
            foreach (var Pro in Prolist)
            {
                SelectListItem item = new SelectListItem();
                item.Value = Pro.Id.ToString();
                item.Text = Pro.Projectname;
                SLI.Add(item);
            }
            ViewBag.Pro = SLI;

            var result = _IUW.TblTransactions.GetQueryable()
                .Include(t => t.Pro)
                .Include(t => t.Prt)
                .OrderByDescending(t => t.Tdate)
                .Take(10)
                .Select(t => new Transactionmodel
                {
                    Id = t.Id,
                    Creditor = t.Creditor,
                    Debitor = t.Debitor,
                    Vatamount = t.Vatamount ?? 0,
                    Projectname = t.Pro != null ? t.Pro.Projectname : null,
                    Detailes = t.Detailes,
                    Tdate = t.Tdate.ToString(),
                    Prtname = t.Prt != null ? t.Prt.Partnername : null,
                    Proid = t.Proid,
                    Note = t.Note
                })
                .ToList();
            //var Trnlist = _IUW.TblTransactions.GetAllwithmaster("Pro").Select(x => new
            //{
            //    x.Id,
            //    x.Creditor,
            //    x.Debitor,
            //    x.Tdate,
            //    x.Detailes,
            //    x.Vatamount,
            //    x.Note,
            //    x.Pro?.Projectname,
            //    x.Pro,
            //    x.Prtid,
            //}).OrderByDescending(x => x.Id);
            //List<Transactionmodel> LTM = new List<Transactionmodel>();
            //foreach (var Trn in Trnlist)
            //{
            //    var Date = Convert.ToDateTime(Trn.Tdate?.ToString()).ToString("yyyy-MM-dd");
            //    var Prtname = "";
            //    if (Trn.Prtid != null)
            //    {
            //        var GP = Prt.Where(x => Trn.Prtid == x.Id).FirstOrDefault();
            //        Prtname = GP?.Partnername;
            //    }
            //    LTM.Add(new Transactionmodel
            //    {
            //        Id = Trn.Id,
            //        Projectname = Trn.Projectname,
            //        Creditor = Trn.Creditor,
            //        Debitor = Trn.Debitor,
            //        Balance = Trn.Creditor - Trn.Debitor,
            //        Detailes = Trn.Detailes,
            //        Vatamount = Trn.Vatamount ?? 0,
            //        Note = Trn.Note,
            //        Tdate = Date,
            //        Proid = Trn.Pro?.Id,
            //        Prtname = Prtname,
            //    });
            //}
            //var Data = LTM.Take(10);
            return View("Index", result);
        }
    }
}
