using AgileActorsAPIAggregation.Interfaces;
using System.Net;

namespace AgileActorsAPIAggregation.Helpers
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpClientWrapper(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<T> GetFromJsonAsync<T>(string requestUri)
        {
            var httpClient = _httpClientFactory.CreateClient();

            // Set User-Agent header if needed
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("MyApp/1.0");

            var response = await httpClient.GetAsync(requestUri);

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                // Handle 403 Forbidden status code
                throw new UnauthorizedAccessException("Access to the resource is forbidden.");
            }

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }
    }
}
