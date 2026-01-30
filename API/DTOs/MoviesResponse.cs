namespace API.DTOs;

public class MoviesResponse
{
    public int Id { get; set; }
    
    public required string Title { get; set; }
    
    public string PosterPath { get; set; }
    
    public float VoteAverage { get; set; }
    
    public float Popularity { get; set; }
    
    public required DateTime ReleaseDate { get; set; }
    
    public List<string> Genres { get; set; }
    
    public required string Status { get; set; }
}