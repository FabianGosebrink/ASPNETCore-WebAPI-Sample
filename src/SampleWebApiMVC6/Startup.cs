using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SampleWebApiMVC6.Models;
using SampleWebApiMVC6.Services;

namespace SampleWebApiMVC6
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            List<HouseEntity> houses = new List<HouseEntity>()
            {
                new HouseEntity() {City = "Town1", Id = 1, Street = "Street1", ZipCode = 1234},
                new HouseEntity() {City = "Town2", Id = 2, Street = "Street2", ZipCode = 5678},
                new HouseEntity() {City = "Town3", Id = 3, Street = "Street3", ZipCode = 9012},
                new HouseEntity() {City = "Town4", Id = 4, Street = "Street4", ZipCode = 3456}
            };

            Singleton.Instance.Houses = houses;

            services.AddTransient<IHouseMapper, HouseMapper>();
            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseIISPlatformHandler();

            app.UseStaticFiles();

            app.UseMvc();
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
