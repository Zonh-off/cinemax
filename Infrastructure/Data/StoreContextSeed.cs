using Core.Entities;
using Infrastucture.Data;
using Infrastucture.Services.TheMovieDatabase;

namespace Infrastructure.Data;

public class StoreContextSeed
{
    public static async Task SeedAsync(StoreContext context, TmdbService tmdbService)
    {
        if (!context.Movies.Any())
        {
            var nowPlayingTask = tmdbService.GetNowPlayingMoviesAsync();
            var genresTask = tmdbService.GetGenresAsync();
            
            await Task.WhenAll(nowPlayingTask, genresTask);
            
            var nowPlayingMovies = await nowPlayingTask;
            var genres = await genresTask;
            
            context.Movies.AddRange(nowPlayingMovies.Results.Select(m => new Movie
            {
                ExternalId = m.Id,
                Title = m.Title,
                PosterPath = m.Poster_Path,
                VoteAverage = m.Vote_Average,
                Popularity = m.Popularity,
                ReleaseDate = m.Release_Date,
                Genres = genres.Genres
                   .Where(g => m.Genre_Ids.Contains(g.Id))
                   .Select(g => g.Name)
                   .ToList(),
                Status = MovieStatus.Now
            }).ToList());
            
            await context.SaveChangesAsync();
        }
    }
}