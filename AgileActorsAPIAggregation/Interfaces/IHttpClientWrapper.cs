namespace AgileActorsAPIAggregation.Interfaces
{
    public interface IHttpClientWrapper
    {
        Task<T> GetFromJsonAsync<T>(string requestUri);
    }
}
