using Core.Common;

namespace Core.Entities;

public class Seat : BaseEntity
{
    public required int Row { get; set; }
    public required int Number { get; set; }
    
    public required int SeatTypeId { get; set; }
    public SeatType SeatType { get; set; }
    public required int HallId { get; set; }
    public Hall Hall { get; set; } = null!;
}