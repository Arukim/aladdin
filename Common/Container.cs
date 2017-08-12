
using Microsoft.Extensions.DependencyInjection;

namespace Aladdin.Common
{
    public class Container
    {
        private static ServiceProvider _serviceProvider;

        public static ServiceCollection Create()
        {
            return new ServiceCollection();
        }

        public static void Build(IServiceCollection sc){
            _serviceProvider = sc.BuildServiceProvider();
        }

        public static T GetService<T>()
        {
            return _serviceProvider.GetService<T>();
        }
    }
}