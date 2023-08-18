using System.Linq.Expressions;
using SampleWebApiAspNetCore.Entities;

namespace SampleWebApiAspNetCore.Repositories;

public interface IUserRepository
{
    void Add(UserEntity item);
    void Delete(int id);
    void Update(UserEntity item);
    void Save();
    UserEntity Get(Expression<Func<UserEntity, bool>> predicate, params Expression<Func<UserEntity, object>>[] includeProperties);
}