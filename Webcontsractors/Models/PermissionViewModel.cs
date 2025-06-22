using Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Webcontsractors.Models
{
    public class PermissionViewModel
    {
        public string? UserId { get; set; }

        // كل عنصر = "permissionId|pageId"
        public List<string> SelectedPermissions { get; set; } = new();

        // الصفحات اللي تم اختيارها كـ Master (Checkbox الرئيسي فقط بدون صلاحيات)
        public List<string> PageMasterSelections { get; set; } = new();
        public List<MenuPermissionViewModel> Menus { get; set; } = new();
    }
}
