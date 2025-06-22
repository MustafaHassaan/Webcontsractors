﻿using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Webcontsractors.Domain.Models;

namespace Repo.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        ConstractorsContext _Dbc;
        public Repository(ConstractorsContext Dbc)
        {
            _Dbc = Dbc;
        }
        public IEnumerable<T> GetAll()
        {
            return _Dbc.Set<T>().ToList();
        }
        public IEnumerable<T> GetAllwithmaster(string entity)
        {
            return _Dbc.Set<T>().Include(entity);
        }
        public T Get(int id)
        {
            return _Dbc.Set<T>().Find(id);
        }
        public void AddRange(IEnumerable<T> entities)
        {
            _Dbc.Set<T>().AddRangeAsync(entities);
        }
        public void Insert(T entity)
        {
            _Dbc.Set<T>().Add(entity);
        }
        public void Update(T entity)
        {
            _Dbc.Set<T>().Update(entity);
        }
        public void Delbyid(int id)
        {
            var Entdel = _Dbc.Set<T>().Find(id);
            _Dbc.Entry(Entdel).State = EntityState.Deleted;
        }
        public void Delete(T entity)
        {
            _Dbc.Set<T>().Attach(entity);
            _Dbc.Entry(entity).State = EntityState.Deleted;
        }
        public T Find(Expression<Func<T, bool>> Name)
        {
            return _Dbc.Set<T>().SingleOrDefault(Name);
        }
        public IQueryable<T> GetQueryable()
        {
            return _Dbc.Set<T>();
        }

    }
}
