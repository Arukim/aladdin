using System.Threading.Tasks;
using Aladdin.DAL.Interfaces;
using Aladdin.DAL.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Aladdin.DAL.Providers
{
    internal class GameDataProvider : AbstractDataProvider<GameEntity>, IGameDataProvider 
    {
        
    }
}