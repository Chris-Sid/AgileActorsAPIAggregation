using AgileActorsAPIAggregation.Interfaces;
using AgileActorsAPIAggregation.Models;

namespace AgileActorsAPIAggregation.Services
{
    public class FallbackService : IFallbackService
    {
        public WeatherModel GetDefaultWeatherData()
        {
            return new WeatherModel
            {
                Name = "N/A",
                Main = new Main { Temp = 0, Pressure = 0, Humidity = 0 },
                Weather = new Weather[] { new Weather { Main = "Unavailable", Description = "No data available" } }
            };
        }

        public NewsModel GetDefaultNewsData()
        {
            return new NewsModel
            {
                Status = "Unavailable",
                TotalResults = 0,
                Articles = new Article[]
                {
                new Article
                {
                    Source = new Source { Id = "N/A", Name = "N/A" },
                    Author = "N/A",
                    Title = "Unavailable",
                    Description = "No news data available",
                    Url = "N/A",
                    UrlToImage = "N/A",
                    PublishedAt = "N/A",
                    Content = "N/A"
                }
                }
            };
        }

        public GithubModel GetDefaultGithubData()
        {
            return new GithubModel
            {
                Name = "Unavailable",
                Description = "GitHub data is currently unavailable."
            };
        }
    }

}
