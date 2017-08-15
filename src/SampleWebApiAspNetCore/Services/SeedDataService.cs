using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Repositories;

namespace SampleWebApiAspNetCore.Services
{
    public class SeedDataService : ISeedDataService
    {
        IHouseRepository _repository;


        public SeedDataService(IHouseRepository repository)
        {
            _repository = repository;
        }

        public void EnsureSeedData()
        {
            _repository.Add(new HouseEntity() { City = "Town1", Id = 1, Street = "Street1", ZipCode = 1234 });
            _repository.Add(new HouseEntity() { City = "Town2", Id = 2, Street = "Street2", ZipCode = 1234 });
            _repository.Add(new HouseEntity() { City = "Town3", Id = 3, Street = "Street3", ZipCode = 1234 });
            _repository.Add(new HouseEntity() { City = "Town4", Id = 4, Street = "Street4", ZipCode = 1234 });
        }
    }
}
