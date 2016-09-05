using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Crdt.Core.API.Models;
using Crdt.Core.Messaging;
using Crdt.Core.Storage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Crdt.Core.API
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddConfiguration();
            services.AddAppDependencis();
            services.AddMvc();

            services.AddTransient<IDatabaseService, DatabaseService>();
            services.AddSingleton<IStorage, MemoryStorage>();
            
            services.AddSingleton<IReplicasUpdater, ReplicasUpdater>();

            services.AddSingleton<IReplicaOperationsConsumer, RabbitMqConsumer>();
            services.AddSingleton<IReplicaOperationsSender, RabbitMqSender>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if(env.IsDevelopment()){
                app.UseDeveloperExceptionPage();
            }
            app.UseMvc();
        }
    }
}