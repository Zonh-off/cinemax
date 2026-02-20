namespace Infrastucture.Services.TheMovieDatabase;

public class TmdbMovieDetailsResponse
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Overview { get; set; }
    public required float Vote_Average;
    public required int Vote_Count { get; set; }
    public required int Runtime { get; set; }
    public required List<TmdbGenre> Genres { get; set; }
    public required TmdbCredits Credits { get; set; }
    public required TmdbVideoContainer Videos { get; set; }
    public required TmdbImageContainer Images { get; set; }
}

public class TmdbCredits
{
    public required List<TmdbCast> Cast { get; set; }
    public required List<TmdbCrew> Crew { get; set; }
}

public class TmdbCast
{
    public required string Name { get; set; }
    public required string Character { get; set; }
    public required string Profile_Path { get; set; }
}

public class TmdbCrew
{
    public required string Name { get; set; }
    public required string Job { get; set; }
}

public class TmdbVideoContainer
{
    public required List<TmdbVideo> Results { get; set; }
}

public class TmdbVideo
{
    public required string Key { get; set; }
    public required string Site { get; set; }
    public required string Type { get; set; }
    public required string Name { get; set; }
}

public class TmdbImageContainer
{
    public required List<TmdbBackdrop> Backdrops { get; set; }
}

public class TmdbBackdrop
{
    public required string File_Path { get; set; }
}