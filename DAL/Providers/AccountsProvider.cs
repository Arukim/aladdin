using System.Threading.Tasks;
using Aladdin.DAL.Interfaces;
using Aladdin.DAL.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Aladdin.DAL.Providers
{
    internal class AccountDataProvider : AbstractDataProvider<AccountEntity>, IAccountDataProvider
    {
        public async Task<AccountEntity> GetMainAccount()
        {
            return await Collection.AsQueryable()
                                .Where(x => x.IsBase)
                                .FirstOrDefaultAsync();
        }
    }
}