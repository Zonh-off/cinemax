using Core.Entities;

namespace Core.Specifications;

public class CinemaDetailsSpecification : BaseSpecification<Cinema>
{
    public CinemaDetailsSpecification(int cinemaId) 
        : base(x => x.Id == cinemaId)
    {
        AddInclude("Halls");
        AddInclude("Halls.Showtimes");
        AddInclude("Halls.Showtimes.Movie");
    }
}