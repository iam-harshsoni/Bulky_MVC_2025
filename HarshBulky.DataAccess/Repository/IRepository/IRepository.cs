using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HarshBulky.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        //T - Category

        IEnumerable<T> GetAll();
        T Get(Expression<Func<T, bool>> filter);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

        /*  We will remove this update() and move it to another repository, as sometimes,
           update is more complicated, we might need to update some properties only. 
           So it cant be generic as Category might have some different update logic 
           and Product might have some different update logic. 

           When Category or product implements the Genertic IRepository, at that time, 
           we will implement update and Save method.

        */
        //void Update(T entity); 

    }
}
