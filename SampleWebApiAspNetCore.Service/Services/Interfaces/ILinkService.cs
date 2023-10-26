using Microsoft.AspNetCore.Mvc;
using SampleWebApiAspNetCore.Domain.Models;

namespace SampleWebApiAspNetCore.Service.Services.Interfaces
{
    public interface ILinkService<T>
    {
        object ExpandSingleFoodItem(object resource, int identifier, ApiVersion version);

        List<LinkDto> CreateLinksForCollection(QueryParameters queryParameters, int totalCount, ApiVersion version);
    }
}
