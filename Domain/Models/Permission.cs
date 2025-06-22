using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Permission
    {
        public int Id { get; set; }
        public int PageId { get; set; }
        public string PermissionType { get; set; }
        public Page Page { get; set; }
        public ICollection<Userpermission> Userpermissions { get; set; }
    }
}
