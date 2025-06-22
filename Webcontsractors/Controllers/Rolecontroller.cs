using DocumentFormat.OpenXml.Office.CustomUI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Repo.Unitofwork;
using Webcontsractors.Models; // أو المكان اللي فيه Userpermission

namespace Webcontsractors.Controllers
{
    public class Rolecontroller : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var httpContext = context.HttpContext;

            // 1. الحصول على IUnitofwork
            var _UOW = httpContext.RequestServices.GetService(typeof(IUnitofwork)) as IUnitofwork;

            // 2. قراءة Id المستخدم من الكوكيز
            var idStr = httpContext.Request.Cookies["Id"];
            if (!string.IsNullOrEmpty(idStr) && int.TryParse(idStr, out int userId) && _UOW != null)
            {
                // 3. تحميل صلاحيات المستخدم من أكثر من جدول + Submenu
                var query = from up in _UOW.Userpermissions.GetQueryable()
                            where up.Userid == userId
                            join p in _UOW.Permissions.GetQueryable() on up.Permissionid equals p.Id into up_p
                            from perm in up_p.DefaultIfEmpty()
                            join pg in _UOW.Pages.GetQueryable() on up.Pageid equals pg.Id into up_pg
                            from page in up_pg.DefaultIfEmpty()
                            join sm in _UOW.Submenus.GetQueryable() on page.SMId equals sm.Id into pg_sm
                            from submenu in pg_sm.DefaultIfEmpty()
                            select new
                            {
                                Singlpage = perm != null ? perm.PermissionType : null,
                                Submenuame = page != null ? page.Title : null,
                                Menuname = submenu != null ? submenu.Name : null
                            };

                var result = query.ToList();

                // 4. حفظهم في Session
                if (result.Any())
                {
                    string permissionsJson = JsonConvert.SerializeObject(result);
                    httpContext.Session.SetString("UserPermissions", permissionsJson);
                }
            }

            base.OnActionExecuting(context);
        }

    }
}
