using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Helpers;
using SampleWebApiAspNetCore.Repositories;

namespace SampleWebApiAspNetCore.Services
{
    public class SeedDataService : ISeedDataService
    {
        public void Initialize(FoodDbContext context)
        {
            context.FoodItems.Add(new FoodEntity() { Calories = 1000, Type = "Starter", Name = "Lasagne", Created = DateTime.Now });
            context.FoodItems.Add(new FoodEntity() { Calories = 1100, Type = "Main", Name = "Hamburger", Created = DateTime.Now });
            context.FoodItems.Add(new FoodEntity() { Calories = 1200, Type = "Dessert", Name = "Spaghetti", Created = DateTime.Now });
            context.FoodItems.Add(new FoodEntity() { Calories = 1300, Type = "Starter", Name = "Pizza", Created = DateTime.Now });

            context.Roles.Add(new RoleEntity { Id = 1, Role = "Admin", Information = "This role can all operations." });
            context.Roles.Add(new RoleEntity { Id = 2, Role = "Standard", Information = "This role can only read operations." });

            string password = "123456";
            context.Users.Add(new UserEntity() { FirstName = "john", LastName = "Doe", UserName = "john_doe", Email = "john.doe@mail.com", PasswordHash = password.HashPassword(), RoleId=1});

            context.SaveChanges();
        }
    }
}
