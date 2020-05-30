using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiDemo.Filters
{
    /*
     * NOTE: if attribute [ApiController] got applied to controller,
     * and you want to check ModelState.IsValid in custom ActionFilter,
     * you need to set 'ApiBehaviorOptions.SuppressModelStateInvalidFilter' to true,
     * otherwise, it will not called because it automatically respond HTTP 400 Bad Request
     * when model validation failed during after model binding. 
     * 
     * For more details, please refer to: 
     * https://docs.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-3.1#automatic-http-400-responses 
     */
    public class ValidateModelAsyncActionFilterAttribute : IAsyncActionFilter
    {
        ILogger<ValidateModelAsyncActionFilterAttribute> _logger;
        public ValidateModelAsyncActionFilterAttribute(ILogger<ValidateModelAsyncActionFilterAttribute> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                context.HttpContext.Response.Headers.Add("x-async-action-filter", "ModelState.IsValid is false");

                _logger.LogInformation("header `{0}` got added", "x-async-action-filter");
            }

            await next();
        }
    }
}
