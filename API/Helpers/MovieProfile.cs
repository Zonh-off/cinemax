using API.DTOs;
using AutoMapper;
using Core.Entities;

namespace API.Helpers;

public class MovieProfile : Profile
{
    private const string Poster500 = "https://image.tmdb.org/t/p/w500";
    private const string PlaceholderPoster = "https://placehold.co/400x600";

    public MovieProfile()
    {
        CreateMap<Movie, MovieDetailsResponse>()
           .ForMember(d => d.PosterPath, o =>
                          o.MapFrom(s => !string.IsNullOrEmpty(s.PosterPath) ? Poster500 + s.PosterPath : PlaceholderPoster));
    }
}