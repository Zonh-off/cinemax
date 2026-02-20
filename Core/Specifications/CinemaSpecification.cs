using Core.Entities;

namespace Core.Specifications;

public class CinemaSpecification : BaseSpecification<Cinema>
{
    public CinemaSpecification(int? cityId) 
        : base(x => !cityId.HasValue || x.CityId == cityId)
    {
        
    }
}