using Core.Common;

namespace Core.Entities;

public class City : BaseEntity
{
    public required string Name { get; set; } = string.Empty;
    public required double Latitude { get; set; }
    public required double Longitude { get; set; }
    
    public ICollection<Cinema> Cinemas { get; set; } = new List<Cinema>();
}