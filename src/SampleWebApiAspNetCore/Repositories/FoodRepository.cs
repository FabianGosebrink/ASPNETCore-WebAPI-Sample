using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Models;
using System.Linq.Dynamic.Core;
using SampleWebApiAspNetCore.Helpers;

namespace SampleWebApiAspNetCore.Repositories
{
    public class FoodRepository : IFoodRepository
    {
        private readonly ConcurrentDictionary<int, FoodItem> _storage = new ConcurrentDictionary<int, FoodItem>();

        public FoodItem GetSingle(int id)
        {
            FoodItem foodItem;
            return _storage.TryGetValue(id, out foodItem) ? foodItem : null;
        }

        public void Add(FoodItem item)
        {
            item.Id = !_storage.Values.Any() ? 1 : _storage.Values.Max(x => x.Id) + 1;

            if (!_storage.TryAdd(item.Id, item))
            {
                throw new Exception("Item could not be added");
            }
        }

        public void Delete(int id)
        {
            FoodItem foodItem;
            if (!_storage.TryRemove(id, out foodItem))
            {
                throw new Exception("Item could not be removed");
            }
        }

        public FoodItem Update(int id, FoodItem item)
        {
            _storage.TryUpdate(id, item, GetSingle(id));
            return item;
        }

        public IQueryable<FoodItem> GetAll(QueryParameters queryParameters)
        {
            IQueryable<FoodItem> _allItems = _storage.Values.AsQueryable().OrderBy(queryParameters.OrderBy,
               queryParameters.IsDescending());

            if (queryParameters.HasQuery())
            {
                _allItems = _allItems
                    .Where(x => x.Calories.ToString().Contains(queryParameters.Query.ToLowerInvariant())
                    || x.Name.ToLowerInvariant().Contains(queryParameters.Query.ToLowerInvariant()));
            }

            return _allItems
                .Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);
        }

        public ICollection<FoodItem> GetRandomMeal()
        {
            List<FoodItem> toReturn = new List<FoodItem>();

            toReturn.Add(GetRandomItem("Starter"));
            toReturn.Add(GetRandomItem("Main"));
            toReturn.Add(GetRandomItem("Dessert"));

            return toReturn;
        }

        public int Count()
        {
            return _storage.Count;
        }

        public bool Save()
        {
            // To keep interface consistent with Controllers, Tests & EF Interfaces
            return true;
        }
        private FoodItem GetRandomItem(string type)
        {
            return _storage.Values
                .Where(x => x.Type == type)
                .OrderBy(o => Guid.NewGuid())
                .FirstOrDefault();
        }
    }
}