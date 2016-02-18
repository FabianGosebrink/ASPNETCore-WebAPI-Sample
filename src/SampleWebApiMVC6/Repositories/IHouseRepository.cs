using System.Collections.Generic;
using SampleWebApiMVC6.Models;

namespace SampleWebApiMVC6.Repositories
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
