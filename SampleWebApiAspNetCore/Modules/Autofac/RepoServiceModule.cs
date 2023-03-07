using Autofac;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Options;
using SampleWebApiAspNetCore.Repositories;
using SampleWebApiAspNetCore.Services;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SampleWebApiAspNetCore.Modules.Autofac
{
    public class RepoServiceModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(SeedDataService)).As(typeof(ISeedDataService)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(FoodSqlRepository)).As(typeof(IFoodRepository)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(LinkService<>)).As(typeof(ILinkService<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(ConfigureSwaggerOptions)).As(typeof(IConfigureOptions<SwaggerOptions>)).InstancePerMatchingLifetimeScope();
            builder.RegisterGeneric(typeof(ActionContextAccessor)).As(typeof(IActionContextAccessor)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(UrlHelperFactory)).As(typeof(IUrlHelperFactory)).InstancePerLifetimeScope();

           

            
            builder.Services.AddSingleton<IUrlHelperFactory, UrlHelperFactory>();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();

        }
    }
}
