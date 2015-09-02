using System.Linq;
using SampleWebApiMVC6.Models;

namespace SampleWebApiMVC6.Services
{
    public class HouseMapper : IHouseMapper
    {
        public HouseDto MapToDto(HouseEntity houseEntity)
        {
            return new HouseDto()
            {
                Id = houseEntity.Id,
                ZipCode = houseEntity.ZipCode,
                City = houseEntity.City,
                Street = houseEntity.Street
            };
        }

        public HouseEntity MapToEntity(HouseDto houseDto)
        {
            return new HouseEntity()
            {
                Id = houseDto.Id == 0 ? Singleton.Instance.Houses.Max(x => x.Id) + 1 : houseDto.Id,
                ZipCode = houseDto.ZipCode,
                City = houseDto.City,
                Street = houseDto.Street
            };
        }
    }
}