using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class CitiesController(IUnitOfWork unit, IMapper mapper) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CityResponse>>> GetCities()
    {
        var cities = await unit.Repository<City>().GetAllAsync();
       
        if (!cities.Any()) return NotFound("Cities not found");
        
        var result = mapper.Map<IReadOnlyList<CityResponse>>(cities);
        
        return Ok(result);
    }
}