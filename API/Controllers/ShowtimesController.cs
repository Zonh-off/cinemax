using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ShowtimesController(IUnitOfWork unit, IMapper mapper, ISeatHoldService holds) : BaseApiController
{
    [HttpGet("/api/showtimes/{showtimeId:int}/seats-snapshot")]
    public async Task<ActionResult<ShowtimeSeatsSnapshotResponse>> GetSeatsSnapshot(int showtimeId)
    {
        var spec = new ShowtimeSeatsSnapshotSpec(showtimeId);
        var showtime = await unit.Repository<Showtime>().GetEntityWithSpec(spec);
        if (showtime == null) return NotFound();

        var soldSpec = new SoldSeatIdsForShowtimeSpec(showtimeId);
        var soldSeatIds = await unit.Repository<OrderTicket>().GetAllEntityWithSpec(soldSpec);

        var held = await holds.GetHeldSeatsAsync(showtimeId);

        var prices = showtime.Prices
           .Select(p => new ShowtimeSeatPriceDto { SeatTypeId = p.SeatTypeId, Price = p.Price })
           .ToList();

        var response = new ShowtimeSeatsSnapshotResponse
        {
            ShowtimeId = showtime.Id,
            HallId = showtime.HallId,
            Seats = showtime.Hall.Seats.Select(s => new ShowtimeSeatDto
            {
                Id = s.Id,
                Row = s.Row,
                Number = s.Number,
                SeatTypeId = s.SeatTypeId
            }).ToList(),
            SoldSeatIds = soldSeatIds.ToList(),
            HeldSeats = held,
            Prices = prices
        };

        return Ok(response);
    }
    
    [HttpGet("paged")]
    public async Task<ActionResult> GetShowtimes([FromQuery] ShowtimesQueryParams specParams)
    {
        var specification = new ShowtimesForMovieSpec(specParams);

        return Ok(await CreatePagedResult(
                      unit.Repository<Showtime>(),
                      specification,
                      specParams.PageNumber,
                      specParams.PageSize,
                      mapper.Map<ShowtimeRowResponse>
                  ));
    }

    [HttpGet]
    public async Task<IReadOnlyList<ShowtimesDayResponse>> GetMovieShowtimesGrouped([FromQuery] ShowtimesQueryParams query)
    {
        var spec = new ShowtimesForMovieSpec(query);

        var entities = await unit.Repository<Showtime>().GetAllEntityWithSpec(spec);

        var rows = mapper.Map<List<ShowtimeRowResponse>>(entities);

        var grouped = rows
           .OrderBy(x => x.StartTime)
           .GroupBy(x => DateOnly.FromDateTime(x.StartTime))
           .Select(g => new ShowtimesDayResponse
            {
                Date = g.Key,
                Showtimes = g.ToList()
            })
           .OrderBy(x => x.Date)
           .ToList();

        return grouped;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ShowtimeRowResponse>> GetShowtime(int id)
    {
        var showtime = await unit.Repository<Showtime>().GetByIdAsync(id);
        
        if (showtime == null) return NotFound();
        
        var result = mapper.Map<ShowtimeRowResponse>(showtime);

        return result;
    }
    
    [HttpGet("/api/showtimes/{showtimeId:int}/seats")]
    public async Task<ActionResult> GetSeatsForShowtime(int showtimeId)
    {
        var spec = new ShowtimeWithSeatsSpec(showtimeId);

        var showtime = await unit.Repository<Showtime>().GetEntityWithSpec(spec);
        if (showtime == null) return NotFound();

        var data = new
        {
            ShowtimeId = showtime.Id,
            HallId = showtime.HallId,
            Seats = showtime.Hall.Seats.Select(seat => new
            {
                seat.Id,
                seat.Row,
                seat.Number,
                seat.SeatTypeId
            }).ToList()
        };

        return Ok(data);
    }
}