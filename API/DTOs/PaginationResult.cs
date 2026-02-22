namespace API.DTOs;

public class PaginationResult<T>(int pageNumber, int pageSize, int totalItems, IReadOnlyList<T> data)
{
    public int PageNumber { get; set; } = pageNumber;
    public int PageSize { get; set; } = pageSize;
    public int TotalItems { get; set; } = totalItems;
    public IReadOnlyList<T> Data { get; set; } = data;
}