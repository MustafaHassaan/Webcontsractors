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
    public class Unitofwork : IUnitofwork
    {
        ConstractorsContext _Dbc;
        public IRepository<Opration> Oprations { get; private set; }
        public IRepository<Partner> Partners { get; private set; }
        public IRepository<Payment> Payments { get; private set; }
        public IRepository<Project> Projects { get; private set; }
        public IRepository<TblTransaction> TblTransactions { get; private set; }
        public IRepository<User> Users { get; private set; }
        public IRepository<Permission> Permissions { get; private set; }
        public IRepository<Userpermission> Userpermissions { get; private set; }
        public IRepository<Menu> Menus { get; private set; }
        public IRepository<Submenu> Submenus { get; private set; }
        public IRepository<Page> Pages { get; private set; }

        public Unitofwork(ConstractorsContext Dbc)
        {
            _Dbc = Dbc;
            Oprations = new Repository<Opration>(_Dbc);
            Partners = new Repository<Partner>(_Dbc);
            Payments = new Repository<Payment>(_Dbc);
            Projects = new Repository<Project>(_Dbc);
            TblTransactions = new Repository<TblTransaction>(_Dbc);
            Users = new Repository<User>(_Dbc);
            Permissions = new Repository<Permission>(_Dbc);
            Menus = new Repository<Menu>(_Dbc);
            Submenus = new Repository<Submenu>(_Dbc);
            Pages = new Repository<Page>(_Dbc);
            Userpermissions = new Repository<Userpermission>(_Dbc);
        }
        public int Complete()
        {
            return _Dbc.SaveChanges();
        }
        public void Dispose()
        {
            GC.Collect();
            _Dbc.Dispose();
        }
    }
}
