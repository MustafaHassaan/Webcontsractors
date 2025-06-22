using Domain.Models;
using Repo.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webcontsractors.Domain.Models;

namespace Repo.Unitofwork
{
    public interface IUnitofwork : IDisposable
    {
        IRepository<Opration> Oprations { get; }
        IRepository<Partner> Partners { get; }
        IRepository<Payment> Payments { get; }
        IRepository<Project> Projects { get; }
        IRepository<TblTransaction> TblTransactions { get; }
        IRepository<User> Users { get; }
        IRepository<Permission> Permissions { get; }
        IRepository<Userpermission> Userpermissions { get; }
        IRepository<Menu> Menus { get; }
        IRepository<Submenu> Submenus { get; }
        IRepository<Page> Pages { get; }
        int Complete();
    }
}
