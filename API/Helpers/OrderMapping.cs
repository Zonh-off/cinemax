using API.DTOs;
using AutoMapper;
using Core.Entities;

namespace API.Helpers;

public class OrderMapping : Profile
{
    public OrderMapping()
    {
        CreateMap<Order, OrderListItemResponse>()
           .ForMember(d => d.OrderId, o => o.MapFrom(s => s.Id))
           .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
           .ForMember(d => d.ShowtimeId, o => o.MapFrom(s => s.ShowtimeId))
           .ForMember(d => d.ShowtimeStartTime, o => o.MapFrom(s => s.Showtime.StartTime))
           .ForMember(d => d.MovieId, o => o.MapFrom(s => s.Showtime.MovieId))
           .ForMember(d => d.MovieTitle, o => o.MapFrom(s => s.Showtime.Movie.Title))
           .ForMember(d => d.PosterPath, o => o.MapFrom(s =>
                                                            !string.IsNullOrEmpty(s.Showtime.Movie.PosterPath)
                                                                ? "https://image.tmdb.org/t/p/w500" + s.Showtime.Movie.PosterPath
                                                                : "https://placehold.co/400x600"))
           .ForMember(d => d.CinemaId, o => o.MapFrom(s => s.Showtime.Hall.CinemaId))
           .ForMember(d => d.CinemaName, o => o.MapFrom(s => s.Showtime.Hall.Cinema.Name))
           .ForMember(d => d.HallName, o => o.MapFrom(s => s.Showtime.Hall.Name))
           .ForMember(d => d.TicketsCount, o => o.MapFrom(s => s.Tickets.Count));
    }
}