namespace Core.Specifications;

public class MovieSpecParams
{
    private const int MaxPageSize = 50;
    public int PageNumber { get; set; } = 1;
    
    private int _pageSize = 5;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
    
    public string? Sort { get; set; }
    
    private string? _search;

    public string Search
    {
        get => _search ?? "";
        set => _search = value.ToLower();
    }

    private List<string>? _genres = [];

    public List<string> Genres
    {
        get => _genres ?? new List<string>();
        set {
            _genres = value.SelectMany(x => x.Split(",", StringSplitOptions.RemoveEmptyEntries)).ToList();
        }
    }

    private string? _status;

    public string Status
    {
        get => _status ?? "now";
        set => _status = value.ToLower();
    }
}