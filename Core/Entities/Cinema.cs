using Core.Common;

namespace Core.Entities;

public class Cinema : BaseEntity
{
    public required string Name { get; set; } = string.Empty;
    public required string Address { get; set; } = string.Empty;
    
    public required double Latitude { get; set; }
    public required double Longitude { get; set; }
    
    public required int CityId { get; set; }
    public City City { get; set; } = null!;
    
    public ICollection<Hall> Halls { get; set; } = new List<Hall>();
}