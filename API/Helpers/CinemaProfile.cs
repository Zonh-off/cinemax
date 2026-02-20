using API.DTOs;
using AutoMapper;
using Core.Entities;

namespace API.Helpers;

public class CinemaProfile : Profile
{
    public CinemaProfile()
    {
        CreateMap<Cinema, CinemaResponse>();
    }
}