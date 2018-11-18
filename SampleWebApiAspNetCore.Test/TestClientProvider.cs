using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using WebApplication11;

namespace SampleWebApiAspNetCore.Test
{
    public class TestClientProvider
    {
        public HttpClient Client { get; private set; }

        public TestClientProvider()
        {
            var server = new TestServer(
                new WebHostBuilder()
                .UseStartup<Startup>());

            Client = server.CreateClient();
        }
    }
}
