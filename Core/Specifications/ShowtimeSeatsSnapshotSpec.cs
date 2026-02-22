using Core.Entities;

namespace Core.Specifications;

public sealed class ShowtimeSeatsSnapshotSpec : BaseSpecification<Showtime>
{
    public ShowtimeSeatsSnapshotSpec(int showtimeId)
        : base(s => s.Id == showtimeId)
    {
        AddInclude(s => s.Hall);
        AddInclude(s => s.Hall.Seats);
        AddInclude(s => s.Prices);
    }
}