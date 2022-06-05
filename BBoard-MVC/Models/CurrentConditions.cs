using Newtonsoft.Json;

namespace BBoard_MVC.Models
{
    public class CurrentConditions
    {
       [JsonProperty("current")]
        public  string CurTemp { get; set; }
        public string Desc { get; set; }
        public string WindSpd { get; set; }
        public string WindDegree { get; set; }
        public string Visibility { get; set; }
        public string Humidity { get; set;  }
        public string Time { get; set; }
        public string Sunrise { get; set; }

        public string Sunset { get; set; }
        public string Feelslike { get; set; }
        public string Pressure { get; set; }
        public string Dewpoint { get; set; }
        public string Clouds { get; set; }
        public string Uvi { get; set; }
    }
}
