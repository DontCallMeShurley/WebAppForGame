using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection;
using WebAppForGame.Models;

namespace WebAppForGame.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult HowToGet()
        {

            return View();
        }
        public IActionResult Contacts()
        {

            return View();
        }

        public IActionResult Privacy()
        {
            return Redirect("https://ssc.soulsofworld.ru/uploads/oferta_330710629601.docx");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}