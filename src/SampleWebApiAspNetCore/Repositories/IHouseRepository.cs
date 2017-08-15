using SampleWebApiAspNetCore.Entities;
using System.Linq;

namespace SampleWebApiAspNetCore.Repositories
{
    public interface IHouseRepository
    {
        void Add(HouseEntity item);
        void Delete(int id);
        IQueryable<HouseEntity> GetAll();
        HouseEntity GetSingle(int id);
        bool Save();
        int Count();
        void Update(HouseEntity item);
    }
}
