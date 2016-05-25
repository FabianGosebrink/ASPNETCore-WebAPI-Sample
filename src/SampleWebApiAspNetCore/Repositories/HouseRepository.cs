using System.Collections.Generic;
using System.Linq;
using SampleWebApiAspNetCore.Models;

namespace SampleWebApiAspNetCore.Repositories
{
    public class HouseRepository : IHouseRepository
    {
        readonly Dictionary<int, HouseEntity> _houses = new Dictionary<int, HouseEntity>();

        public HouseRepository()
        {
            _houses.Add(1, new HouseEntity() { City = "Town1", Id = 1, Street = "Street1", ZipCode = 1234 });
            _houses.Add(2, new HouseEntity() { City = "Town2", Id = 2, Street = "Street2", ZipCode = 1234 });
            _houses.Add(3, new HouseEntity() { City = "Town3", Id = 3, Street = "Street3", ZipCode = 1234 });
            _houses.Add(4, new HouseEntity() { City = "Town4", Id = 4, Street = "Street4", ZipCode = 1234 });
        }

        public List<HouseEntity> GetAll()
        {
            return _houses.Select(x => x.Value).ToList();
        }

        public HouseEntity GetSingle(int id)
        {
            return _houses.FirstOrDefault(x => x.Key == id).Value;
        }

        public HouseEntity Add(HouseEntity toAdd)
        {
            int newId = !GetAll().Any() ? 1 : GetAll().Max(x => x.Id) + 1;
            toAdd.Id = newId;
            _houses.Add(newId, toAdd);
            return toAdd;
        }

        public HouseEntity Update(HouseEntity toUpdate)
        {
            HouseEntity single = GetSingle(toUpdate.Id);

            if (single == null)
            {
                return null;
            }

            _houses[single.Id] = toUpdate;
            return toUpdate;
        }

        public void Delete(int id)
        {
            _houses.Remove(id);
        }
    }
}