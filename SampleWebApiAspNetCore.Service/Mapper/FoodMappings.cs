using AutoMapper;
using SampleWebApiAspNetCore.Domain.DataTransferObjects.Dtos;
using SampleWebApiAspNetCore.Domain.Entities;

namespace SampleWebApiAspNetCore.Service.Mapper
{
    public class FoodMappings : Profile
    {
        public FoodMappings()
        {
            CreateMap<FoodEntity, FoodDto>().ReverseMap();
            CreateMap<FoodEntity, FoodUpdateDto>().ReverseMap();
            CreateMap<FoodEntity, FoodCreateDto>().ReverseMap();
        }
    }
}
