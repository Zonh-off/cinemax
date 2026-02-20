namespace Core.Specifications;

public class ShowtimesQueryParams
{
    public int? MovieId { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public int? CityId { get; set; }
    public int? CinemaId { get; set; }
    
    public int PageNumber { get; set; } = 1;

    private const int MaxPageSize = 50;
    private int _pageSize = 10;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }
}