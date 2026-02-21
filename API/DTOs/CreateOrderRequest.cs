namespace API.DTOs;

public class CreateOrderRequest
{
    public int ShowtimeId { get; set; }
    public List<int> SeatIds { get; set; } = new();
}