using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ShowtimesController(IUnitOfWork unit, IMapper mapper) : BaseApiController
{
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