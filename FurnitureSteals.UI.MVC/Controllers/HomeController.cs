using FurnitureSteals.UI.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FurnitureSteals.UI.MVC.Controllers
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

        public IActionResult About()
        {
            return View();

        }

        public IActionResult Policies()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}
