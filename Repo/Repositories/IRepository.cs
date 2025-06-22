using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repo.Repositories
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAllwithmaster(string entity);
        T Get(int id);
        void AddRange(IEnumerable<T> entities);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delbyid(int id);
        T Find(Expression<Func<T, bool>> Name);
        IQueryable<T> GetQueryable();
    }
}
