using Core.Entities;
using Infrastucture.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Admin;

public class CreateShowtimesRequest
{
    public int CinemaId { get; set; }
    public int HallId { get; set; }
    public List<int> MovieIds { get; set; } = new();

    public DateOnly FromDate { get; set; }
    public DateOnly ToDate { get; set; }

    public List<TimeOnly> Times { get; set; } = new();

    public int DurationMinutes { get; set; } = 120;
}

public class SetShowtimePricesRequest
{
    public int ShowtimeId { get; set; }
    public Dictionary<int, decimal> PricesBySeatTypeId { get; set; } = new();
}

[Authorize(Roles="Admin")]
[Route("api/admin/showtimes")]
public class AdminShowtimesController(StoreContext context) : ControllerBase
{
    [HttpPost("generate")]
    public async Task<ActionResult> Generate([FromBody] CreateShowtimesRequest req)
    {
        var from = req.FromDate.ToDateTime(new TimeOnly(0,0));
        var to = req.ToDate.ToDateTime(new TimeOnly(0,0));

        var showtimes = new List<Showtime>();

        for (var day = from.Date; day <= to.Date; day = day.AddDays(1))
        {
            foreach (var movieId in req.MovieIds.Distinct())
            foreach (var time in req.Times.Distinct())
            {
                var start = new DateTime(day.Year, day.Month, day.Day, time.Hour, time.Minute, 0, DateTimeKind.Utc);
                var end = start.AddMinutes(req.DurationMinutes);

                showtimes.Add(new Showtime
                {
                    MovieId = movieId,
                    HallId = req.HallId,
                    StartTime = start,
                    EndTime = end
                });
            }
        }

        context.Showtimes.AddRange(showtimes);
        await context.SaveChangesAsync();

        return Ok(new { created = showtimes.Count });
    }
    
    [HttpPost("showtime-prices")]
    public async Task<ActionResult> SetPrices([FromBody] SetShowtimePricesRequest req)
    {
        var existing = context.ShowtimeSeatPrices.Where(p => p.ShowtimeId == req.ShowtimeId);
        context.ShowtimeSeatPrices.RemoveRange(existing);

        var rows = req.PricesBySeatTypeId.Select(kv => new ShowtimeSeatPrice
        {
            ShowtimeId = req.ShowtimeId,
            SeatTypeId = kv.Key,
            Price = kv.Value
        }).ToList();

        context.ShowtimeSeatPrices.AddRange(rows);
        await context.SaveChangesAsync();

        return Ok(new { updated = rows.Count });
    }
    
    [HttpPut("{orderId:int}/status")]
    public async Task<ActionResult> UpdateStatus(int orderId, [FromBody] UpdateOrderStatusRequest req)
    {
        var order = await context.Orders.FindAsync(orderId);
        if (order == null) return NotFound();

        order.Status = req.Status;
        await context.SaveChangesAsync();

        return Ok(new { orderId, status = order.Status.ToString() });
    }
}