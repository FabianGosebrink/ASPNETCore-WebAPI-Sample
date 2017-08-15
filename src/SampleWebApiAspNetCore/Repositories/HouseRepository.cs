using System.Collections.Generic;
using System.Linq;
using SampleWebApiAspNetCore.Models;
using SampleWebApiAspNetCore.Entities;

namespace SampleWebApiAspNetCore.Repositories
{
    public class HouseRepository : IHouseRepository
    {
        readonly Dictionary<int, HouseEntity> _houses = new Dictionary<int, HouseEntity>();

        public IQueryable<HouseEntity> GetAll()
        {
            return _houses.Select(x => x.Value).AsQueryable();
        }

        public HouseEntity GetSingle(int id)
        {
            return _houses.FirstOrDefault(x => x.Key == id).Value;
        }

        public void Add(HouseEntity toAdd)
        {
            int newId = !GetAll().Any() ? 1 : GetAll().Max(x => x.Id) + 1;
            toAdd.Id = newId;
            _houses.Add(newId, toAdd);
        }

        public void Update(HouseEntity toUpdate)
        {
            HouseEntity single = GetSingle(toUpdate.Id);
            _houses[single.Id] = toUpdate;
        }

        public void Delete(int id)
        {
            _houses.Remove(id);
        }

        public int Count()
        {
            return _houses.Count();
        }

        public bool Save()
        {
            // would call saveChanges on Entity Framework
            // always returns true here
            return true;
        }
    }
}