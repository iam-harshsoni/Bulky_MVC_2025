using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HarshBulky.DataAccess.Data;
using HarshBulky.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace HarshBulky.DataAccess.Repository
{
    internal class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;

        internal DbSet<T> dbset;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbset = _db.Set<T>();
            
            // _db.Set<Product>();
            // _db.Set<Category>();
            // _db.Categories = dbSet.
        }

        public void Add(T entity)
        {
            dbset.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbset;
            query = query.Where(filter);
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll()
        {
            /*
                Difference betweewn IQueryable and IEnumerable

                - IEnumerable fetches all data from the database first and then applies filters in memory (client-side).
                - Best for in-memory data collection:
                - Ideal when working with local collections like List, Array, etc., not databases
            
            
                - IQueryable 
                - Executes queries in the database:
                - Filters are converted into SQL and executed at the database level (server-side), which improves performance for large datasets.
                - Best for remote data sources (e.g., Entity Framework):
                - Suitable when querying from a database as it allows LINQ-to-SQL translation
             */

            IQueryable<T> query = dbset;
            return query.ToList();
        }

        public void Remove(T entity)
        {
            dbset.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbset.RemoveRange(entities);
        }
    }
}
