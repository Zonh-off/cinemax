namespace Infrastucture.Services.TheMovieDatabase;

public class TmdbMovieResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Poster_Path { get; set; }
    public float Vote_Average { get; set; }
    public float Popularity { get; set; }
    public required DateTime Release_Date { get; set; }
    public List<int> Genre_Ids { get; set; }
}