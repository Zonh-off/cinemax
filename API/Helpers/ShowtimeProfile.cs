using API.DTOs;
using AutoMapper;
using Core.Entities;

namespace API.Helpers;

public class ShowtimeProfile : Profile
{
    private const string Poster500 = "https://image.tmdb.org/t/p/w500";
    private const string PlaceholderPoster = "https://placehold.co/400x600";

    public ShowtimeProfile()
    {
        CreateMap<Showtime, ShowtimeRowResponse>()
           .ForMember(d => d.ShowtimeId, o => o.MapFrom(s => s.Id))
           .ForMember(d => d.HallName, o => o.MapFrom(s => s.Hall.Name))
           .ForMember(d => d.CinemaId, o => o.MapFrom(s => s.Hall.CinemaId))
           .ForMember(d => d.CinemaName, o => o.MapFrom(s => s.Hall.Cinema.Name))
           .ForMember(d => d.MovieTitle, o => o.MapFrom(s => s.Movie.Title))
           .ForMember(d => d.PosterPath, o => o.MapFrom(s =>
                                                            !string.IsNullOrEmpty(s.Movie.PosterPath) ? Poster500 + s.Movie.PosterPath : PlaceholderPoster));
    }
}