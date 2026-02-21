namespace API.DTOs;

public class TicketResponse
{
    public int OrderId { get; set; }
    public int TicketId { get; set; }
    public string TicketCode { get; set; } = "";

    public int ShowtimeId { get; set; }
    public DateTime StartTime { get; set; }

    public int MovieId { get; set; }
    public string MovieTitle { get; set; } = "";
    public string PosterPath { get; set; } = "";

    public int CinemaId { get; set; }
    public string CinemaName { get; set; } = "";
    public string HallName { get; set; } = "";

    public int SeatId { get; set; }
    public int Row { get; set; }
    public int Number { get; set; }
    public decimal UnitPrice { get; set; }

    public string Status { get; set; } = "";
}