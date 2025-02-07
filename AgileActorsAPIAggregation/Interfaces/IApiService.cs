using AgileActorsAPIAggregation.Models;

namespace AgileActorsAPIAggregation.Interfaces
{
    public interface IApiService
    {
        Task<WeatherModel> GetWeatherDataAsync(string CityNameForWeather);
        Task<NewsModel> GetNewsDataAsync(DateOnly from, DateOnly to, string SortByNews);
        Task<GithubModel> GetGithubDataAsync();
    }
}
