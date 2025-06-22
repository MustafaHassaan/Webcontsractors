using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Repo.Unitofwork;
using System.Security.Claims;
using Webcontsractors.Domain.Models;
using Webcontsractors.Models;
using System.Diagnostics;

namespace Webcontsractors.Controllers
{
    public class Oprationscontroller : Rolecontroller
    {
        IUnitofwork _IUW;
        public Oprationscontroller(IUnitofwork IUW)
        {
            _IUW = IUW;
        }
        public IActionResult Index()
        {
            var OPRlist = _IUW.Oprations.GetAllwithmaster("Usr").Select(x => new
            {
                x.Id,
                x.Oprationname,
                x.Detailes,
                x.Tblname,
                x.Date,
                x.Time,
                x.Usr.Username,
            });
            List<Oprationmodel> LOM = new List<Oprationmodel> ();
            foreach (var OPR in OPRlist)
            {
                LOM.Add(new Oprationmodel
                {
                    Id = OPR.Id,
                    Oprationname = OPR.Oprationname,
                    Detailes = OPR.Detailes,
                    Tblname = OPR.Tblname,
                    Date = OPR.Date,
                    Time = OPR.Time,
                    Username = OPR.Username,
                });
            }
            return View(LOM);
        }
    }
}
