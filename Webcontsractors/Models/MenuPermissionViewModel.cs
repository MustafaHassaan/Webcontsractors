namespace Webcontsractors.Models
{
    public class MenuPermissionViewModel
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public List<SubMenuPermissionViewModel> SubMenus { get; set; }
    }
}
