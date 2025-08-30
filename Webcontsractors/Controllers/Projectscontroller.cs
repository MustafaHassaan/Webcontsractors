using DocumentFormat.OpenXml.Office2010.Excel;
using Humanizer;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Repo.Unitofwork;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Webcontsractors.Domain.Models;
using Webcontsractors.Models;

namespace Webcontsractors.Controllers
{
    public class Projectscontroller : Rolecontroller
    {
        IUnitofwork _IUW;
        public Projectscontroller(IUnitofwork IUW)
        {
            _IUW = IUW;
        }
        public ActionResult Index()
        {
            var Prt = _IUW.Partners.GetAll();
            var Pro = _IUW.Projects.GetAll();
            var Trn = _IUW.TblTransactions.GetAll();
            var QSOuterJoin = from Pr in Pro //Left Data Source
                              join Tr in Trn//Right Data Source
                              on Pr.Id equals Tr.Proid //Inner Join Condition
                              into Transgroup //Performing LINQ Group Join
                              from Pros in Transgroup.DefaultIfEmpty() //Performing Left Outer Join
                              select new { 
                                  Proid = Pr.Id,
                                  Prtname = Pr.Prt?.Partnername,
                                  Proname = Pr.Projectname,
                                  Amount = Pr.Amount,
                                  Amountvat = Pr.Amountvat,
                                  Opningbalance = Pr.Opningbalance,
                                  Cred = Math.Round(Transgroup.Sum(T => Convert.ToDouble(T.Creditor)),2),
                                  Dipt = Math.Round(Transgroup.Sum(T => Convert.ToDouble(T.Debitor)),2),

                                  //Balance = Math.Round(Transgroup.Sum(T => Convert.ToDouble(T.Creditor)) -
                                  //                     Transgroup.Sum(T => Convert.ToDouble(T.Debitor)) -
                                  //                     Transgroup.Where(T => Convert.ToDouble(T.Creditor) > 0).Sum(T => Convert.ToDouble(T.Vatamount)) +
                                  //                     Transgroup.Where(T => Convert.ToDouble(T.Debitor) > 0).Sum(T => Convert.ToDouble(T.Vatamount)), 2),
                                  Balance = Math.Round(Transgroup.Sum(T => Convert.ToDouble(T.Creditor)) -
                                                       Transgroup.Sum(T => Convert.ToDouble(T.Debitor))),
                                  Note = Pr.Note,
                                  Status = Pr.Status,
                              }; //Projecting the Result to 
            var data = QSOuterJoin.GroupBy(item => item.Proid,
              (key, group) => new { Project = key, Items = group.FirstOrDefault() }).ToList();
            List<Projectmodel> LPM = new List<Projectmodel>();
            foreach (var Protrn in data)
            {
                var Proid = "0";
                if (Protrn.Items.Proid != null)
                {
                    Proid = Protrn.Items.Proid.ToString();
                }
                LPM.Add(new Projectmodel
                {
                    Id = int.Parse(Proid.ToString()),
                    Projectname = Protrn.Items.Proname?.ToString(),
                    Amount = Protrn.Items.Amount,
                    Amountvat = Protrn.Items.Amountvat,
                    Opningbalance = Protrn.Items.Opningbalance,
                    Balance = decimal.Parse(Protrn.Items.Balance.ToString()),
                    Partnername = Protrn.Items.Prtname,
                    Note = Protrn.Items.Note,
                    Status = Protrn.Items.Status,
                });
            }
            return View(LPM);
        }
        [HttpGet]
        public IActionResult Prosave()
        {
            var Prtlist = _IUW.Partners.GetAll();
            List<SelectListItem> SLI = new List<SelectListItem>();
            foreach (var Prt in Prtlist)
            {
                SelectListItem item = new SelectListItem();
                item.Value = Prt.Id.ToString();
                item.Text = Prt.Partnername;
                SLI.Add(item);
            }
            ViewBag.Prt = SLI;
            ViewBag.Date = DateTime.Now.ToString("yyyy-MM-dd");
            return View();
        }
        [HttpPost]
        public ActionResult Prosave(Projectmodel Pro, string STxt)
        {
            var FN = _IUW.Projects.GetAll().Where(x => x.Projectname == Pro.Projectname).ToList();
            if (FN.Count == 0)
            {
                Project NPro = new Project();
                NPro.Projectname = Pro.Projectname;
                NPro.Amount = Pro.Amount;
                NPro.Amountvat = Pro.Amountvat;
                NPro.Opningbalance = Pro.Opningbalance;
                NPro.Prtid = Pro.Prtid;
                NPro.Tdate = Pro.Tdate;
                NPro.Note = Pro.Note;
                NPro.Status = Pro.Status;
                _IUW.Projects.Insert(NPro);
                _IUW.Complete();
                Oprationsviewmodel OC = new Oprationsviewmodel(_IUW);
                var Uid = int.Parse(HttpContext.Request.Cookies["Id"]);
                OC.AddProjects(Uid);
                if (STxt == "Saveadd")
                {
                    return Json("Ok");
                }
                else
                {
                    return Redirect("Index");
                }
            }
            else
            {
                return Redirect("Index");
            }
            
        }
        [HttpGet]
        public ActionResult Proedit(int id)
        {
            var Prtlist = _IUW.Partners.GetAll();
            List<SelectListItem> SLI = new List<SelectListItem>();
            foreach (var Prt in Prtlist)
            {
                SelectListItem item = new SelectListItem();
                item.Value = Prt.Id.ToString();
                item.Text = Prt.Partnername;
                SLI.Add(item);
            }
            ViewBag.Prt = SLI;
            var Pro = _IUW.Projects.Get(id);
            if (Pro != null) {
                var Blcdata = _IUW.TblTransactions.GetAll().GroupBy(t => t.Proid == id).Select(t => new {
                    Blc = Math.Round(t.Sum(d => Convert.ToDouble(d.Creditor)) - t.Sum(d => Convert.ToDouble(d.Debitor)), 2),
                }).FirstOrDefault();
                Projectmodel NPro = new Projectmodel();
                ViewBag.Status = Pro.Status;
                if (Pro.Status != null && Pro.Status != "Open") {
                    NPro.Profits = Convert.ToDouble(Blcdata.Blc);
                }
                else
                {
                    NPro.Profits = 0.00;
                }
                NPro.Projectname = Pro.Projectname;
                NPro.Amount = Pro.Amount;
                NPro.Amountvat = Pro.Amountvat;
                NPro.Opningbalance = Pro.Opningbalance;
                NPro.Prtid = Pro.Prtid;
                if (Pro.Tdate == null)
                {
                    ViewBag.Date = DateTime.Now.ToString("yyyy-MM-dd");
                }
                else
                {
                    NPro.Tdate = Pro.Tdate;
                    ViewBag.Date = NPro.Tdate.Value.ToString("yyyy-MM-dd");
                }
                NPro.Note = Pro.Note;
                return View(NPro);
            }
            return View();
        }
        [HttpPost]
        public IActionResult Proedit(Projectmodel Pro)
        {
            Project NPro = new Project();
            NPro.Id = Pro.Id;
            NPro.Projectname = Pro.Projectname;
            NPro.Amount = Pro.Amount;
            NPro.Amountvat = Pro.Amountvat;
            NPro.Opningbalance = Pro.Opningbalance;
            NPro.Prtid = Pro.Prtid;
            NPro.Tdate = Pro.Tdate;
            NPro.Note = Pro.Note;
            NPro.Status = Pro.Status;
            _IUW.Projects.Update(NPro);
            _IUW.Complete();
            Oprationsviewmodel OC = new Oprationsviewmodel(_IUW);
            var Uid = int.Parse(HttpContext.Request.Cookies["Id"]);
            OC.Edit(Pro.Id, "Project",Uid);
            //return View();
            return Json("Ok");
        }
        public IActionResult Prodetailes(int id)
        {
            var Prtlist = _IUW.Partners.GetAll();
            List<SelectListItem> SLI = new List<SelectListItem>();
            foreach (var Prt in Prtlist)
            {
                SelectListItem item = new SelectListItem();
                item.Value = Prt.Id.ToString();
                item.Text = Prt.Partnername;
                SLI.Add(item);
            }
            ViewBag.Prt = SLI;
            var Trn = _IUW.TblTransactions.GetAllwithmaster("Pro").Where(x => x.Proid == id);
            var TL = Trn.ToList();
            if (TL.Count > 0)
            {
                var TrnPro = Trn.Select(x => x.Pro).FirstOrDefault();
                Projectmodel NPro = new Projectmodel();
                NPro.Projectname = TrnPro.Projectname;
                NPro.Amount = TrnPro.Amount;
                NPro.Amountvat = TrnPro.Amountvat;
                NPro.Prtid = TrnPro.Prtid;
                var totalBalance = Math.Round(
                    TL.Sum(t => Convert.ToDouble(t.Creditor)) -
                    TL.Sum(t => Convert.ToDouble(t.Debitor)) -
                    TL.Where(t => Convert.ToDouble(t.Creditor) > 0).Sum(t => Convert.ToDouble(t.Vatamount)) +
                    TL.Where(t => Convert.ToDouble(t.Debitor) > 0).Sum(t => Convert.ToDouble(t.Vatamount)),
                    2
                );
                NPro.Balance = decimal.Parse(totalBalance.ToString());
                NPro.Partnername = TrnPro.Prt?.Partnername;
                if (TrnPro.Tdate == null)
                {
                    ViewBag.Date = DateTime.Now.ToString("yyyy-MM-dd");
                }
                else
                {
                    ViewBag.Date = TrnPro.Tdate?.ToString("yyyy-MM-dd");
                }
                NPro.Note = TrnPro.Note;
                NPro.Status = TrnPro.Status;
                foreach (var Trans in Trn)
                {
                    NPro.Transactions.Add(new TblTransaction
                    {
                        Id = Trans.Id,
                        Creditor = Trans.Creditor,
                        Debitor = Trans.Debitor,
                        Tdate = Trans.Tdate,
                        Detailes = Trans.Detailes,
                        Vatamount = Trans.Vatamount,
                        Note = Trans.Note
                    });
                }
                return View(NPro);
            }
            else
            {
                Projectmodel NPro = new Projectmodel();
                var Pro = _IUW.Projects.GetAllwithmaster("Prt").Where(x => x.Id == id).First();
                NPro.Projectname = Pro.Projectname;
                NPro.Amount = Pro.Amount;
                NPro.Amountvat = Pro.Amountvat;
                NPro.Prtid = Pro.Prtid;
                NPro.Partnername = Pro.Prt?.Partnername;
                NPro.Note = Pro.Note;
                NPro.Balance = Pro.Opningbalance;
                if (NPro.Tdate == null)
                {
                    ViewBag.Date = DateTime.Now.ToString("yyyy-MM-dd");
                }
                else
                {
                    ViewBag.Date = NPro.Tdate?.ToString("yyyy-MM-dd");
                }
                NPro.Status = Pro.Status;
                return View(NPro);
            }
        }
        public IActionResult Prodete(Projectmodel Pro)
        {
            Project MPro = new Project();
            MPro.Id = Pro.Id;
            _IUW.Projects.Delbyid(MPro.Id);
            _IUW.Complete();
            Oprationsviewmodel OC = new Oprationsviewmodel(_IUW);
            OC.Delete(Pro.Id, "Project");
            return Json("Ok");
        }
        [HttpGet]
        public JsonResult CalcProfitJson(int id)
        {
            var Blcdata = _IUW.TblTransactions.GetAll().Where(x => x.Proid == id).GroupBy(t => t.Proid == id).Select(t => new
            {
                Blc = Math.Round(t.Sum(d => Convert.ToDouble(d.Creditor)) - t.Sum(d => Convert.ToDouble(d.Debitor)), 2),
            }).FirstOrDefault();
            if (Blcdata != null) {
            return Json(Blcdata.Blc.ToString());
            }
            else
            {
                return Json(Blcdata?.Blc.ToString());
            }

        }
    }
}
