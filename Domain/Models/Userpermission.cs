using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webcontsractors.Domain.Models;

namespace Domain.Models
{
    public class Userpermission
    {
        public int Id { get; set; }
        public int Userid { get; set; }
        public int? Permissionid { get; set; }
        public int Pageid { get; set; }

        public Permission Permissions { get; set; }
        public Page Pages { get; set; }
    }
}
