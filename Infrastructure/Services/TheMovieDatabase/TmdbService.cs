using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;

namespace Infrastucture.Services.TheMovieDatabase;

public class TmdbService(HttpClient httpClient, IConfiguration configuration)
{
    private readonly string _apiKey = configuration["TheMovieDatabase"];

    public async Task<TmdbNowPlayingResponse> GetNowPlayingMoviesAsync()
    {
        var url = $"movie/now_playing?api_key={_apiKey}";
        
        var response = await httpClient.GetFromJsonAsync<TmdbNowPlayingResponse>(url);
        
        if (response?.Results == null) 
            throw new Exception("No movies found");

        return response;
    }

    public async Task<TmdbGenresResponse> GetGenresAsync()
    {
        var url = $"genre/movie/list?api_key={_apiKey}";
        
        var response = await httpClient.GetFromJsonAsync<TmdbGenresResponse>(url);
        
        if (response == null)
            throw new Exception("No genres found");
        
        return response;
    }
    
    public async Task<TmdbMovieDetailsResponse> GetMovieDetailsAsync(int externalId)
    {
        var url = $"movie/{externalId}?api_key={_apiKey}&append_to_response=credits,videos,images";

        var response = await httpClient.GetFromJsonAsync<TmdbMovieDetailsResponse>(url);
            
        if (response == null)
            throw new Exception($"No details for movie with external id {externalId} found");
        
        return response;
    }
}