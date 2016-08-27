using Crdt.Core.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Crdt.Core.API
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services){
            services.AddConfiguration();
            services.AddAppDependencis();
            services.AddMvc();            
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var test = new RabbitMqMessageClient();
            loggerFactory.AddConsole();

            if(env.IsDevelopment()){
                app.UseDeveloperExceptionPage();
            }
            app.UseMvc();
        }
    }
}