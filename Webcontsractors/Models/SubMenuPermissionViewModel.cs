namespace Webcontsractors.Models
{
    public class SubMenuPermissionViewModel
    {
        public int SubMenuId { get; set; }
        public string SubMenuName { get; set; }
        public List<PagePermissionViewModel> Pages { get; set; }
    }
}
