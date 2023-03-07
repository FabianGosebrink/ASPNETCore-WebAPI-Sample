using FluentValidation.AspNetCore;
using SampleWebApiAspNetCore.ValidationRules.FluentValidation;

namespace SampleWebApiAspNetCore.Helpers
{
    public static class StartupExtension
    {

        public static void AddFluentValidationExtensions(this IServiceCollection services)
        {

            services.AddControllersWithViews().AddFluentValidation(x =>
            {
                x.RegisterValidatorsFromAssemblyContaining<CreateFoodDtoValidator>();

            });
        }
    }
}
