using Crdt.Core;
using Crdt.Core.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Crdt.Server
{
    public static class AddDependencyService
    {
        public static IServiceCollection AddAppDependencis(this IServiceCollection services){
            services.AddTransient<IDatabaseService, DatabaseService>();
            services.AddTransient<IStorage, MemoryStorage>();
            return services;
        }
    }
}