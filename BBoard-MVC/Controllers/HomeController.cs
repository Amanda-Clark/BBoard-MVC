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
            rootClass = GetHeadlineData().Result;
            var covidStories = GetCovidData().Result;
            dynamicModel.rootClass = rootClass;
           // dynamicModel.usStoriesClass = usStoriesClass;
            dynamicModel.covidStories = covidStories;
            // var  usStoriesClass = GetHeadlineData().Result;

            // var covidStories = GetCovidData().Result;

            //ViewBag.CurrentCondition = curCon;

            return View(dynamicModel);
        }

     

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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


       
        public async Task<Root> GetHeadlineData()
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
                myDeserializedClass = JsonConvert.DeserializeObject<Root>(responseBody);

                return myDeserializedClass;

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