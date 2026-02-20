using Core.Common;

namespace Core.Entities;


public class Order : BaseEntity
{
    public required string UserId { get; set; }
    public AppUser User { get; set; } = null!;

    public required int ShowtimeId { get; set; }
    public Showtime Showtime { get; set; } = null!;

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    public decimal TotalPrice { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ExpiresAt { get; set; }

    public string? PaymentProvider { get; set; }
    public string? PaymentIntentId { get; set; } 
    public string? PaymentStatus { get; set; } 

    public ICollection<OrderTicket> Tickets { get; set; } = new List<OrderTicket>();
}

public class OrderTicket : BaseEntity
{
    public required int OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public required int ShowtimeId { get; set; }
    public Showtime Showtime { get; set; } = null!;

    public required int SeatId { get; set; }
    public Seat Seat { get; set; } = null!;

    public required int SeatTypeId { get; set; }
    public SeatType SeatType { get; set; } = null!;

    public decimal UnitPrice { get; set; }

    public string TicketCode { get; set; } = Guid.NewGuid().ToString("N");
}

public enum OrderStatus
{
    Pending = 0,
    Confirmed = 1,
    Cancelled = 2,
    Expired = 3
}
