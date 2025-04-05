using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarshBulky.Models;

namespace HarshBulky.DataAccess.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        /*
            This Interface will implement all the Irepository methods as well as 2 other methods
            - Update method
            - Save Method.
         
         */
        void Update(Category obj);
    }
}
