using API.DTOs;
using Core.Common;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    protected async Task<PaginationResult<TDto>> CreatePagedResult<T, TDto>(
        IGenericRepository<T> repository, 
        ISpecification<T> spec, 
        int pageIndex, 
        int pageSize, 
        Func<T, TDto> mapper) where T : BaseEntity
    {
        var items = await repository.GetAllEntityWithSpec(spec);
        var count = await repository.CountAsync(spec);

        var dtoItems = items.Select(mapper).ToList();

        return new PaginationResult<TDto>(pageIndex, pageSize, count, dtoItems);
    }
}