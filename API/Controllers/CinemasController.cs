using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class CinemasController(IUnitOfWork unit, IMapper mapper) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CinemaResponse>>> GetCinemas([FromQuery] int? cityId)
    {
        var spec = new CinemaSpecification(cityId);
        
        var cinemas = await unit.Repository<Cinema>().GetAllEntityWithSpec(spec);
        
        var result = mapper.Map<IReadOnlyList<CinemaResponse>>(cinemas);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CinemaResponse>> GetCinema(int id)
    {
        var cinema = await unit.Repository<Cinema>().GetByIdAsync(id);

        if (cinema == null) return NotFound();
        
        var result = mapper.Map<CinemaResponse>(cinema);

        return Ok(result);
    }
}