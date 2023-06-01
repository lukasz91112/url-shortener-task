using Microsoft.AspNetCore.Mvc;

namespace UrlShortener.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}