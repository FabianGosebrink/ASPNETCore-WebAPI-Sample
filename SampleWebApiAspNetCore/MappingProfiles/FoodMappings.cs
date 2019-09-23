using AutoMapper;
using SampleWebApiAspNetCore.Dtos;
using SampleWebApiAspNetCore.Entities;

namespace SampleWebApiAspNetCore.MappingProfiles
{
    public class FoodMappings : Profile
    {
        public FoodMappings()
        {
            CreateMap<FoodItem, FoodItemDto>().ReverseMap();
            CreateMap<FoodItem, FoodUpdateDto>().ReverseMap();
            CreateMap<FoodItem, FoodCreateDto>().ReverseMap();
        }
    }
}
