using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class AuthorMiddleware{

    private RequestDelegate _next;

    public AuthorMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context){

        context.Response.Headers.Add("x-author-name", "netcore-demo");
        context.Response.Headers.Add("x-author-email", "test@test.com");

        await _next(context);
    }
}