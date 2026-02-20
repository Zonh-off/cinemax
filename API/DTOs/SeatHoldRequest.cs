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

public class OrderTicketSummary
{
    public int SeatId { get; set; }
    public int Row { get; set; }
    public int Number { get; set; }
    public string SeatType { get; set; } = "";
    public decimal UnitPrice { get; set; }
}

public class OrderSummaryResponse
{
    public int OrderId { get; set; }
    public int ShowtimeId { get; set; }
    public DateTime ExpiresAt { get; set; }
    public decimal TotalPrice { get; set; }
    public List<OrderTicketSummary> Tickets { get; set; } = new();
    public string Status { get; set; } = "";
}