namespace API.DTOs;

public class ShowtimeSeatDto
{
    public int Id { get; set; }
    public int Row { get; set; }
    public int Number { get; set; }
    public int SeatTypeId { get; set; }
}

public class ShowtimeSeatPriceDto
{
    public int SeatTypeId { get; set; }
    public decimal Price { get; set; }
}

public class ShowtimeSeatsSnapshotResponse
{
    public int ShowtimeId { get; set; }
    public int HallId { get; set; }

    public List<ShowtimeSeatDto> Seats { get; set; } = new();
    public List<int> SoldSeatIds { get; set; } = new();

    // seatId -> userId (для UI можна замінити на bool, якщо не хочеш світити userId)
    public Dictionary<int, string> HeldSeats { get; set; } = new();

    public List<ShowtimeSeatPriceDto> Prices { get; set; } = new();
}