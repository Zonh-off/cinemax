using Core.Common;

namespace Core.Entities;

public class ShowtimeSeatPrice : BaseEntity
{
    public int ShowtimeId { get; set; }
    public Showtime Showtime { get; set; } = null!;
    
    public int SeatTypeId { get; set; }
    public SeatType SeatType { get; set; } = null!;
    
    public decimal Price { get; set; }
}