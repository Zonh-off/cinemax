using Core.Common;

namespace Core.Entities;

public class Hall : BaseEntity
{
    public required string Name { get; set; } = string.Empty;
    
    public required int CinemaId { get; set; }
    public Cinema Cinema { get; set; } = null!;
    
    public ICollection<Seat> Seats { get; set; } = new List<Seat>();
    public ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();
}