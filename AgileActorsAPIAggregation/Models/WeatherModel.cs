namespace AgileActorsAPIAggregation.Models
{
    public class WeatherModel
    {
    public string Name { get; set; } //Name == City Name
        public Main Main { get; set; }
        public Weather[] Weather { get; set; }
    }

    public class Main
    {
        public double Temp { get; set; }
        public double Pressure { get; set; }
        public double Humidity { get; set; }
    }

    public class Weather
    {
        public string Main { get; set; }
        public string Description { get; set; }
    }

}
