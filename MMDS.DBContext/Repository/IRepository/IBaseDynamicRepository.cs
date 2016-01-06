using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MMDS.DBContext.Repository.IRepository
{
    public interface IBaseDynamicRepository<T>
        where T : class
    {
        void Add(T item);

        void Delete(T item);

        IQueryable<T> GetAll();

        IQueryable<T> Query(Expression<Func<T, bool>> query);

        IQueryable<T> Query();

        void Save();

        void Dispose();
    }
}
