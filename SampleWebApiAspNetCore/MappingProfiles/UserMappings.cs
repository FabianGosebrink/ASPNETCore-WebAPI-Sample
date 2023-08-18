using AutoMapper;
using SampleWebApiAspNetCore.Dtos;
using SampleWebApiAspNetCore.Entities;

namespace SampleWebApiAspNetCore.MappingProfiles;

public class UserMappings : Profile
{
    public UserMappings()
    {
        CreateMap<UserRegisterDto, UserEntity>();
    }
}
