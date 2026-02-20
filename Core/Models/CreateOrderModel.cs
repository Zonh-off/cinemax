namespace Core.Models;

public class CreateOrderModel
{
    public int ShowtimeId { get; set; }
    public List<int> SeatIds { get; set; } = new();
}
public class OrderTicketSummaryModel
{
    public int SeatId { get; set; }
    public int Row { get; set; }
    public int Number { get; set; }
    public string SeatType { get; set; } = "";
    public decimal UnitPrice { get; set; }
}

public class OrderSummaryModel
{
    public int OrderId { get; set; }
    public int ShowtimeId { get; set; }
    public DateTime ExpiresAt { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = "";
    public List<OrderTicketSummaryModel> Tickets { get; set; } = new();
}