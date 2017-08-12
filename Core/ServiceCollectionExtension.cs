using Aladdin.Common.Interfaces;
using Aladdin.DAL.Interfaces;
using Aladdin.DAL.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Aladdin.Core
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection RegisterCore(this IServiceCollection collection)
        {
            return collection.AddTransient<ICoach, Coach>();
        }
    }
}