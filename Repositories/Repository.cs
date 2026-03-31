using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication1.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual IEnumerable<T> GetAll(params string[] includes)
        {
            IQueryable<T> query = _dbSet;
            if (includes != null)
            {
                foreach (var include in includes) query = query.Include(include);
            }
            return query.ToList();
        }

        public virtual T? GetFirstOrDefault(System.Linq.Expressions.Expression<System.Func<T, bool>> filter, params string[] includes)
        {
            IQueryable<T> query = _dbSet;
            if (includes != null)
            {
                foreach (var include in includes) query = query.Include(include);
            }
            return query.FirstOrDefault(filter);
        }

        public virtual IEnumerable<T> Find(System.Linq.Expressions.Expression<System.Func<T, bool>> filter, params string[] includes)
        {
            IQueryable<T> query = _dbSet.Where(filter);
            if (includes != null)
            {
                foreach (var include in includes) query = query.Include(include);
            }
            return query.ToList();
        }

        public virtual T? GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public virtual void Add(T entity)
        {
            _dbSet.Add(entity);
            Save();
        }

        public virtual void Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            Save();
        }

        public virtual void Delete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                Save();
            }
        }

        public virtual void Save()
        {
            _context.SaveChanges();
        }
    }
}
