using AgileActorsAPIAggregation.Helpers;
using AgileActorsAPIAggregation.Interfaces;
using AgileActorsAPIAggregation.Models;
using Microsoft.Extensions.Caching.Memory;

namespace AgileActorsAPIAggregation.Services
{
    public class NewsService : BaseService
    {
        private readonly AppSettings _apiSettings;
        private readonly IFallbackService _fallbackService;

        public NewsService(IHttpClientWrapper httpClientWrapper, IMemoryCache cache, AppSettings apiSettings, IFallbackService fallbackService) : base(httpClientWrapper, cache)
        {
            _apiSettings = apiSettings;
            _fallbackService = fallbackService;
        }

        public async Task<NewsModel> GetNewsDataAsync(NewsRequest request)
        {
            try
            {
                string url = BuildNewsApiUrl(request);

                return await GetFromCacheOrFetchAsync<NewsModel>("newsData", url, _fallbackService.GetDefaultNewsData);
            }
            catch (HttpRequestException)
            {
                return _fallbackService.GetDefaultNewsData();
            }
        }

        private string BuildNewsApiUrl(NewsRequest request)
        {
            switch (request.SortBy)
            {
                case "relevancy":
                    return $"{_apiSettings.NewsApiUrl}?q=apple&from={request.FromDate}&to={request.ToDate}&sortBy=relevancy&apiKey={_apiSettings.NewsApiKey}";
                case "popularity":
                    return $"{_apiSettings.NewsApiUrl}?q=apple&from={request.FromDate}&to={request.ToDate}&sortBy=popularity&apiKey={_apiSettings.NewsApiKey}";
                case "publishedat":
                    return $"{_apiSettings.NewsApiUrl}?q=apple&from={request.FromDate}&to={request.ToDate}&sortBy=publishedAt&apiKey={_apiSettings.NewsApiKey}";
                default:
                    return $"{_apiSettings.NewsApiUrl}?q=apple&from={request.FromDate}&to={request.ToDate}&sortBy=popularity&apiKey={_apiSettings.NewsApiKey}";
            }
        }
    }

}
