using AgileActorsAPIAggregation.Interfaces;

namespace AgileActorsAPIAggregation.Models
{
    public class AppSettings
    {
        public string WeatherApiUrl { get; set; }
        public string GithubApiUrl { get; set; }
        public string NewsApiUrl { get; set; }
        public string WeatherApiKey { get; set; }
        public string NewsApiKey { get; set; }
    }
}
