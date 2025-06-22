using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Page
    {
        public int Id { get; set; }
        public int SMId { get; set; }
        public string Title { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Route { get; set; }

        public ICollection<Permission> Permissions { get; set; }
        public Submenu Submenus { get; set; }
    }

}
