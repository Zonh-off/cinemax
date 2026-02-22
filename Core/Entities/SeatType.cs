using Core.Common;

namespace Core.Entities;

public class SeatType : BaseEntity
{
    public string Name { get; set; }
    public float Weight { get; set; }
}