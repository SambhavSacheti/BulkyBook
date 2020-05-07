using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using BulkyBook.DataAccess.Repository.IRepository;

namespace BulkyBook.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext dbContext;

        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            dbSet = dbContext.Set<T>();
        }


        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(int id)
        {
            return dbSet.Find(id);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
          string includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var inclueProp in includeProperties.Split(new char[] { ',' },
                StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(inclueProp);
                }
            }
               if(orderBy != null)
               return  orderBy(query)?.ToList() ?? new List<T>();
               else
               return query.ToList<T>();

        }

        public T GetFirstorDefault(Expression<Func<T, bool>> filter = null, string includeProperties = null)
        {
              IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var inclueProp in includeProperties.Split(new char[] { ',' },
                StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(inclueProp);
                }
            }

            return query.FirstOrDefault();
        }


        public void Remove(int id)
        {
          T entity = dbSet.Find(id);
         Remove(entity);
        }

        public void Remove(T entity)
        {
             dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }
    }
}