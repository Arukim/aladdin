
using Microsoft.Extensions.DependencyInjection;

namespace Aladdin.Config
{
    public class Config
    {
        private static ServiceProvider _serviceProvider;
        static Config()
        {
            _serviceProvider = new ServiceCollection()
                .RegisterDAL()
                .BuildServiceProvider();
        }

        public static T GetService<T>()
        {
            return _serviceProvider.GetService<T>();
        }
    }
}