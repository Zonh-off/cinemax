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
    public ICollection<OrderReservedSeat> ReservedSeats { get; set; } = new List<OrderReservedSeat>();
}

public enum OrderStatus
{
    Pending = 0,
    Confirmed = 1,
    Cancelled = 2,
    Expired = 3
}
