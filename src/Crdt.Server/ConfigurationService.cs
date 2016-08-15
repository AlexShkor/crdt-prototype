using Crdt.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Crdt.Server
{
    public static class ConfigurationService
    {
        public static IServiceCollection AddConfiguration(this IServiceCollection services){
            var configuration = new Configuaration();

            services.AddSingleton(configuration);
            return services;
        }
    }
}