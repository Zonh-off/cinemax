using API.DTOs;
using AutoMapper;
using Core.Entities;
using Infrastucture.Services.TheMovieDatabase;

namespace API.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<TmdbGenre, GenreResponse>();
        
        CreateMap<Movie, MoviesResponse>()
           .ForMember(d => d.VoteAverage, o => o.MapFrom(s => Math.Round(s.VoteAverage, 1)))
           .ForMember(d => d.Status, 
                      o => 
                          o.MapFrom(s => s.Status.ToString()))
           .ForMember(d => d.PosterPath, o => 
                          o.MapFrom(s =>  !string.IsNullOrEmpty(s.PosterPath) 
                                        ? "https://image.tmdb.org/t/p/w500" + s.PosterPath 
                                        : "https://placehold.co/400x600"));

        CreateMap<TmdbCast, CastDto>()
            .ForCtorParam("Name", opt => opt.MapFrom(src => src.Name))
            .ForCtorParam("Character", opt => opt.MapFrom(src => src.Character))
            .ForCtorParam("ProfilePath", opt => opt.MapFrom(src => 
                !string.IsNullOrEmpty(src.Profile_Path) 
                    ? "https://image.tmdb.org/t/p/w185" + src.Profile_Path 
                    : null));

        CreateMap<TmdbVideo, VideoDto>()
            .ForCtorParam("Name", opt => opt.MapFrom(src => src.Name))
            .ForCtorParam("Key", opt => opt.MapFrom(src => src.Key))
            .ForCtorParam("Site", opt => opt.MapFrom(src => src.Site))
            .ForCtorParam("Type", opt => opt.MapFrom(src => src.Type));
        
        CreateMap<Movie, MovieDetailsResponse>()
           .ForMember(d => d.Status, 
                      o => o.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.PosterPath, o => 
                           o.MapFrom(s =>  !string.IsNullOrEmpty(s.PosterPath) 
                                         ? "https://image.tmdb.org/t/p/w500" + s.PosterPath 
                                         : "https://placehold.co/400x600"))
            .ForMember(d => d.VoteAverage, o => o.MapFrom(s => Math.Round(s.VoteAverage, 1)));
        
        CreateMap<TmdbMovieDetailsResponse, MovieDetailsResponse>()
            .ForMember(d => d.Director, o => o.MapFrom(s => 
                s.Credits.Crew.FirstOrDefault(c => c.Job == "Director").Name ?? "Unknown"))
            .ForMember(d => d.Actors, o => o.MapFrom(s => s.Credits.Cast.Take(10)))
            .ForMember(d => d.TrailerUrl, o => o.MapFrom(s => 
                s.Videos.Results.FirstOrDefault(v => v.Type == "Trailer" && v.Site == "YouTube") != null
                ? "https://www.youtube.com/watch?v=" + s.Videos.Results.First(v => v.Type == "Trailer" && v.Site == "YouTube").Key
                : null))
            .ForMember(d => d.Images, o => o.MapFrom(s => 
                s.Images.Backdrops.Take(8).Select(i => "https://image.tmdb.org/t/p/w780" + i.File_Path).ToList()))
            .ForMember(d => d.Videos, o => o.MapFrom(s => s.Videos.Results.Take(5)))
            .ForMember(d => d.VoteCount, o => o.MapFrom(s => s.Vote_Count))
            .ForMember(d => d.RatingDistribution, o => o.MapFrom(s => GenerateFakeDistribution(s.Vote_Average, s.Vote_Count)))
           .ForMember(d => d.Genres, o => o.MapFrom(s => s.Genres.Select(g => g.Name)));
    }
    
    private Dictionary<int, int> GenerateFakeDistribution(double avg, int total)
    {
        var dist = new Dictionary<int, int>();
        for (int i = 1; i <= 10; i++)
        {
            double weight = Math.Exp(-Math.Pow(i - avg, 2) / 4);
            dist.Add(i, (int)(total * weight / 5));
        }
        return dist;
    }
}