using SampleWebApiAspNetCore.Repositories;
using System.Threading.Tasks;

namespace SampleWebApiAspNetCore.Services
{
    public interface ISeedDataService
    {
        Task Initialize(FoodDbContext context);
    }
}
