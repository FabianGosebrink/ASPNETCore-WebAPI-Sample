using System.Collections.Generic;
using SampleWebApiAspNetCore.Models;

namespace SampleWebApiAspNetCore.Repositories
{
    public interface IHouseRepository
    {
        List<HouseEntity> GetAll();
        HouseEntity GetSingle(int id);
        HouseEntity Add(HouseEntity toAdd);
        HouseEntity Update(HouseEntity toUpdate);
        void Delete(int id);
    }
}
