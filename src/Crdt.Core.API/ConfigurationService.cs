using Crdt.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Crdt.Core.API
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