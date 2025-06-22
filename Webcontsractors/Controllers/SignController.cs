using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Repo.Unitofwork;
using System.Security.Claims;
using Webcontsractors.Domain.Models;
using Webcontsractors.Models;
using Domain.Models;
using Newtonsoft.Json;

namespace Webcontsractors.Controllers
{
    public class SignController : Controller
    {
        IUnitofwork _IUW;
        public SignController(IUnitofwork IUW)
        {
            _IUW = IUW;
        }
        public IActionResult Index()
        {
            var Usrlist = _IUW.Users.GetAll();
            List<Usermodel> LUM = new List<Usermodel>();
            foreach (var Usr in Usrlist)
            {
                LUM.Add(new Usermodel
                {
                    Id = Usr.Id,
                    Username = Usr.Username,
                    Password = Usr.Password,
                });
            }
            return View(LUM);
        }
        //public IActionResult Index()
        //{
        //    var Usrlist = _IUW.Users.GetAll();
        //    List<Usermodel> LUM = new List<Usermodel>();
        //    foreach (var Usr in Usrlist)
        //    {
        //        LUM.Add(new Usermodel
        //        {
        //            Id = Usr.Id,
        //            Username = Usr.Username,
        //            Password = Usr.Password,
        //        });
        //    }
        //    return View(LUM);
        //}
        [HttpGet]
        public ActionResult Signin()
        {
            var Username = HttpContext.Request.Cookies["Username"];
            var Password = HttpContext.Request.Cookies["Password"];
            var Permissions = HttpContext.Request.Cookies["Permissions"];
            var Id = HttpContext.Request.Cookies["Id"];
            if (Username != null && Password != null)
            {
                ViewBag.flage = Username;
                return Redirect("/Home/Index");

            }
            else
            {
                return View();
            }
        }


        [HttpPost]
        public ActionResult Signin(Usermodel User)
        {
            var Usr = _IUW.Users.Find(x => x.Username == User.Username && x.Password == User.Password);
            if (Usr == null)
            {
                TempData["error"] = "خطأ بأسم المستخدم او كلمة السر";
                return View();
            }
            else
            {
                HttpContext.Response.Cookies.Append("Username", Usr.Username, new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(30),
                    HttpOnly = true, // Accessible only by the server
                    IsEssential = true // Required for GDPR compliance
                });
                HttpContext.Response.Cookies.Append("Password", Usr.Password, new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(30),
                    HttpOnly = true, // Accessible only by the server
                    IsEssential = true // Required for GDPR compliance
                });
                HttpContext.Response.Cookies.Append("Id", Usr.Id.ToString(), new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(30),
                    HttpOnly = true, // Accessible only by the server
                    IsEssential = true // Required for GDPR compliance
                });
                return Redirect("/Home/Index");
            }
        }
        [HttpGet]
        public ActionResult Signout()
        {
            HttpContext.Response.Cookies.Delete("Id");
            HttpContext.Response.Cookies.Delete("Username");
            HttpContext.Response.Cookies.Delete("Password");
            HttpContext.Response.Cookies.Delete("Permissions");
            return Redirect("Signin");
        }
        public IActionResult Useredit(int id)
        {
            var Usr = _IUW.Users.Get(id);
            Usermodel UM = new Usermodel();
            UM.Id = Usr.Id;
            UM.Username = Usr.Username;
            UM.Password = Usr.Password;
            return View(UM);
        }
        [HttpPost]
        public ActionResult Useredit(Usermodel User)
        {
            User usr = new User();
            usr.Id = User.Id;
            usr.Username = User.Username;
            usr.Password = User.Password;
            _IUW.Users.Update(usr);
            _IUW.Complete();
            return Redirect("/Sign/Index");
        }

    }
}
