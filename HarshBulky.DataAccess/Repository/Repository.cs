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
    public class Repository<T> : IRepository<T> where T : class
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
            _db.Products.Include(u => u.Category);
        }

        public void Add(T entity)
        {
            dbset.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbset;
            query = query.Where(filter);

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includProp);
                }
            }

            return query.FirstOrDefault();
        }

        // Category, CoverType
        public IEnumerable<T> GetAll(string? includeProperties = null)
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

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach(var includProp in includeProperties
                    .Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includProp);
                }
            }

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
