﻿//using SampleWebApiAspNetCore.Data.Repositories.Interfaces;
//using SampleWebApiAspNetCore.Domain.Entities;
//using SampleWebApiAspNetCore.Domain.Models;

using SampleWebApiAspNetCore.Data.Repositories.Interfaces;
using SampleWebApiAspNetCore.Domain.Entities;
using SampleWebApiAspNetCore.Domain.Models;


namespace SampleWebApiAspNetCore.Data.Repositories.Implementations
{
    public class FoodSqlRepository : IFoodRepository
    {
        private readonly FoodDbContext _foodDbContext;

        public FoodSqlRepository(FoodDbContext foodDbContext)
        {
            _foodDbContext = foodDbContext;
        }

        public FoodEntity GetSingle(int id)
        {
            return _foodDbContext.FoodItems.FirstOrDefault(x => x.Id == id);
        }

        public void Add(FoodEntity item)
        {
            _foodDbContext.FoodItems.Add(item);
        }

        public void Delete(int id)
        {
            FoodEntity foodItem = GetSingle(id);
            _foodDbContext.FoodItems.Remove(foodItem);
        }

        public FoodEntity Update(int id, FoodEntity item)
        {
            _foodDbContext.FoodItems.Update(item);
            return item;
        }

        //public IQueryable<FoodEntity> GetAll(QueryParameters queryParameters)
        //{
        //    IQueryable<FoodEntity> _allItems = _foodDbContext.FoodItems.OrderBy(queryParameters.OrderBy,
        //      queryParameters.IsDescending);

        //    if (queryParameters.HasQuery)
        //    {
        //        _allItems = _allItems
        //            .Where(x => x.Calories.ToString().Contains(queryParameters.Query.ToLowerInvariant())
        //            || x.Name.ToLowerInvariant().Contains(queryParameters.Query.ToLowerInvariant()));
        //    }

        //    return _allItems
        //        .Skip(queryParameters.PageCount * (queryParameters.Page - 1))
        //        .Take(queryParameters.PageCount);
        //}

        public IQueryable<FoodEntity> GetAll(QueryParameters queryParameters)
        {
            IQueryable<FoodEntity> _allItems = _foodDbContext.FoodItems;

            if (queryParameters.HasQuery)
            {
                _allItems = _allItems
                    .Where(x => x.Calories.ToString().Contains(queryParameters.Query.ToLowerInvariant())
                        || x.Name.ToLowerInvariant().Contains(queryParameters.Query.ToLowerInvariant()));
            }

            // Apply sorting
            switch (queryParameters.OrderBy)
            {
                case "Calories":
                    _allItems = queryParameters.IsDescending
                        ? _allItems.OrderByDescending(x => x.Calories)
                        : _allItems.OrderBy(x => x.Calories);
                    break;

                case "Name":
                default:
                    _allItems = queryParameters.IsDescending
                        ? _allItems.OrderByDescending(x => x.Name)
                        : _allItems.OrderBy(x => x.Name);
                    break;
            }

            return _allItems
                .Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);
        }


        public int Count()
        {
            return _foodDbContext.FoodItems.Count();
        }

        public bool Save()
        {
            return (_foodDbContext.SaveChanges() >= 0);
        }

        public ICollection<FoodEntity> GetRandomMeal()
        {
            List<FoodEntity> toReturn = new List<FoodEntity>();

            toReturn.Add(GetRandomItem("Starter"));
            toReturn.Add(GetRandomItem("Main"));
            toReturn.Add(GetRandomItem("Dessert"));

            return toReturn;
        }

        private FoodEntity GetRandomItem(string type)
        {
            return _foodDbContext.FoodItems
                .Where(x => x.Type == type)
                .OrderBy(o => Guid.NewGuid())
                .FirstOrDefault();
        }
    }
}
