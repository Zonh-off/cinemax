using Core.Entities;

namespace Core.Specifications;

public sealed class TicketByCodeSpec : BaseSpecification<OrderTicket>
{
    public TicketByCodeSpec(string ticketCode)
        : base(t => t.TicketCode == ticketCode)
    {
        AddInclude(t => t.Order);
        AddInclude(t => t.Showtime);
        AddInclude(t => t.Showtime.Movie);
        AddInclude(t => t.Showtime.Hall);
        AddInclude(t => t.Showtime.Hall.Cinema);
        AddInclude(t => t.Seat);
    }
}