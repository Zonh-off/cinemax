namespace Core.Interfaces;

public interface IShowtimeNotifier
{
    Task SeatsConfirmedAsync(int showtimeId, IReadOnlyList<int> seatIds);
}