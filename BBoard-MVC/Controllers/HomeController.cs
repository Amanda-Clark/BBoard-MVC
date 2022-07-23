using BBoard_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text;
using System.Dynamic;

namespace BBoard_MVC.Controllers
{
    public class HomeController : Controller
    {
        
       

        public HomeController()
        {
           
        }

        public  IActionResult Index()
        {

            dynamic dynamicModel = new ExpandoObject();
            var rootClass = GetWeatherData().Result;
            var newsStories = GetHeadlineData(rootClass).Result;
            rootClass.articles = newsStories.articles;

            var covidStories = GetCovidData().Result;
            if(rootClass.alerts != null)
            {
                rootClass.numAlerts = rootClass.alerts.Count;
            }
           
            rootClass.numStories = rootClass.articles.Count;
            rootClass.covid_articles = covidStories.articles;

            for (var i = 0; i<5; i++)
            {
                DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                DateTime forecastDate = origin.AddSeconds(rootClass.daily[i].dt);
      

                rootClass.daily[i].ForecastDate = forecastDate.ToString().Substring(0,9);
              


            }

            if (rootClass.alerts != null)
            {
                foreach (var alert in rootClass.alerts)
                {
                    DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                    DateTime startTime = origin.AddSeconds(alert.start);
                    DateTime endTime = origin.AddSeconds(alert.end);

                    alert.actualEndTime = endTime.ToString();
                    alert.actualStartTime = startTime.ToString();
                   
                }
            }
          

            return View(rootClass);
        }

     

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<Root> FooBar()
        {
            using (var client = new HttpClient())

            {
                Root myDeserializedClass = null;
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0)");
                var NewsKey = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("APIKeys")["News"];

                string url = "https://newsapi.org/v2/top-headlines?country=us&pageSize=15&" +
                            "apiKey=" + NewsKey;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                JsonNode curHeadlines = JsonNode.Parse(responseBody);
                myDeserializedClass = JsonConvert.DeserializeObject<Root>(responseBody);

                return myDeserializedClass;

            }

        }

        public async Task<Root> GetWeatherData()
        {
            using (var client = new HttpClient())

            {
                Root myDeserializedClass = null;
                var WeatherKey = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("APIKeys")["Weather"];
                var Latitude = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("LocationData")["Latitude"];
                var Longitude = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("LocationData")["Longitude"];
                client.BaseAddress = new Uri(@"https://api.openweathermap.org/data/2.5/onecall?lat=" + Latitude + "&lon="
                          + Longitude + "&units=imperial&exclude=minutely,hourly&appid=" + WeatherKey);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                JsonNode curweather = JsonNode.Parse(responseBody);
                myDeserializedClass = JsonConvert.DeserializeObject<Root>(responseBody);
               
                return myDeserializedClass;

            }

        }


       
        public async Task<Root> GetHeadlineData(Root rootClass)
        {
            using (var client = new HttpClient())

            {
                Root myDeserializedClass = null;
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0)");
                var NewsKey = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("APIKeys")["News"];

                string url = "https://newsapi.org/v2/top-headlines?country=us&pageSize=15&"+
                            "apiKey=" + NewsKey;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                JsonNode curHeadlines = JsonNode.Parse(responseBody);
                rootClass = JsonConvert.DeserializeObject<Root>(responseBody);

                return rootClass;

            }

        }

        public async Task<Root> GetCovidData()
        {
            using (var client = new HttpClient())

            {
                Root myDeserializedClass = null;
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0)");
                var NewsKey = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("APIKeys")["News"];

                client.BaseAddress = new Uri(@"https://newsapi.org/v2/top-headlines?country=us&q=covid&pageSize=15&" +
                            "apiKey=" + NewsKey);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync(client.BaseAddress).Result;
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                JsonNode curHeadlines = JsonNode.Parse(responseBody);
                myDeserializedClass = JsonConvert.DeserializeObject<Root>(responseBody);

                return myDeserializedClass;

            }

        }




    }

 
}