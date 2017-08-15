using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aladdin.DAL.Interfaces
{
    public interface IRepository<T>
    {
        Task Add(T entity);
        IQueryable<T> GetAll();
    }
}