using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiDemo.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthorHeaderMiddleware(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware<AuthorHeaderMiddleware>();

            return applicationBuilder;
        }
    }
}
