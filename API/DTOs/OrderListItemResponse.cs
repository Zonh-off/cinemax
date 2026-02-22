namespace API.DTOs;

public class OrderListItemResponse
{
    public int OrderId { get; set; }
    public string Status { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public decimal TotalPrice { get; set; }

    public int ShowtimeId { get; set; }
    public DateTime ShowtimeStartTime { get; set; }

    public int MovieId { get; set; }
    public string MovieTitle { get; set; } = "";
    public string PosterPath { get; set; } = "";

    public int CinemaId { get; set; }
    public string CinemaName { get; set; } = "";
    public string HallName { get; set; } = "";

    public int TicketsCount { get; set; }
}