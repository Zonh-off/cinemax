namespace API.DTOs;

public class SeatsHoldRequest
{
    public List<int> SeatIds { get; set; } = new();
}

public class SeatHoldRequest
{
    public int SeatId { get; set; }
}

public class CreateOrderRequest
{
    public int ShowtimeId { get; set; }
    public List<int> SeatIds { get; set; } = new();
}