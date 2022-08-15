using Microsoft.AspNetCore.Mvc;
using SampleWebApiAspNetCore.Models;
using SampleWebApiAspNetCore.Helpers;
using System.Reflection;

namespace SampleWebApiAspNetCore.Services
{
    [AttributeUsage(AttributeTargets.Method)]
    public class LinkCollectionAttribute : Attribute
    {
        private string name;

        public LinkCollectionAttribute(string name)
        {
            this.name = name;
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class HateoasAttribute : Attribute
    {

    }

    public class LinkService<T> : ILinkService<T>
    {
        private readonly IUrlHelper _urlHelper;

        public LinkService(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public List<LinkDto> CreateLinksForCollection(QueryParameters queryParameters, int totalCount, ApiVersion version)
        {
            Type myType = (typeof(T));
            MethodInfo[] methods = myType.GetMethods();

            var links = new List<LinkDto>();
            var getAllMethodName = GetGetAllMethod(methods);

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

            var links = GetLinks(identifier, version);

            resourceToReturn.Add("links", links);

            return resourceToReturn;
        }


        private IEnumerable<LinkDto> GetLinks(int id, ApiVersion version)
        {
            Type myType = (typeof(T));
            MethodInfo[] methods = myType.GetMethods();
            var links = new List<LinkDto>();

            var getLink = _urlHelper.Link(GetGetSingleMethod(methods), new { version = version.ToString(), id = id });
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

        private string GetMethod(MethodInfo[] methods, Type type)
        {
            var method = methods.Where(m => m.GetCustomAttributes(type, false).Length > 0).ToArray().FirstOrDefault();

            if (method is null)
            {
                return null;
            }

            return method.Name;
        }

        private string GetGetSingleMethod(MethodInfo[] methods)
        {
            var getMethods = methods.Where(m => m.GetCustomAttributes(typeof(HttpGetAttribute), false).Length > 0).ToArray();

            if (getMethods.Length == 0)
            {
                return null;
            }

            foreach (var getMethod in getMethods)
            {
                var routeAttribs = getMethod.GetCustomAttributes(typeof(RouteAttribute));

                if(routeAttribs.Count() == 1)
                {
                    return getMethod.Name;
                }
            }


            return null;
        }

        private string GetGetAllMethod(MethodInfo[] methods)
        {
            var getMethods = methods.Where(m => m.GetCustomAttributes(typeof(HttpGetAttribute), false).Length > 0).ToArray();

            if (getMethods.Length == 0)
            {
                return null;
            }

            foreach (var getMethod in getMethods)
            {
                var routeAttribs = getMethod.GetCustomAttributes(typeof(RouteAttribute));

                if (routeAttribs.Count() == 0)
                {
                    return getMethod.Name;
                }
            }


            return null;
        }
    }
}
