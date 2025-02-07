namespace AgileActorsAPIAggregation.Models
{
    public class NewsRequest
    {
        public string FromDate { get; }
        public string ToDate { get; }
        public string SortBy { get; }

        public NewsRequest(DateOnly from, DateOnly to, string sortBy)
        {
            FromDate = from.ToString("yyyy-MM-dd").Equals("0001-01-01") ? DateTime.Now.Date.AddDays(-4).ToString("yyyy-MM-dd") : from.ToString("yyyy-MM-dd");
            ToDate = to.ToString("yyyy-MM-dd");
            SortBy = string.IsNullOrEmpty(sortBy) ? "popularity" : sortBy.ToLower();
        }
    }
}
