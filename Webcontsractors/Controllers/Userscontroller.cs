using Microsoft.AspNetCore.Mvc;
using Repo.Unitofwork;
using Webcontsractors.Domain.Models;
using Webcontsractors.Models;

namespace Webcontsractors.Controllers
{
    public class Userscontroller : Rolecontroller
    {
        IUnitofwork _IUW;
        public Userscontroller(IUnitofwork IUW)
        {
            _IUW = IUW;
        }
        public ActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Signup(Usermodel User)
        {
            User usr = new User();
            usr.Username = User.Username;
            usr.Password = User.Password;
            User.Username = User.Username;
            _IUW.Users.Insert(usr);
            _IUW.Complete();
            return Json("Ok");
        }
        public IActionResult Userdelete(Usermodel User)
        {
            var UD = _IUW.Userpermissions.GetAll().Where(x => x.Userid == User.Id);
            if (UD == null)
            {
                User usr = new User();
                usr.Id = User.Id;
                _IUW.Users.Delbyid(usr.Id);
                _IUW.Complete();
                return Json("Ok");
            }
            else
            {
                return Json("Not Ok");
            }
        }
    }
}
