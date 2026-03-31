using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace WebApplication1.Repositories
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(params string[] includes);
        T? GetFirstOrDefault(Expression<Func<T, bool>> filter, params string[] includes);
        IEnumerable<T> Find(Expression<Func<T, bool>> filter, params string[] includes);
        T? GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
        void Save();
    }
}
