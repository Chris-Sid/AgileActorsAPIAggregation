namespace AgileActorsAPIAggregation.Models
{
    public class AggregatedDataResponse
    {
        public string CityNameForWeather { get; set; }
        public NewsModel NewsData { get; set; }
        public WeatherModel WeatherData { get; set; }
    }

}
