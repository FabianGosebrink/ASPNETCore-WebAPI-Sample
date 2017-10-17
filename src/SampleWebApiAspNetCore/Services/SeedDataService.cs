using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Repositories;
using System;

namespace SampleWebApiAspNetCore.Services
{
    public class SeedDataService : ISeedDataService
    {
        IFoodRepository _repository;

        public SeedDataService(IFoodRepository repository)
        {
            _repository = repository;
        }

        public void EnsureSeedData()
        {
            _repository.Add(new FoodItem() { Calories = 1000, Id = 1, Name = "Lasagne", Created = DateTime.Now });
            _repository.Add(new FoodItem() { Calories = 1100, Id = 2, Name = "Hamburger", Created = DateTime.Now });
            _repository.Add(new FoodItem() { Calories = 1200, Id = 3, Name = "Spaghetti", Created = DateTime.Now });
            _repository.Add(new FoodItem() { Calories = 1300, Id = 4, Name = "Pizza", Created = DateTime.Now });
        }
    }
}
