using Core.Common;

namespace Core.Entities;

public class Ticket : BaseEntity
{
    public string UserEmail { get; set; } = "";
    public AppUser User { get; set; } = null!;
    
    public int ShowtimeId { get; set; }
    public Showtime Showtime { get; set; } = null!;
    
    public int SeatId { get; set; }
    public Seat Seat { get; set; } = null!;

    public decimal PricePaid { get; set; }
    public DateTime PurchasedAt { get; set; } = DateTime.UtcNow;
}