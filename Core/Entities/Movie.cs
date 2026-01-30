using Core.Common;

namespace Core.Entities;

public enum MovieStatus
{
    Now,
    Soon,
    Ended
}

public class Movie : BaseEntity
{
    public required int ExternalId { get; set; }
    public required string Title { get; set; }
    public string PosterPath { get; set; } = string.Empty;
    public float VoteAverage { get; set; }
    public float Popularity { get; set; }
    public required DateTime ReleaseDate { get; set; }
    public List<string> Genres { get; set; } = [];
    public required MovieStatus Status { get; set; }
}