using System.Threading.Tasks;
using Aladdin.DAL.Models;

namespace Aladdin.DAL.Interfaces{
    public interface IAccountDataProvider : IRepository<AccountEntity>{
        Task<AccountEntity> GetMainAccount();
    }
}