namespace API.DTOs;

public class ShowtimeSeatsSnapshotResponse
{
    public int ShowtimeId { get; set; }
    public int HallId { get; set; }

    public List<ShowtimeSeatDto> Seats { get; set; } = new();
    public List<int> SoldSeatIds { get; set; } = new();

    public Dictionary<int, string> HeldSeats { get; set; } = new();

    public List<ShowtimeSeatPriceDto> Prices { get; set; } = new();
}