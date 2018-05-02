
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Web.Http.Filters;

namespace Housing.Foundation.Library.Filters
{
    class FoundationExceptionFilter : ExceptionFilterAttribute
    {
        /// <summary>
        /// A custom exception filter that handles the exceptions in the Apartment, Batch, and person
        /// service controllers.
        /// 
        /// TODO: Integrate exception testing in unit testing.
        /// </summary>
        /// <param name="actionExecutedContext">The context for the exception</param>
        public override void OnException(HttpActionExecutedContext actx)
        {
            HttpResponseMessage res = new HttpResponseMessage();



            // Default server error handling.
            if (actx.Response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                actx.Response.Content = new StringContent("Something went wrong, please try again later.");
            }

            // Cannot get a response to the server.
            if (actx.Response.StatusCode == System.Net.HttpStatusCode.BadGateway)
            {
                // Log.
                actx.Response.Content = new StringContent("Unable to get a response from the server");
            }
        }
    }
}
