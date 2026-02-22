using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastucture.Services.TheMovieDatabase;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class MoviesController(IUnitOfWork unit, TmdbService tmdbService, IMapper mapper) : BaseApiController
{
    [HttpGet("{id:int}")]
    public async Task<ActionResult<MovieDetailsResponse>> GetMovie(int id)
    {
        var movie = await unit.Repository<Movie>().GetByIdAsync(id);
        
        if (movie == null) return NotFound("Movie not found");

        var extraDetails = await tmdbService.GetMovieDetailsAsync(movie.ExternalId);
        
        if (extraDetails == null) return NotFound("Movie details not found");

        var result = mapper.Map<MovieDetailsResponse>(movie);
    
        mapper.Map(extraDetails, result);

        return Ok(result);
    }
}