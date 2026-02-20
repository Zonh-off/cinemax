using System.Text.Json;
using Core.Entities;
using Infrastucture.Data;
using Infrastucture.Services.TheMovieDatabase;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class StoreContextSeed
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static async Task SeedAsync(StoreContext context, TmdbService tmdbService)
    {
        if (!await context.Cities.AnyAsync())
        {
            var citiesData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/cities.json");
            var cities = JsonSerializer.Deserialize<List<City>>(citiesData, JsonOptions);
            if (cities is null || cities.Count == 0) return;

            context.Cities.AddRange(cities);
            await context.SaveChangesAsync();
        }

        if (!await context.Cinemas.AnyAsync())
        {
            var cinemasData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/cinemas.json");
            var cinemas = JsonSerializer.Deserialize<List<Cinema>>(cinemasData, JsonOptions);
            if (cinemas is null || cinemas.Count == 0) return;

            // IMPORTANT: cinemas.json should contain CityId (valid FK)
            context.Cinemas.AddRange(cinemas);
            await context.SaveChangesAsync();
        }

        if (!await context.Halls.AnyAsync())
        {
            var hallsData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/halls.json");
            var halls = JsonSerializer.Deserialize<List<Hall>>(hallsData, JsonOptions);
            if (halls is null || halls.Count == 0) return;

            context.Halls.AddRange(halls);
            await context.SaveChangesAsync();
        }

        if (!await context.SeatTypes.AnyAsync())
        {
            var seatTypesData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/seatTypes.json");
            var seatTypes = JsonSerializer.Deserialize<List<SeatType>>(seatTypesData, JsonOptions);
            if (seatTypes is null || seatTypes.Count == 0) return;

            context.SeatTypes.AddRange(seatTypes);
            await context.SaveChangesAsync();
        }

        if (!await context.Seats.AnyAsync())
        {
            // Get seat type ids
            var standardId = await context.SeatTypes
                .Where(x => x.Name == "Standard")
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            var vipId = await context.SeatTypes
                .Where(x => x.Name == "VIP")
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            if (standardId == 0 || vipId == 0)
                throw new Exception("SeatTypes must include 'Standard' and 'VIP'.");

            var hallsFromDb = await context.Halls.AsNoTracking().ToListAsync();
            var allSeats = new List<Seat>(hallsFromDb.Count * 10 * 12);

            foreach (var hall in hallsFromDb)
            {
                for (int row = 1; row <= 10; row++)
                {
                    for (int col = 1; col <= 12; col++)
                    {
                        allSeats.Add(new Seat
                        {
                            Row = row,
                            Number = col,
                            SeatTypeId = row > 8 ? vipId : standardId,
                            HallId = hall.Id
                        });
                    }
                }
            }

            context.Seats.AddRange(allSeats);
            await context.SaveChangesAsync();
        }

        if (!await context.Movies.AnyAsync())
        {
            var nowPlayingMovies = await tmdbService.GetNowPlayingMoviesAsync();

            var movies = nowPlayingMovies.Results.Select(m => new Movie
            {
                ExternalId = m.Id,
                Title = m.Title,
                PosterPath = m.Poster_Path ?? string.Empty,
                ReleaseDate = m.Release_Date
            }).ToList();

            context.Movies.AddRange(movies);
            await context.SaveChangesAsync();
        }

        if (!await context.Showtimes.AnyAsync())
        {
            var showtimesData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/showtimes.json");
            var showtimes = JsonSerializer.Deserialize<List<Showtime>>(showtimesData, JsonOptions);
            if (showtimes is null || showtimes.Count == 0) return;

            foreach (var s in showtimes)
                s.Prices = new List<ShowtimeSeatPrice>();

            context.Showtimes.AddRange(showtimes);
            await context.SaveChangesAsync();
        }

        if (!await context.ShowtimeSeatPrices.AnyAsync())
        {
            var pricesData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/showtimeSeatPrices.json");
            var prices = JsonSerializer.Deserialize<List<ShowtimeSeatPrice>>(pricesData, JsonOptions);
            if (prices is null || prices.Count == 0) return;

            context.ShowtimeSeatPrices.AddRange(prices);
            await context.SaveChangesAsync();
        }
    }
}