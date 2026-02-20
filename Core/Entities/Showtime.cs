using Core.Common;

namespace Core.Entities;

public class Showtime : BaseEntity
{
    public required DateTime StartTime { get; set; }
    public required DateTime EndTime { get; set; }
    
    public required int MovieId { get; set; }
    public Movie Movie { get; set; } = null!;
    
    public required int HallId { get; set; }
    public Hall Hall { get; set; } = null!;
    
    public ICollection<ShowtimeSeatPrice> Prices { get; set; } = new List<ShowtimeSeatPrice>();
}