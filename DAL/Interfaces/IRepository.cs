using System.Collections.Generic;
using System.Linq;

namespace Aladdin.DAL.Interfaces
{
    public interface IRepository<T>
    {
        void Add(T entity);
        IQueryable<T> GetAll();
    }
}