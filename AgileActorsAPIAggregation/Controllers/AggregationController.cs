using AgileActorsAPIAggregation.Interfaces;
using AgileActorsAPIAggregation.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AgileActorsAPIAggregation.Controllers
{
    [Route("[controller]")]
    public class AggregationController : ControllerBase
    {
        private readonly IApiService _apiService;

        public AggregationController(IApiService apiService)
        {
            _apiService = apiService;
        }

        /// <summary>
        /// Sort news by posting 0,1,2  : 0== relevancy , 1 ==popularity , 2 == publishedAt 
        /// </summary>
        /// <param name="request">The request containing the sorting criteria.</param>
        /// <returns>The sorted news data.</returns>
        [HttpPost]
        [Route("getAggregatedData")]
        [SwaggerOperation(Summary = "Aggregates data from multiple sources.", Description = "Retrieves weather and news data based on the provided request.")]
        [SwaggerResponse(200, "Successfully aggregated data", typeof(AggregatedDataResponse))]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<IActionResult> GetAggregatedData([FromBody] AggregatedDataRequest request)
        {
            AggregatedDataModel aggregatedData = new AggregatedDataModel();
            try
            {
                var weatherDataTask = _apiService.GetWeatherDataAsync(request.CityNameForWeather);
                var newsDataTask = _apiService.GetNewsDataAsync(request.NewsFromDate, request.NewsToDate, request?.SortByNews?.ToString());
                var githubDataTask = _apiService.GetGithubDataAsync();

                await Task.WhenAll(weatherDataTask, newsDataTask, githubDataTask);

                aggregatedData.WeatherData = await weatherDataTask;
                aggregatedData.NewsData = await newsDataTask;
                aggregatedData.GithubData = await githubDataTask;

                return Ok(aggregatedData);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = "An error occurred while processing your request." });
            }
        }
    }
}
