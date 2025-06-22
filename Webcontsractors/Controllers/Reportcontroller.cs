using AspNetCore.Reporting;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2013.Word;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using Repo.Unitofwork;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using Webcontsractors.Models;
using static Azure.Core.HttpHeader;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Webcontsractors.Controllers
{
    public class Reportcontroller : Rolecontroller
    {
        IUnitofwork _IUW;
        public Reportcontroller(IUnitofwork IUW)
        {
            _IUW = IUW;
        }
        public IActionResult Index()
        {
            // جلب البيانات الأساسية للمشاريع والشركاء
            var projects = _IUW.Projects.GetAll();
            var partners = _IUW.Partners.GetAll();
            ViewBag.Pro = projects.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Projectname?.ToString()
            }).ToList();
            ViewBag.PartnerList = partners.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Partnername?.ToString()
            }).ToList();
            return View();
        }
    }
}
