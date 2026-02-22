using Core.Common;

namespace Core.Entities;

public class Movie : BaseEntity
{
    public required int ExternalId { get; set; }
    public required string Title { get; set; }
    public string PosterPath { get; set; } = string.Empty;
    public required DateTime ReleaseDate { get; set; }

    public ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();
}