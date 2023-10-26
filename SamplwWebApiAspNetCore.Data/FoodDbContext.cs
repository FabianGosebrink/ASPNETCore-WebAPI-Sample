using Microsoft.EntityFrameworkCore;
using SampleWebApiAspNetCore.Domain.Entities;

namespace SampleWebApiAspNetCore.Data
{
    public class FoodDbContext : DbContext
    {
        public FoodDbContext(DbContextOptions<FoodDbContext> options)
            : base(options)
        {
        }

        public DbSet<FoodEntity> FoodItems { get; set; } = null!;
    }
}
