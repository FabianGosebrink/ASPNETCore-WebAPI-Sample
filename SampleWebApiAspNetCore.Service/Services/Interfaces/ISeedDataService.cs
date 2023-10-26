using SampleWebApiAspNetCore.Data;

namespace SampleWebApiAspNetCore.Service.Services.Interfaces
{
    public interface ISeedDataService
    {
        void Initialize(FoodDbContext context);
    }
}
