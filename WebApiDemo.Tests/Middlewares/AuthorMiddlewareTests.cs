using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApiDemo.Middlewares;

namespace WebApiDemo.Tests.Middlewares
{
    public class Tests
    {
        public Tests()
        {

        }
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task Test1()
        {
            using var host = await new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .UseTestServer()
                        .ConfigureServices(services =>
                        {
                        })
                        .Configure(app =>
                        {
                            app.UseAuthorMiddleware();

                            app.Run(async context =>
                            {
                                await context.Response.WriteAsync("GOOD");
                            });
                        });
                })
                .StartAsync();

            var response = await host.GetTestServer().CreateClient().GetAsync("/");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("netcore-demo", response.Headers.GetValues("x-author-name").First());
            Assert.AreEqual("test@test.com", response.Headers.GetValues("x-author-email").First(), "header x-author-email is not match");

            Console.WriteLine("good");

            TestContext.WriteLine("good sfsf");
        }
    }
}