namespace API.DTOs;

public class SeatsHoldRequest
{
    public List<int> SeatIds { get; set; } = new();
}