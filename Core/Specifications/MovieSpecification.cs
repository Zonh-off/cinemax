using Core.Entities;

namespace Core.Specifications;

public class MovieSpecification : BaseSpecification<Movie>
{
    public MovieSpecification(MovieSpecParams specParams)
        : base(x =>
                   // Search
                   (string.IsNullOrEmpty(specParams.Search) || x.Title.ToLower().Contains(specParams.Search)) 
                   // Status
                   && (string.IsNullOrEmpty(specParams.Status) 
                       || x.Status == Enum.Parse<MovieStatus>(specParams.Status, true))
                   // Genres
                   && (!specParams.Genres.Any() || x.Genres.Any(g => specParams.Genres.Contains(g))))
    {
        ApplyPaging(specParams.PageSize * (specParams.PageNumber - 1), specParams.PageSize);
        
        switch (specParams.Sort)
        {
            case "trending":
                AddOrderByDescending(x => x.Status == MovieStatus.Now); 
                AddOrderByDescending(x => x.ReleaseDate);
                AddOrderByDescending(x => x.Popularity);
                break;
            
            case "popular":
                AddOrderByDescending(x => x.Popularity);
                break;
            
            case "new":
                AddOrderByDescending(x => x.ReleaseDate);
                break;

            default:
                AddOrderByDescending(x => x.Status == MovieStatus.Now); 
                AddOrderByDescending(x => x.ReleaseDate);
                AddOrderByDescending(x => x.Popularity);
                break;
        }
    }
}