using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FreeRentLibrary.Models;
using System.Diagnostics;

namespace FreeRentLibrary.Controllers
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

        public IActionResult Privacy()
        {
            return View();
        }


    }
}
