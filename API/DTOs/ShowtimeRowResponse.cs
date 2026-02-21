namespace API.DTOs;

public class ShowtimeRowResponse
{
    public int ShowtimeId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public int HallId { get; set; }
    public string HallName { get; set; } = "";

    public int CinemaId { get; set; }
    public string CinemaName { get; set; } = "";

    public int MovieId { get; set; }
    public string MovieTitle { get; set; } = "";
    public string PosterPath { get; set; } = "";
}