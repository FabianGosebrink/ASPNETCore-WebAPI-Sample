using SampleWebApiMVC6.Models;

namespace SampleWebApiMVC6.Services
{
    public interface IHouseMapper
    {
        HouseDto MapToDto(HouseEntity houseEntity);
        HouseEntity MapToEntity(HouseDto houseDto);
    }
}