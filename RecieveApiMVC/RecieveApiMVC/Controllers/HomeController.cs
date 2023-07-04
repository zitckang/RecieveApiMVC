using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RecieveApiMVC.Models;
using System.Data;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace RecieveApiMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        string baseUrl = "https://localhost:44353/"; // This is the base URL for the API


        public async Task<IActionResult> Index()
        {
            DataTable dt = new DataTable();
            using (var client = new HttpClient())
            { 
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync("Regions/GetAllRegions");

                if (Res.IsSuccessStatusCode)
                {
                    var resultdata = Res.Content.ReadAsStringAsync().Result;
                    dt = JsonConvert.DeserializeObject<DataTable>(resultdata);
                }
                else
                {
                    //ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    Console.WriteLine("Internal server Error");
                    Console.WriteLine("Failed - Calling API");
                }
            }
            return View(dt);
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}