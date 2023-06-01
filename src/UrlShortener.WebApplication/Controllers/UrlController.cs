using Microsoft.AspNetCore.Mvc;
using UrlShortener.Services.Interfaces;
using UrlShortener.Services.Models;
using UrlShortener.WebApplication.Models;

namespace UrlShortener.WebApplication.Controllers
{
    public class UrlController : Controller
    {
        private readonly IUrlShortenerService _urlShortenerService;

        public UrlController(IUrlShortenerService urlShortenerService)
        {
            _urlShortenerService = urlShortenerService;
        }

        [HttpGet]
        public async Task<IActionResult> RedirectTo(string id)
        {
            var originalUrl = await _urlShortenerService.GetOriginalUrl(id);

            return new RedirectResult(originalUrl);
        }

        [HttpGet]
        public async Task<IActionResult> ViewAll()
        {
            var response = await _urlShortenerService.GetShortUrls();

            var models = response.Select(x => new UrlViewModel { OriginalUrl = x.OriginalUrl, ShortUrl = x.ShortUrl }).ToList();

            return View(models);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddUrlViewModel addUrlRequest)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var request = new CreateShortUrlRequest
            {
                OriginalUrl = addUrlRequest.OriginalUrl,
            };

            await _urlShortenerService.CreateShortUrl(request);

            return RedirectToAction("ViewAll");
        }
    }
}
