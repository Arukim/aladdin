using Aladdin.DAL.Interfaces;
using Aladdin.DAL.Providers;
using Microsoft.Extensions.DependencyInjection;  

public static class ServiceCollectionExtension {
    public static IServiceCollection RegisterDAL(this IServiceCollection collection){
        return collection.AddTransient<IAccountDataProvider,AccountDataProvider>()
                        .AddTransient<IGameDataProvider, GameDataProvider>()
                        .AddTransient<IGenomeDataProvider, GenomeDataProvider>();
    }
}