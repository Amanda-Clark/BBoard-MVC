namespace BBoard_MVC.Models
{
    public class HeadlineRoot
    {
        public string status { get; set; }
        public int totalResults { get; set; }
        public List<Article> articles { get; set; }
    }
}
