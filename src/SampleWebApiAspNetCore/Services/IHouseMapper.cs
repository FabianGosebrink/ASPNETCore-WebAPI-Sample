using SampleWebApiAspNetCore.Models;

namespace SampleWebApiAspNetCore.Services
{
    public interface IHouseMapper
    {
        HouseDto MapToDto(HouseEntity houseEntity);
        HouseEntity MapToEntity(HouseDto houseDto);
    }
}