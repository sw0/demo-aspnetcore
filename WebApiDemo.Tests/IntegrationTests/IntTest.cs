using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebApiDemo.Tests.IntegrationTests
{
    public class IntTest
    {
        TestServer _server;

        [SetUp]
        public void Setup()
        {
            var hostBuilder = new WebHostBuilder()
                .UseStartup<Startup>();

            _server = new TestServer(hostBuilder);
        }

        [Test]
        public async Task ListAuthorsTest()
        {

            var client = _server.CreateClient();

            var response = await client.GetAsync("api/author/list");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            //verify result filter behavior
            Assert.IsTrue(response.Headers.Contains("x-result-filter"), "I think Result Filter got applied?");

            //verified, got the response here
            var result = await response.Content.ReadAsStringAsync();

            TestContext.WriteLine(result);
        }
    }
}
