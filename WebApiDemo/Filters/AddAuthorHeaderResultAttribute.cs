using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiDemo.Filters
{
    public class AddAuthorHeaderResultAttribute : ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.HttpContext.Response.Headers.ContainsKey("x-result-filter"))
            {
                context.HttpContext.Response.Headers.Add("x-result-filter-2nd", "new header key to avoid server error");
            }
            else
            {
                context.HttpContext.Response.Headers.Add("x-result-filter", nameof(AddAuthorHeaderResultAttribute));
            }

            base.OnResultExecuting(context);
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            base.OnResultExecuted(context);
        }
    }
}
