using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiDemo.Middlewares
{
    public class AuthorHeaderMiddleware
    {
        RequestDelegate _next;
        public AuthorHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Response.Headers.Add("x-author", "shawn lin");

            await _next(context);
        }
    }
}
