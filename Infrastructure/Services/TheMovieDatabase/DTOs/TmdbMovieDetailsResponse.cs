namespace Infrastucture.Services.TheMovieDatabase;

public class TmdbMovieDetailsResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Overview { get; set; }
    public float Vote_Average;
    public int Vote_Count { get; set; }
    public int Runtime { get; set; }
    public List<TmdbGenre> Genres { get; set; }
    public TmdbCredits Credits { get; set; }
    public TmdbVideoContainer Videos { get; set; }
    public TmdbImageContainer Images { get; set; }
}

public class TmdbCredits
{
    public List<TmdbCast> Cast { get; set; }
    public List<TmdbCrew> Crew { get; set; }
}

public class TmdbCast
{
    public string Name { get; set; }
    public string Character { get; set; }
    public string Profile_Path { get; set; }
}

public class TmdbCrew
{
    public string Name { get; set; }
    public string Job { get; set; }
}

public class TmdbVideoContainer
{
    public List<TmdbVideo> Results { get; set; }
}

public class TmdbVideo
{
    public string Key { get; set; }
    public string Site { get; set; }
    public string Type { get; set; }
    public string Name { get; set; }
}

public class TmdbImageContainer
{
    public List<TmdbBackdrop> Backdrops { get; set; }
}

public class TmdbBackdrop
{
    public string File_Path { get; set; }
}