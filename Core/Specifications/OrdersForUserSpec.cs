using Core.Entities;

namespace Core.Specifications;

public sealed class OrdersForUserSpec : BaseSpecification<Order>
{
    public OrdersForUserSpec(string userId) : base(o => o.UserId == userId)
    {
        AddInclude(o => o.Showtime);
        AddInclude(o => o.Showtime.Movie);
        AddInclude(o => o.Showtime.Hall);
        AddInclude(o => o.Showtime.Hall.Cinema);
        AddInclude(o => o.Tickets);

        AddOrderByDescending(o => o.CreatedAt);
    }
}