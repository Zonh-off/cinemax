using Core.Common;

namespace Core.Entities;

public class OrderReservedSeat : BaseEntity
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
}