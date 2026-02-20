using API.DTOs;
using AutoMapper;
using Core.Entities;
using Infrastucture.Services.TheMovieDatabase;

namespace API.Helpers;

public class CityProfile : Profile
{
    public CityProfile()
    {
        CreateMap<City, CityResponse>();
    }
}


public class CinemaProfile : Profile
{
    public CinemaProfile()
    {
        CreateMap<Cinema, CinemaResponse>();
    }
}

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

public class TmdbProfile : Profile
{
    private const string Profile185 = "https://image.tmdb.org/t/p/w185";
    private const string YoutubeWatch = "https://www.youtube.com/watch?v=";
    private const string Backdrop780 = "https://image.tmdb.org/t/p/w780";

    public TmdbProfile()
    {
        CreateMap<TmdbCast, CastDto>()
            .ForCtorParam("Name", opt => opt.MapFrom(src => src.Name))
            .ForCtorParam("Character", opt => opt.MapFrom(src => src.Character))
            .ForCtorParam("ProfilePath", opt => opt.MapFrom(src =>
                !string.IsNullOrEmpty(src.Profile_Path) ? Profile185 + src.Profile_Path : null));

        CreateMap<TmdbVideo, VideoDto>()
            .ForCtorParam("Name", opt => opt.MapFrom(src => src.Name))
            .ForCtorParam("Key", opt => opt.MapFrom(src => src.Key))
            .ForCtorParam("Site", opt => opt.MapFrom(src => src.Site))
            .ForCtorParam("Type", opt => opt.MapFrom(src => src.Type));

        CreateMap<TmdbMovieDetailsResponse, MovieDetailsResponse>()
            .ForMember(d => d.VoteAverage, o => o.MapFrom(s => Math.Round(s.Vote_Average, 1)))
            .ForMember(d => d.Director, o => o.MapFrom(s =>
                s.Credits.Crew.FirstOrDefault(c => c.Job == "Director").Name ?? "Unknown"))
            .ForMember(d => d.Actors, o => o.MapFrom(s => s.Credits.Cast.Take(10)))
           .ForMember(d => d.TrailerUrl, o => o.MapFrom(s => 
                                                            s.Videos.Results.FirstOrDefault(v => v.Type == "Trailer" && v.Site == "YouTube") != null
                                                                ? "https://www.youtube.com/watch?v=" + s.Videos.Results.First(v => v.Type == "Trailer" && v.Site == "YouTube").Key
                                                                : null))
            .ForMember(d => d.Images, o => o.MapFrom(s =>
                s.Images.Backdrops.Take(8).Select(i => Backdrop780 + i.File_Path).ToList()))
            .ForMember(d => d.Videos, o => o.MapFrom(s => s.Videos.Results.Take(5)))
            .ForMember(d => d.VoteCount, o => o.MapFrom(s => s.Vote_Count))
            .ForMember(d => d.RatingDistribution, o => o.MapFrom(s => RatingDistributionHelper.GenerateFakeDistribution(s.Vote_Average, s.Vote_Count)))
            .ForMember(d => d.Genres, o => o.MapFrom(s => s.Genres.Select(g => g.Name)));
    }
}

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

public static class RatingDistributionHelper
{
    public static Dictionary<int, int> GenerateFakeDistribution(double avg, int total)
    {
        var dist = new Dictionary<int, int>(10);
        for (int i = 1; i <= 10; i++)
        {
            double weight = Math.Exp(-Math.Pow(i - avg, 2) / 4);
            dist.Add(i, (int)(total * weight / 5));
        }

        return dist;
    }
}