using Microsoft.AspNetCore.Mvc;
using SampleWebApiAspNetCore.Models;
using SampleWebApiAspNetCore.Helpers;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace SampleWebApiAspNetCore.Services
{
    public class LinkService<T> : ILinkService<T>
    {
        private readonly IUrlHelper _urlHelper;

        public LinkService(IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor)
        {
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        public List<LinkDto> CreateLinksForCollection(QueryParameters queryParameters, int totalCount, ApiVersion version)
        {
            Type controllerType = (typeof(T));
            MethodInfo[] methods = controllerType.GetMethods();

            var links = new List<LinkDto>();
            var getAllMethodName = GetMethod(methods, typeof(HttpGetAttribute), 0);

            // self 
            links.Add(new LinkDto(_urlHelper.Link(getAllMethodName, new
            {
                pagecount = queryParameters.PageCount,
                page = queryParameters.Page,
                orderby = queryParameters.OrderBy
            }), "self", "GET"));

            links.Add(new LinkDto(_urlHelper.Link(getAllMethodName, new
            {
                pagecount = queryParameters.PageCount,
                page = 1,
                orderby = queryParameters.OrderBy
            }), "first", "GET"));

            links.Add(new LinkDto(_urlHelper.Link(getAllMethodName, new
            {
                pagecount = queryParameters.PageCount,
                page = queryParameters.GetTotalPages(totalCount),
                orderby = queryParameters.OrderBy
            }), "last", "GET"));

            if (queryParameters.HasNext(totalCount))
            {
                links.Add(new LinkDto(_urlHelper.Link(getAllMethodName, new
                {
                    pagecount = queryParameters.PageCount,
                    page = queryParameters.Page + 1,
                    orderby = queryParameters.OrderBy
                }), "next", "GET"));
            }

            if (queryParameters.HasPrevious())
            {
                links.Add(new LinkDto(_urlHelper.Link(getAllMethodName, new
                {
                    pagecount = queryParameters.PageCount,
                    page = queryParameters.Page - 1,
                    orderby = queryParameters.OrderBy
                }), "previous", "GET"));
            }

            var posturl = _urlHelper.Link(GetMethod(methods, typeof(HttpPostAttribute)), new { version = version.ToString() });

            links.Add(
               new LinkDto(posturl,
               "create",
               "POST"));

            return links;
        }

        public object ExpandSingleFoodItem(object resource, int identifier, ApiVersion version)
        {
            var resourceToReturn = resource.ToDynamic() as IDictionary<string, object>;

            var links = GetLinksForSingleItem(identifier, version);

            resourceToReturn.Add("links", links);

            return resourceToReturn;
        }


        private IEnumerable<LinkDto> GetLinksForSingleItem(int id, ApiVersion version)
        {
            Type myType = (typeof(T));
            MethodInfo[] methods = myType.GetMethods();
            var links = new List<LinkDto>();

            var getLink = _urlHelper.Link(GetMethod(methods, typeof(HttpGetAttribute), 1), new { version = version.ToString(), id = id });
            links.Add(new LinkDto(getLink, "self", "GET"));

            var deleteLink = _urlHelper.Link(GetMethod(methods, typeof(HttpDeleteAttribute)), new { version = version.ToString(), id = id });
            links.Add(
              new LinkDto(deleteLink,
              "delete",
              "DELETE"));

            var createLink = _urlHelper.Link(GetMethod(methods, typeof(HttpPostAttribute)), new { version = version.ToString() });
            links.Add(
              new LinkDto(createLink,
              "create_food",
              "POST"));

            var updateLink = _urlHelper.Link(GetMethod(methods, typeof(HttpPutAttribute)), new { version = version.ToString(), id = id });
            links.Add(
               new LinkDto(updateLink,
               "update_food",
               "PUT"));

            return links;
        }

        private string GetMethod(MethodInfo[] methods, Type type, int routeParamsLength = 0)
        {
            var filteredMethods = methods.Where(m => m.GetCustomAttributes(type, false).Length > 0).ToArray();

            if (filteredMethods.Length == 0)
            {
                return "";
            }

            if (routeParamsLength == 0)
            {
                var toReturn = filteredMethods.FirstOrDefault();

                return toReturn is not null ? toReturn.Name : "";
            }

            foreach (var method in filteredMethods)
            {
                var routeAttribs = method.GetCustomAttributes(typeof(RouteAttribute));

                if (routeAttribs.Count() == routeParamsLength)
                {
                    return method.Name;
                }
            }

            return "";
        }
    }
}
