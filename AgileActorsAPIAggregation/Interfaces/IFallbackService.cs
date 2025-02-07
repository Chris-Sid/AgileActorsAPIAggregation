using AgileActorsAPIAggregation.Models;

namespace AgileActorsAPIAggregation.Interfaces
{
    public interface IFallbackService
    {
        WeatherModel GetDefaultWeatherData();
        NewsModel GetDefaultNewsData();
        GithubModel GetDefaultGithubData();
    }
}