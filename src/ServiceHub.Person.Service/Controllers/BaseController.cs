using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ServiceHub.Person.Service.Controllers
{
  public abstract class BaseController : Controller
  {
    protected readonly ILogger logger;

    protected BaseController(ILoggerFactory loggerFactory)
    {
      logger = loggerFactory.CreateLogger(this.GetType().Name);
    }
  }
}
