using Core.Entities;

namespace Core.Specifications;

public sealed class SoldSeatIdsForShowtimeSpec : BaseSpecification<OrderTicket, int>
{
    public SoldSeatIdsForShowtimeSpec(int showtimeId)
        : base(t => t.ShowtimeId == showtimeId)
    {
        AddSelect(t => t.SeatId);
    }
}