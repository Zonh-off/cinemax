namespace API.DTOs;

public class MovieDetailsResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Status { get; set; }
    public string Overview { get; set; }
    public string PosterPath { get; set; }
    public float VoteAverage { get; set; }
    public int VoteCount { get; set; }
    public int Runtime { get; set; }
    public List<string> Genres { get; set; }
    public bool Adult { get; set; }
    public string Director { get; set; }
    public List<CastDto> Actors { get; set; }
    public string TrailerUrl { get; set; }
    public List<string> Images { get; set; }
    public List<VideoDto> Videos { get; set; }
    public Dictionary<int, int> RatingDistribution { get; set; }
}
public record CastDto(string Name, string Character, string ProfilePath);
public record VideoDto(string Name, string Key, string Site, string Type);
