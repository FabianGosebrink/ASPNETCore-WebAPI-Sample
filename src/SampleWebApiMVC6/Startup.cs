using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Routing;
using Microsoft.Framework.DependencyInjection;
using SampleWebApiMVC6.Models;
using SampleWebApiMVC6.Services;
using Microsoft.Framework.Configuration;
using Microsoft.Dnx.Runtime;

namespace SampleWebApiMVC6
{
    public class Startup
    {
        IConfigurationRoot Configuration; 

        public Startup(IApplicationEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(env.ApplicationBasePath)
                            //.AddJsonFile("config.json")
                            .AddEnvironmentVariables();

            Configuration = builder.Build();

            List<HouseEntity> houses = new List<HouseEntity>()
            {
                new HouseEntity() {City = "Town1", Id = 1, Street = "Street1", ZipCode = 1234},
                new HouseEntity() {City = "Town2", Id = 2, Street = "Street2", ZipCode = 5678},
                new HouseEntity() {City = "Town3", Id = 3, Street = "Street3", ZipCode = 9012},
                new HouseEntity() {City = "Town4", Id = 4, Street = "Street4", ZipCode = 3456}
            };

            Singleton.Instance.Houses = houses;
        }

        // This method gets called by a runtime.
        // Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            // Uncomment the following line to add Web API services which makes it easier to port Web API 2 controllers.
            // You will also need to add the Microsoft.AspNet.Mvc.WebApiCompatShim package to the 'dependencies' section of project.json.
            // services.AddWebApiConventions();

            services.AddTransient<IHouseMapper, HouseMapper>();
        }

        // Configure is called after ConfigureServices is called.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseIISPlatformHandler();
            // Configure the HTTP request pipeline.
            app.UseStaticFiles();

            // Add MVC to the request pipeline.
            app.UseMvc();
            // Add the following route for porting Web API 2 controllers.
            // routes.MapWebApiRoute("DefaultApi", "api/{controller}/{id?}");
        }
    }
}
