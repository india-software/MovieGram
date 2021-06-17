

using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace Munna.Concentrix.MovieGram
{
    public class ValidateModelStateAttribute : Microsoft.AspNetCore.Mvc.Filters.ActionFilterAttribute
    {
        public void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ModelState.IsValid == false)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    HttpStatusCode.BadRequest, actionContext.ModelState);
            }
        }
    }
}
