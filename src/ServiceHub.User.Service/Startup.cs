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
            SeedMockUsers();
        }

        private void SeedMockUsers()
        {
            const string connectionString = @"mongodb://db";
            IMongoCollection<User.Context.Models.User> mc =
                new MongoClient(connectionString)
                    .GetDatabase("userdb")
                    .GetCollection<User.Context.Models.User>("users");

            UserStorage context = new UserStorage(new UserRepository(mc));
            string jsonStr = System.IO.File.ReadAllText(@"C:\Users\tjhay\MockUsers.json");
            List<User.Context.Models.User> users = 
                Deserialize<List<User.Context.Models.User>>(jsonStr);

            foreach (var user in users)
            {
                context.Insert(user);
            }
        }

        // Deserialize JSON string and return object.
        private T Deserialize<T>(string jsonStr)
        {
            T obj = default(T);
            MemoryStream ms = new MemoryStream();
            try
            {
                DataContractJsonSerializer ser =
                    new DataContractJsonSerializer(typeof(T));
                StreamWriter writer = new StreamWriter(ms);
                writer.Write(jsonStr);
                writer.Flush();
                ms.Position = 0;
                obj = (T)ser.ReadObject(ms);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                ms.Close();
            }
            return obj;
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
