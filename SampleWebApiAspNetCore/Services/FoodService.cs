using Microsoft.AspNetCore.Mvc;
using SampleWebApiAspNetCore.Models;

namespace SampleWebApiAspNetCore.Services
{
public class FoodService : IFoodService
{
    private readonly IFoodRepository _foodRepository;
    private readonly ILinkService _linkService;

    public FoodService(IFoodRepository foodRepository, ILinkService linkService)
    {
        _foodRepository = foodRepository;
        _linkService = linkService;
    }

    public FoodResultDto GetAllFoods(QueryParameters queryParameters, ApiVersion version)
    {
        var foodItems = _foodRepository.GetAll(queryParameters);
        var allItemCount = _foodRepository.Count();
        var paginationMetadata = new PaginationMetadata(allItemCount, queryParameters);

        var links = _linkService.CreateLinksForCollection(queryParameters, allItemCount, version);
        var toReturn = foodItems.Select(x => _linkService.ExpandSingleFoodItem(x, x.Id, version));

        return new FoodResultDto(toReturn, links, paginationMetadata);
    }
}

}
