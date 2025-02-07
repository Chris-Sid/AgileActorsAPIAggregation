using AgileActorsAPIAggregation.Interfaces;
using AgileActorsAPIAggregation.Models;
using AgileActorsAPIAggregation.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using System.Diagnostics;
using System.Net.Http.Json;

namespace ApiServiceTests
{
    public class ApiServiceTests
    {
        private readonly Mock<IHttpClientWrapper> _httpClientMock;
        private readonly Mock<IMemoryCache> _memoryCacheMock;
        private readonly Mock<IOptions<AppSettings>> _appSettingsMock;
        private readonly Mock<IFallbackService> _fallbackServiceMock;
        private readonly NewsService _newsService;
        private readonly ApiService _apiService;

        public ApiServiceTests()
        {
            _httpClientMock = new Mock<IHttpClientWrapper>();
            _memoryCacheMock = new Mock<IMemoryCache>();
            _appSettingsMock = new Mock<IOptions<AppSettings>>();
            _fallbackServiceMock = new Mock<IFallbackService>();
          

            var appSettings = new AppSettings
            {
                WeatherApiUrl = "https://api.openweathermap.org/data/2.5/weather",
                WeatherApiKey = "565f5ed8323c94d8aacae2382f04f4bc",
                NewsApiUrl = $"https://newsapi.org/v2/everything?q=apple&from={DateTime.Now.AddDays(-3).Date}&to={DateTime.Now.Date}&sortBy=popularity&apiKey=9e869ac50c7242529420350254da0b0f",
                GithubApiUrl = "https://api.github.com/repos/dotnet/aspnetcore"
            };

            _appSettingsMock.Setup(x => x.Value).Returns(appSettings);


            _newsService = new NewsService(_httpClientMock.Object, _memoryCacheMock.Object, _appSettingsMock.Object.Value, _fallbackServiceMock.Object);

            _apiService = new ApiService(
                      _httpClientMock.Object,
                      _memoryCacheMock.Object,
                      Mock.Of<IConfiguration>(),
                      _fallbackServiceMock.Object,
                      _appSettingsMock.Object,
                      _newsService);
        }

        [Fact]
        public async Task GetWeatherDataAsync_ReturnsWeatherData()
        {
            var weatherData = new WeatherModel
            {
                Name = "London",
                Main = new Main { Temp = 15, Pressure = 1012, Humidity = 80 },
                Weather = new[] { new Weather { Main = "Clouds", Description = "overcast clouds" } }
            };

            _httpClientMock.Setup(client => client.GetFromJsonAsync<WeatherModel>(It.IsAny<string>()))
                .ReturnsAsync(weatherData);

            object cachedData;
            _memoryCacheMock.Setup(m => m.TryGetValue(It.IsAny<object>(), out cachedData)).Returns(false);
            _memoryCacheMock.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>);

            var result = await _apiService.GetWeatherDataAsync("Athens");

            Assert.NotNull(result);
            Assert.Equal(15, result.Main.Temp);
            Assert.Equal("London", result.Name);
 
            Debug.WriteLine($"Test completed. Result: {result.Name}, Temp: {result.Main.Temp}");
        }

        [Fact]
        public async Task GetWeatherDataAsync_UsesFallback_OnHttpRequestException()
        {
            // Arrange
            var fallbackWeatherData = new WeatherModel
            {
                Name = "N/A",
                Main = new Main { Temp = 0, Pressure = 0, Humidity = 0 },
                Weather = new[] { new Weather { Main = "Unavailable", Description = "No data available" } }
            };

            _httpClientMock.Setup(client => client.GetFromJsonAsync<WeatherModel>(It.IsAny<string>()))
                .ThrowsAsync(new HttpRequestException());
            _fallbackServiceMock.Setup(f => f.GetDefaultWeatherData()).Returns(fallbackWeatherData);

            object cachedData;
            _memoryCacheMock.Setup(m => m.TryGetValue(It.IsAny<object>(), out cachedData)).Returns(false);
            _memoryCacheMock.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>);

            Debug.WriteLine("Starting test for GetWeatherDataAsync_UsesFallback_OnHttpRequestException.");

            var result = await _apiService.GetWeatherDataAsync("London");

            Assert.NotNull(result);
            Assert.Equal("N/A", result.Name);

            Debug.WriteLine($"Test completed. Result: {result.Name}");
        }
    }


}