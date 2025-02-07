using AgileActorsAPIAggregation.Interfaces;
using AgileActorsAPIAggregation.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AgileActorsAPIAggregation.Services
{
    public class ApiService : BaseService, IApiService
    {
        private readonly AppSettings _apiSettings;
        private readonly IFallbackService _fallbackService;
        private readonly NewsService _newsService;

        public ApiService(IHttpClientWrapper httpClientWrapper, IMemoryCache cache, IConfiguration configuration, IFallbackService fallbackService, IOptions<AppSettings> apiSettings, NewsService newsService)
            : base(httpClientWrapper, cache)
        {
            _fallbackService = fallbackService;
            _apiSettings = apiSettings.Value;
            _newsService = newsService; 
        }

        public async Task<WeatherModel> GetWeatherDataAsync(string CityNameForWeather)
        {
            try
            {
                return await GetFromCacheOrFetchAsync<WeatherModel>("weatherData", $"{_apiSettings.WeatherApiUrl}?q={CityNameForWeather}&appid={_apiSettings.WeatherApiKey}", _fallbackService.GetDefaultWeatherData);
            }
            catch (HttpRequestException)
            {
                return _fallbackService.GetDefaultWeatherData();
            }
        }

        public async Task<NewsModel> GetNewsDataAsync(DateOnly from, DateOnly to, string SortByNews)
        {
            try
            {
                var newsRequest = new NewsRequest(from, to, SortByNews);
                var newsData = await _newsService.GetNewsDataAsync(newsRequest);
                return newsData;
            }
            catch (HttpRequestException)
            {
                return _fallbackService.GetDefaultNewsData();
            }
        }

        public async Task<GithubModel> GetGithubDataAsync()
        {
            try
            {
                return await GetFromCacheOrFetchAsync<GithubModel>("githubData", _apiSettings.GithubApiUrl, _fallbackService.GetDefaultGithubData);
            }
            catch (HttpRequestException)
            {
                return _fallbackService.GetDefaultGithubData();
            }
        }
    }
}
