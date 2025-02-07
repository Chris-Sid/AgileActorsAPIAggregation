using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AgileActorsAPIAggregation.Models
{
    public class AggregatedDataRequest
    {
        [SwaggerSchema("The city name for weather data")]
        [DefaultValue("Athens")]
        public string CityNameForWeather { get; set; } = "Athens";

        [SwaggerSchema("The start date for news data")]
        [DefaultValue("2025-02-06")]
        public DateOnly NewsFromDate { get; set; }= DateOnly.FromDateTime(DateTime.Now.Date.AddDays(-3));

        [SwaggerSchema("The end date for news data")]
        public DateOnly NewsToDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        [SwaggerSchema("The sort order for news data", Description = "Suggested values give numbers that corresponds to : 0 = relevancy,1 = popularity, 2 = publishedAt")]
        [EnumDataType(typeof(SortNewsBy))]
        public SortNewsBy? SortByNews { get; set; } = SortNewsBy.popularity;

        public enum SortNewsBy
        {
            [Display(Name = "Sort by relevancy")]
            relevancy,
            [Display(Name = "Sort by popularity")]
            popularity,
            [Display(Name = "Sort by published date")]
            publishedAt
        }
    }

    public class SwaggerEnumInfoAttribute : SwaggerSchemaAttribute
    {
        public SwaggerEnumInfoAttribute(string description) : base(description)
        {
        }
    }

}
