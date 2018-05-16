using CM = ServiceHub.Person.Context.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using ServiceHub.Person.Context.Interfaces;
using ServiceHub.Person.Library.Models;
using System;
using System.Collections.Generic;

namespace ServiceHub.Person.Service
{
  public class Startup
  {
    public Startup(IConfiguration configuration, IHostingEnvironment env)
    {
      Configuration = configuration;
      Env = env;
    }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            List<string> strings = new List<string>();
            if (Env.IsStaging())
            {
                strings.Add(Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING"));
                strings.Add(Environment.GetEnvironmentVariable("MONGODB_DATABASE"));
                strings.Add(Environment.GetEnvironmentVariable("MONGODB_COLLECTION"));
                strings.Add(Environment.GetEnvironmentVariable("MONGODB_METADATA_COLLECTION"));
                strings.Add(Environment.GetEnvironmentVariable("MONGODB_METADATA_ID"));
                strings.Add(Environment.GetEnvironmentVariable("SALESFORCE_URLS"));
                strings.Add(Environment.GetEnvironmentVariable("SALESFORCE_GET_BY_ID"));
            }
            else
            {
                strings.Add(Configuration.GetSection("MongoDB:ConnectionString").Value);
                strings.Add(Configuration.GetSection("MongoDB:Database").Value);
                strings.Add(Configuration.GetSection("MongoDB:Collection").Value);
                strings.Add(Configuration.GetSection("MongoDB:MetaDataCollection").Value);
                strings.Add(Configuration.GetSection("MongoDB:MetaDataId").Value);
                strings.Add(Configuration.GetSection("SalesforceURLs:Base").Value);
                strings.Add(Configuration.GetSection("SalesforceURLs:GetById").Value);
            }
            Settings settings = new Settings(strings);
            services.AddSingleton(settings);
            services.AddScoped<IRepository<CM.Person>, CM.PersonRepository>();            
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
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddApplicationInsights(app.ApplicationServices);
            app.UseCors("Open");

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Revature Housing: Person API V1");
            });

            if (Env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
    }
  }
}
