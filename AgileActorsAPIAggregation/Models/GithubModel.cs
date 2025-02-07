namespace AgileActorsAPIAggregation.Models
{
    public class GithubModel
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public Owner Owner { get; set; }
        public string Description { get; set; }
        public int StargazersCount { get; set; }
        public int ForksCount { get; set; }
        public int OpenIssuesCount { get; set; }
        public string HtmlUrl
        {
            get; set;
        }
    }

    public class Owner
    {
        public string Login { get; set; }
        public string AvatarUrl { get; set; }

    }
}
