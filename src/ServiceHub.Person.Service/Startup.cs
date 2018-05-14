using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CM = ServiceHub.Person.Context.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceHub.Person.Library.Models;

namespace ServiceHub.Person.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<CM.PersonRepository>();
            //add and configure Di with setting class, get connection string and database name from appsettings.json
            //settings will be access via IOptions<Settings>
            services.Configure<Settings>(Options =>
            {
                Options.ConnectionString = Configuration.GetSection("MongoDB:ConnectionString").Value;
                Options.Database = Configuration.GetSection("MongoDB:Database").Value;
                Options.CollectionName = Configuration.GetSection("MongoDB:Collection").Value;
                Options.MetaDataCollectionName = Configuration.GetSection("MongoDB:MetaDataCollection").Value;;
                Options.MetaDataCollectionName = Configuration.GetSection("MongoDB:MetaDataId").Value;;
                Options.BaseURL = Configuration.GetSection("SalesforceURLs:Base").Value;
                Options.GetAll = Configuration.GetSection("SalesforceURLs:GetAll").Value;
                Options.CacheExpirationMinutes = int.Parse(Configuration.GetSection("CacheExpirationMinutes").Value);            
            });
            services.AddMvc();

            services.AddCors(o => o.AddPolicy("Open", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials();
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("Open");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
