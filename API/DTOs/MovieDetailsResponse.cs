namespace API.DTOs;

public class MovieDetailsResponse
{
    public required int Id { get; set; }
    public required string Title { get; set; }
    public required string Status { get; set; }
    public required string Overview { get; set; }
    public required string PosterPath { get; set; }
    public required float VoteAverage { get; set; }
    public required int VoteCount { get; set; }
    public required int Runtime { get; set; }
    public required List<string> Genres { get; set; }
    public required bool Adult { get; set; }
    public required string Director { get; set; }
    public required List<CastDto> Actors { get; set; }
    public required string TrailerUrl { get; set; }
    public required List<string> Images { get; set; }
    public required List<VideoDto> Videos { get; set; }
    public required Dictionary<int, int> RatingDistribution { get; set; }
}
public record CastDto(string Name, string Character, string ProfilePath);
public record VideoDto(string Name, string Key, string Site, string Type);
