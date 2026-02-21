namespace API.DTOs;

public class ShowtimesDayResponse
{
    public DateOnly Date { get; set; }
    public List<ShowtimeRowResponse> Showtimes { get; set; } = new();
}