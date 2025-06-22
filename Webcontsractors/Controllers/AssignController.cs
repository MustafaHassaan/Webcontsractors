using DocumentFormat.OpenXml.InkML;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Repo.Unitofwork;
using Webcontsractors.Models;

namespace Webcontsractors.Controllers
{
    public class AssignController : Controller
    {
        IUnitofwork _IUW;
        public AssignController(IUnitofwork IUW)
        {
            _IUW = IUW;
        }
        public IActionResult AssignPermissions(int userId)
        {
            // تحميل المستخدمين للدروب داون
            ViewBag.user = _IUW.Users.GetAll()
                            .Select(x => new SelectListItem
                            {
                                Value = x.Id.ToString(),
                                Text = x.Username
                            }).ToList();

            // الحصول على كل الصلاحيات الخاصة بالمستخدم
            var userPermissions = _IUW.Userpermissions?.GetAll()?
                .Where(up => up.Userid == userId)
                .ToList() ?? new List<Userpermission>();

            // صلاحيات الصفحات المحددة فعليًا
            var userPermissionsWithPages = userPermissions
                .Where(up => up.Permissionid != null)
                .Select(up => new PermissionSelection
                {
                    PermissionId = up.Permissionid.Value,
                    PageId = up.Pageid
                })
                .ToList();

            // الصفحات التي تم اختيارها بدون صلاحيات (Checkbox الرئيسي فقط)
            var pagesOnlySelections = userPermissions
                .Where(up => up.Permissionid == null)
                .Select(up => up.Pageid.ToString())
                .ToList();

            // تحميل المينيوهات وكل التفرعات التابعة
            var menus = _IUW.Menus
                .GetQueryable()
                .Include(m => m.Submenus)
                    .ThenInclude(s => s.Pages)
                    .ThenInclude(p => p.Permissions)
                .ToList();

            // تجهيز ViewModel
            var model = new PermissionViewModel
            {
                UserId = userId.ToString(),
                PageMasterSelections = pagesOnlySelections,
                Menus = menus.Select(menu => new MenuPermissionViewModel
                {
                    MenuId = menu.Id,
                    MenuName = menu.Name,
                    SubMenus = menu.Submenus.Select(sub => new SubMenuPermissionViewModel
                    {
                        SubMenuId = sub.Id,
                        SubMenuName = sub.Name,
                        Pages = sub.Pages.Select(page => new PagePermissionViewModel
                        {
                            PageId = page.Id,
                            PageName = page.Title,
                            Permissions = page.Permissions.Select(permission => new PermissionItemViewModel
                            {
                                PermissionId = permission.Id,
                                Description = permission.PermissionType,
                                IsChecked = userPermissionsWithPages.Any(p =>
                                    p.PermissionId == permission.Id && p.PageId == page.Id)
                            }).ToList()
                        }).ToList()
                    }).ToList()
                }).ToList()
            };

            return View(model);
        }

        //public IActionResult AssignPermissions(int userId)
        //{
        //    ViewBag.user = _IUW.Users.GetAll()
        //                    .Select(x => new SelectListItem
        //                    {
        //                        Value = x.Id.ToString(),   // أو الـ Prtid لو تحب
        //                        Text = x.Username
        //                    }).ToList();

        //    var userPermissionsWithPages = _IUW.Userpermissions?.GetAll()?
        //        .Where(up => up.Userid == userId)
        //        .Select(up => new PermissionSelection
        //        {
        //            PermissionId = up.Permissionid ?? 0,
        //            PageId = up.Pageid
        //        })
        //        .ToList() ?? new List<PermissionSelection>();

        //    //userPermissionIds ??= new List<int>();
        //    var menus = _IUW.Menus
        //        .GetQueryable() // خلي بالك لازم ترجّع IQueryable من الريبو
        //        .Include(m => m.Submenus)
        //            .ThenInclude(s => s.Pages)
        //            .ThenInclude(p => p.Permissions)
        //        .ToList();
        //    // 3. تجهيز الموديل
        //    var model = new PermissionViewModel
        //    {
        //        UserId = userId.ToString(),
        //        Menus = menus.Select(menu => new MenuPermissionViewModel
        //        {
        //            MenuId = menu.Id,
        //            MenuName = menu.Name,
        //            SubMenus = menu.Submenus.Select(sub => new SubMenuPermissionViewModel
        //            {
        //                SubMenuId = sub.Id,
        //                SubMenuName = sub.Name,
        //                Pages = sub.Pages.Select(page => new PagePermissionViewModel
        //                {
        //                    PageId = page.Id,
        //                    PageName = page.Title,
        //                    Permissions = page.Permissions.Select(permission => new PermissionItemViewModel
        //                    {
        //                        PermissionId = permission.Id,
        //                        Description = permission.PermissionType,
        //                        IsChecked = userPermissionsWithPages.Any(p => p.PermissionId == permission.Id && p.PageId == page.Id)
        //                    }).ToList()
        //                }).ToList()
        //            }).ToList()
        //        }).ToList()
        //    };

        //    return View(model);
        //}


        [HttpPost]
        public IActionResult AssignPermissions(PermissionViewModel model)
        {
            if (string.IsNullOrEmpty(model.UserId))
                return BadRequest("UserId is required.");

            int userId = int.Parse(model.UserId);

            // 1. حذف الصلاحيات القديمة
            var oldPermissions = _IUW.Userpermissions.GetAll().Where(x => x.Userid == userId).ToList();
            foreach (var perm in oldPermissions)
            {
                _IUW.Userpermissions.Delbyid(perm.Id);
            }

            // 2. بناء الصلاحيات الجديدة
            var userPermissions = new List<Userpermission>();

            // أولًا: الصلاحيات المربوطة بصفحات (permissionId|pageId)
            foreach (var item in model.SelectedPermissions)
            {
                var parts = item.Split('|');
                if (parts.Length == Convert.ToInt32("2") &&
                    int.TryParse(parts[0], out int permissionId) &&
                    int.TryParse(parts[1], out int pageId))
                {
                    userPermissions.Add(new Userpermission
                    {
                        Userid = userId,
                        Permissionid = permissionId,
                        Pageid = pageId
                    });
                }
            }

            // ثانيًا: الصفحات اللي متعلم عليها بدون صلاحيات
            foreach (var pageIdStr in model.PageMasterSelections)
            {
                if (int.TryParse(pageIdStr, out int pageId))
                {
                    // لو مفيش صلاحيات مضافة لها فعلًا
                    bool alreadyAdded = userPermissions.Any(p => p.Pageid == pageId);
                    if (!alreadyAdded)
                    {
                        userPermissions.Add(new Userpermission
                        {
                            Userid = userId,
                            Pageid = pageId,
                            Permissionid = null // لو Permissionid يقبل null
                        });
                    }
                }
            }

            // 3. إدخال الصلاحيات الجديدة
            foreach (var perm in userPermissions)
            {
                _IUW.Userpermissions.Insert(perm);
            }

            _IUW.Complete();

            TempData["SuccessMessage"] = "تم حفظ الصلاحيات بنجاح";
            return RedirectToAction("AssignPermissions", new { userId = model.UserId });
        }
    }
}
