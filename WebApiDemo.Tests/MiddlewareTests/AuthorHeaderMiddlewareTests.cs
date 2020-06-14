using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApiDemo.Filters;
using WebApiDemo.Middlewares;
using WebApiDemo.Services;

namespace WebApiDemo.Tests.MiddlewareTests
{
    public class AuthorHeaderMiddlewareTests
    {
        [Test]
        public async Task AuthorHeaderExpected()
        {
            //assign
            var content = "short-circuit end middleware.";
            var server = new TestServer(new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddControllers();
                })
                .Configure(configApp =>
                {
                    configApp.UseRouting();

                    configApp.UseMiddleware<AuthorHeaderMiddleware>();

                    configApp.Run(async context => await context.Response.WriteAsync(content));

                }).UseTestServer());

            var client = server.CreateClient();

            //act
            var response = await client.GetAsync("anyurl");

            //assert: verify middleware
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsTrue(response.Headers.GetValues("x-author").First() == "shawn lin", "expect header x-author");
            Assert.AreEqual(content, await response.Content.ReadAsStringAsync());
        }
    }
}
