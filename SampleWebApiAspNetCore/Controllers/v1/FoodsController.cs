using System;
using System.Linq;
using AutoMapper;
using SampleWebApiAspNetCore.Dtos;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SampleWebApiAspNetCore.Repositories;
using System.Collections.Generic;
using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Models;
using SampleWebApiAspNetCore.Helpers;

namespace SampleWebApiAspNetCore.v1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    //[Route("api/[controller]")]
    [ApiController]
    public class FoodsController : ControllerBase
    {
        private readonly IFoodRepository _foodRepository;
        private readonly IUrlHelper _urlHelper;

        public FoodsController(IUrlHelper urlHelper, IFoodRepository foodRepository)
        {
            _foodRepository = foodRepository;
            _urlHelper = urlHelper;
        }

        [HttpGet(Name = nameof(GetAllFoods))]
        public ActionResult GetAllFoods([FromQuery] QueryParameters queryParameters)
        {
            List<FoodItem> foodItems = _foodRepository.GetAll(queryParameters).ToList();

            var allItemCount = _foodRepository.Count();

            var paginationMetadata = new
            {
                totalCount = allItemCount,
                pageSize = queryParameters.PageCount,
                currentPage = queryParameters.Page,
                totalPages = queryParameters.GetTotalPages(allItemCount)
            };

            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            var links = CreateLinksForCollection(queryParameters, allItemCount);

            var toReturn = foodItems.Select(x => ExpandSingleFoodItem(x));

            return Ok(new
            {
                value = toReturn,
                links = links
            });
        }

        [HttpGet]
        [Route("{id:int}", Name = nameof(GetSingleFood))]
        public ActionResult GetSingleFood(int id)
        {
            FoodItem foodItem = _foodRepository.GetSingle(id);

            if (foodItem == null)
            {
                return NotFound();
            }

            return Ok(ExpandSingleFoodItem(foodItem));
        }

        [HttpPost(Name = nameof(AddFood))]
        public ActionResult<FoodItemDto> AddFood([FromBody] FoodCreateDto foodCreateDto)
        {
            if (foodCreateDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            FoodItem toAdd = Mapper.Map<FoodItem>(foodCreateDto);

            _foodRepository.Add(toAdd);

            if (!_foodRepository.Save())
            {
                throw new Exception("Creating a fooditem failed on save.");
            }

            FoodItem newFoodItem = _foodRepository.GetSingle(toAdd.Id);

            return CreatedAtRoute(nameof(GetSingleFood), new { id = newFoodItem.Id },
                Mapper.Map<FoodItemDto>(newFoodItem));
        }

        [HttpPatch("{id:int}", Name = nameof(PartiallyUpdateFood))]
        public ActionResult<FoodItemDto> PartiallyUpdateFood(int id, [FromBody] JsonPatchDocument<FoodUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            FoodItem existingEntity = _foodRepository.GetSingle(id);

            if (existingEntity == null)
            {
                return NotFound();
            }

            FoodUpdateDto foodUpdateDto = Mapper.Map<FoodUpdateDto>(existingEntity);
            patchDoc.ApplyTo(foodUpdateDto, ModelState);

            TryValidateModel(foodUpdateDto);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Mapper.Map(foodUpdateDto, existingEntity);
            FoodItem updated = _foodRepository.Update(id, existingEntity);

            if (!_foodRepository.Save())
            {
                throw new Exception("Updating a fooditem failed on save.");
            }

            return Ok(Mapper.Map<FoodItemDto>(updated));
        }

        [HttpDelete]
        [Route("{id:int}", Name = nameof(RemoveFood))]
        public ActionResult RemoveFood(int id)
        {
            FoodItem foodItem = _foodRepository.GetSingle(id);

            if (foodItem == null)
            {
                return NotFound();
            }

            _foodRepository.Delete(id);

            if (!_foodRepository.Save())
            {
                throw new Exception("Deleting a fooditem failed on save.");
            }

            return NoContent();
        }

        [HttpPut]
        [Route("{id:int}", Name = nameof(UpdateFood))]
        public ActionResult<FoodItemDto> UpdateFood(int id, [FromBody]FoodUpdateDto foodUpdateDto)
        {
            if (foodUpdateDto == null)
            {
                return BadRequest();
            }

            var existingFoodItem = _foodRepository.GetSingle(id);

            if (existingFoodItem == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Mapper.Map(foodUpdateDto, existingFoodItem);

            _foodRepository.Update(id, existingFoodItem);

            if (!_foodRepository.Save())
            {
                throw new Exception("Updating a fooditem failed on save.");
            }

            return Ok(Mapper.Map<FoodItemDto>(existingFoodItem));
        }

        [HttpGet("GetRandomMeal", Name = nameof(GetRandomMeal))]
        public ActionResult GetRandomMeal()
        {
            ICollection<FoodItem> foodItems = _foodRepository.GetRandomMeal();

            IEnumerable<FoodItemDto> dtos = foodItems
                .Select(x => Mapper.Map<FoodItemDto>(x));

            var links = new List<LinkDto>();

            // self 
            links.Add(new LinkDto(_urlHelper.Link(nameof(GetRandomMeal), null), "self", "GET"));

            return Ok(new
            {
                value = dtos,
                links = links
            });
        }

        private List<LinkDto> CreateLinksForCollection(QueryParameters queryParameters, int totalCount)
        {
            var links = new List<LinkDto>();

            // self 
            links.Add(
             new LinkDto(_urlHelper.Link(nameof(GetAllFoods), new
             {
                 pagecount = queryParameters.PageCount,
                 page = queryParameters.Page,
                 orderby = queryParameters.OrderBy
             }), "self", "GET"));

            links.Add(new LinkDto(_urlHelper.Link(nameof(GetAllFoods), new
            {
                pagecount = queryParameters.PageCount,
                page = 1,
                orderby = queryParameters.OrderBy
            }), "first", "GET"));

            links.Add(new LinkDto(_urlHelper.Link(nameof(GetAllFoods), new
            {
                pagecount = queryParameters.PageCount,
                page = queryParameters.GetTotalPages(totalCount),
                orderby = queryParameters.OrderBy
            }), "last", "GET"));

            if (queryParameters.HasNext(totalCount))
            {
                links.Add(new LinkDto(_urlHelper.Link(nameof(GetAllFoods), new
                {
                    pagecount = queryParameters.PageCount,
                    page = queryParameters.Page + 1,
                    orderby = queryParameters.OrderBy
                }), "next", "GET"));
            }

            if (queryParameters.HasPrevious())
            {
                links.Add(new LinkDto(_urlHelper.Link(nameof(GetAllFoods), new
                {
                    pagecount = queryParameters.PageCount,
                    page = queryParameters.Page - 1,
                    orderby = queryParameters.OrderBy
                }), "previous", "GET"));
            }

            links.Add(
               new LinkDto(_urlHelper.Link(nameof(AddFood), null),
               "create_food",
               "POST"));

            return links;
        }

        private dynamic ExpandSingleFoodItem(FoodItem foodItem)
        {
            var links = GetLinks(foodItem.Id);
            FoodItemDto item = Mapper.Map<FoodItemDto>(foodItem);

            var resourceToReturn = item.ToDynamic() as IDictionary<string, object>;
            resourceToReturn.Add("links", links);

            return resourceToReturn;
        }

        private IEnumerable<LinkDto> GetLinks(int id)
        {
            var links = new List<LinkDto>();

            links.Add(
              new LinkDto(_urlHelper.Link(nameof(GetSingleFood), new { id = id }),
              "self",
              "GET"));

            links.Add(
              new LinkDto(_urlHelper.Link(nameof(RemoveFood), new { id = id }),
              "delete_food",
              "DELETE"));

            links.Add(
              new LinkDto(_urlHelper.Link(nameof(AddFood), null),
              "create_food",
              "POST"));

            links.Add(
               new LinkDto(_urlHelper.Link(nameof(UpdateFood), new { id = id }),
               "update_food",
               "PUT"));

            return links;
        }
    }
}
