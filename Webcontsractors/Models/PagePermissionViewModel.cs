namespace Webcontsractors.Models
{
    public class PagePermissionViewModel
    {
        public int PageId { get; set; }
        public string PageName { get; set; }
        public List<PermissionItemViewModel> Permissions { get; set; }
    }
}
