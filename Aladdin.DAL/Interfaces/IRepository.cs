using System.Collections.Generic;

namespace Aladdin.DAL.Interfaces
{
    public interface IRepository<T>
    {
        void Add(T entity);
        IEnumerable<T> GetAll();
    }
}