using API.DTOs;
using Core.Entities;

namespace API.Extentions;

public static class MovieMappingExtentions
{
    public static MoviesResponse? ToDTO(this Movie movie)
    {
        if (movie is null) return null;

        return new MoviesResponse()
        {
            Id = movie.Id,
            Title = movie.Title,
            PosterPath = movie.PosterPath,
            VoteAverage = movie.VoteAverage,
            Popularity = movie.Popularity,
            ReleaseDate = movie.ReleaseDate,
            Genres = movie.Genres,
            Status = movie.Status.ToString()
        };
    }
}