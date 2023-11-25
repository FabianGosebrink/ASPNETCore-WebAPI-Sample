using SampleWebApiAspNetCore.Common.Interface;
using SampleWebApiAspNetCore.Repositories;

namespace SampleWebApiAspNetCore.Services
{
    public interface ISeedDataService : ISingletonService
    {
        void Initialize(FoodDbContext context);
    }
}
