using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Submenu
    {
        public int Id { get; set; }
        public int MenuId { get; set; }
        public string Name { get; set; }

        public Menu Menus { get; set; }
        public ICollection<Page> Pages { get; set; }
    }
}
