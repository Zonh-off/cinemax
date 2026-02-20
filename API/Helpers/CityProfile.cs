using API.DTOs;
using AutoMapper;
using Core.Entities;

namespace API.Helpers;

public class CityProfile : Profile
{
    public CityProfile()
    {
        CreateMap<City, CityResponse>();
    }
}