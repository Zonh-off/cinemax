using Core.Entities;

namespace Core.Specifications;

public sealed class TicketsForUserOrderSpec : BaseSpecification<OrderTicket>
{
    public TicketsForUserOrderSpec(int orderId, string userId)
        : base(t => t.OrderId == orderId && t.Order.UserId == userId)
    {
        AddInclude(t => t.Order);
        AddInclude(t => t.Showtime);
        AddInclude(t => t.Showtime.Movie);
        AddInclude(t => t.Showtime.Hall);
        AddInclude(t => t.Showtime.Hall.Cinema);
        AddInclude(t => t.Seat);

        AddOrderBy(t => t.Seat.Row);
    }
}