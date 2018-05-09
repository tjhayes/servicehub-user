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
using Swashbuckle.AspNetCore.Swagger;
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
        public Dictionary<string, string> SalesforceConfig { get; private set; }
        public Dictionary<string, string> SalesforceURLs { get; private set; }

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
                Options.CollectionName = "PersonNew";

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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Revature Housing: Person API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("Open");

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Revature Housing: Person API V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
