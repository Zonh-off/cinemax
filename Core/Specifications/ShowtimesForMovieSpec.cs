using Core.Entities;

namespace Core.Specifications;

public sealed class ShowtimesForMovieSpec : BaseSpecification<Showtime>
{
    public ShowtimesForMovieSpec(ShowtimesQueryParams p)
        : base(s =>
                   (!p.MovieId.HasValue || s.MovieId == p.MovieId.Value) &&
                   (
                       (!p.From.HasValue && s.StartTime >= DateTime.UtcNow) ||
                       (p.From.HasValue && s.StartTime >= p.From.Value)
                   ) &&
                   (!p.To.HasValue || s.StartTime < p.To.Value) &&
                   (!p.CityId.HasValue || s.Hall.Cinema.CityId == p.CityId.Value) &&
                   (!p.CinemaId.HasValue || s.Hall.CinemaId == p.CinemaId.Value)
        )
    {
        AddInclude(x => x.Movie);
        AddInclude(s => s.Hall);
        AddInclude(s => s.Hall.Cinema);
        AddInclude(s => s.Prices);
        AddOrderBy(s => s.StartTime);
    }
}
