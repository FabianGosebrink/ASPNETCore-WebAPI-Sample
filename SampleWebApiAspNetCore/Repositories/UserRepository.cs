using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SampleWebApiAspNetCore.Entities;

namespace SampleWebApiAspNetCore.Repositories;

public class UserRepository : IUserRepository
{
    private readonly FoodDbContext _dbContext;

    public UserRepository(FoodDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(UserEntity item) => _dbContext.Users.Add(item);

    public void Update(UserEntity item) => _dbContext.Users.Update(item);

    public void Delete(int id)
    {
        var user = _dbContext.Users.SingleOrDefault(x => x.Id == id)!;
        _dbContext.Users.Remove(user);
    }

    public void Save() => _dbContext.SaveChanges();

    public UserEntity Get(Expression<Func<UserEntity, bool>> predicate, params Expression<Func<UserEntity, object>>[] includeProperties)
    {
        IQueryable<UserEntity> query = _dbContext.Users.Where(predicate);
        if (includeProperties.Count() > 0)
        {
            foreach (var property in includeProperties)
            {
                query = query.Include(property);
            }
        }

        return query.SingleOrDefault()!;
    }
}