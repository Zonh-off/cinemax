using Core.Entities;

namespace Core.Specifications;


public class ShowtimeWithSeatsSpec : BaseSpecification<Showtime>
{
    public ShowtimeWithSeatsSpec(int showtimeId)
        : base(s => s.Id == showtimeId)
    {
        AddInclude(s => s.Hall);
        AddInclude(s => s.Hall.Seats);
    }
}