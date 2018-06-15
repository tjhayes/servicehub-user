using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ServiceHub.User.Context.Repositories;

namespace ServiceHub.User.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            const string connectionString = @"mongodb://db";
            //const string connectionString = @"mongodb://cameron-wags:rp7KMfeoIp0KgM7dMMpnZDF9Cmtde0PIlQAQ9pdrpZZaZdO9Pqt9mk8VXl3upDpp2pyrzajfNvOm2JZtqfOzkQ==@cameron-wags.documents.azure.com:10255/?ssl=true&replicaSet=globaldb";

            services.AddMvc();
            services.AddSingleton<IQueueClient>(qc =>
              new QueueClient(
                Environment.GetEnvironmentVariable("SERVICE_BUS_CONNECTION_STRING"),
                Environment.GetEnvironmentVariable("SERVICE_BUS_QUEUE_NAME")
              )
            );

            services.AddTransient<IUserRepository, UserRepository>();

            services.AddSingleton(mc =>
                new MongoClient(connectionString)
                    .GetDatabase("userdb")
                    .GetCollection<User.Context.Models.User>("users")
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddApplicationInsights(app.ApplicationServices);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
