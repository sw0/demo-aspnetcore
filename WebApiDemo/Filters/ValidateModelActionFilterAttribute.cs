using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using WebApiDemo.Models;
using WebApiDemo.Services;

namespace WebApiDemo.Filters
{
    public class ValidateModelActionFilterAttribute : ActionFilterAttribute
    {
        ILogger<ValidateModelActionFilterAttribute> _logger;
        IAuthorService _service;
        public ValidateModelActionFilterAttribute(ILogger<ValidateModelActionFilterAttribute> logger,
            IAuthorService service)
        {
            _logger = logger;
            _service = service;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.HttpContext.Response.Headers.Add("x-sync-action-filter", "ModelState.IsValid is false");

                _logger.LogInformation("header `{0}` got added", "x-sync-action-filter");
            }

            if (context.ActionArguments.TryGetValue("data", out object data))
            {
                if (data is PoemViewModel poem)
                {
                    if (!string.IsNullOrEmpty(poem.Author))
                    {
                        if (!_service.IsValidAuthor(poem.Author))
                        {
                            context.ModelState.AddModelError(nameof(poem.Author), 
                                $"author '{poem.Author}' is invalid");
                        }
                    }
                }
            }

            base.OnActionExecuting(context);
        }
    }
}
