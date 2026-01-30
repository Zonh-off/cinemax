using API.DTOs;
using API.Extentions;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastucture.Services.TheMovieDatabase;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class MoviesController(IUnitOfWork unit, TmdbService tmdbService, IMapper mapper) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MoviesResponse>>> GetAllMovies([FromQuery] MovieSpecParams specParams)
    {
        var specification = new MovieSpecification(specParams);
        
        return Ok(await CreatePagedResult(unit.Repository<Movie>(),
                                          specification,
                                          specParams.PageNumber,
                                          specParams.PageSize, 
                                          mapper.Map<MoviesResponse>));
    }
    
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
    
    [HttpGet("genres")]
    public async Task<IReadOnlyList<GenreResponse>> GetGenresAsync()
    {
        var result = await tmdbService.GetGenresAsync();

        return mapper.Map<IReadOnlyList<GenreResponse>>(result.Genres);
    }
}