namespace AgileActorsAPIAggregation.Models
{
    public class AggregatedDataModel
    {
        public WeatherModel WeatherData { get; set; } = new WeatherModel();
        public NewsModel NewsData { get; set; } = new NewsModel();
        public GithubModel GithubData { get; set; } = new GithubModel();
    }
}
