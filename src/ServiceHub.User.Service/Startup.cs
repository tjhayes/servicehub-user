using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
      services.AddMvc();

      services.AddSingleton<IQueueClient>(qc =>
        new QueueClient(
          Environment.GetEnvironmentVariable("SERVICE_BUS_CONNECTION_STRING"),
          Environment.GetEnvironmentVariable("SERVICE_BUS_QUEUE_NAME")
        )
      );

      services.AddTransient<IUserRepository, UserRepository>();
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
