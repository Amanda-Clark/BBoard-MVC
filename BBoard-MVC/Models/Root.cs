namespace BBoard_MVC.Models
{
    public class Root
    {
        public double lat { get; set; }
        public double lon { get; set; }
        public string timezone { get; set; }
        public int timezone_offset { get; set; }
        public Current current { get; set; }
        public List<Daily> daily { get; set; }
        public List<Alert> alerts { get; set; }
        public string status { get; set; }
        public int totalResults { get; set; }
        public int numAlerts { get; set; }
        public int numStories { get; set; }
        public List<Article> articles { get; set; }
        public List<Article> covid_articles { get; set; }
        public bool Empty { get { return (alerts == null); } }
    }
}
