using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebApiDemo.Filters
{
    /// <summary>
    /// Sample Filter to simulate common model validation by global filter
    /// </summary>
    public class AlwaysInvalidValidationAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            context.ModelState.AddModelError("AlwaysInvalid", $"Example apply custom validation here, lie using FluentValidation");
            context.ModelState.AddModelError("Data", "You can try to get model data by `context.ActionArguments[\"data\"]`");

            if(context.ActionArguments.TryGetValue("data", out object data))
            {
                //todo validation
            }

            if (true)//short-circuit filter
            {
                context.Result = new ContentResult()
                {
                    Content = "BAD REQUEST, Invalidation failed in AlwaysInvalidValidationAttribute. You Custom error response goes here.",
                    StatusCode = 400,
                    ContentType = "text/plain"
                };
            }
            else
            {
                await next();
            }

            await Task.CompletedTask;
        }
    }
}
