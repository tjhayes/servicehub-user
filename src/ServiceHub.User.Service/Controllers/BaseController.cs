using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ServiceHub.User.Service.Controllers
{
  public abstract class BaseController : Controller
  {
    protected readonly ILogger logger;

    protected BaseController(ILoggerFactory loggerFactory)
    {
      logger = loggerFactory.CreateLogger(GetType().Name);
    }
  }
}
