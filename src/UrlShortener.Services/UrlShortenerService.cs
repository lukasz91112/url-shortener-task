using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UrlShortener.Data;
using UrlShortener.Domain.Entities;
using UrlShortener.Services.Interfaces;
using UrlShortener.Services.Models;

namespace UrlShortener.Services
{
    public class UrlShortenerService : IUrlShortenerService
    {
        private readonly UrlShortenerContext _context;
        private readonly IRandomUrlProvider _randomUrlProvider;
        private readonly ILogger<UrlShortenerService> _logger;
        private const int _maxUrlCreationRetriesNumber = 5;

        public UrlShortenerService(
            UrlShortenerContext context,
            IRandomUrlProvider randomUrlProvider,
            ILogger<UrlShortenerService> logger)
        {
            _context = context;
            _randomUrlProvider = randomUrlProvider;
            _logger = logger;
        }

        public async Task CreateShortUrl(CreateShortUrlRequest request)
        {
            _logger.LogInformation("Creating short url for: {0}", request.OriginalUrl);

            var retriesNumber = 0;

            string newShortUrl = string.Empty;
            bool isDuplicate = true;

            while (isDuplicate)
            {
                _logger.LogInformation("Attempting to generate short url. Try number: {0}", retriesNumber);

                if(retriesNumber > _maxUrlCreationRetriesNumber)
                {
                    _logger.LogError("Exceeded maximum number of retries. Failed to generate unique short url!");

                    throw new Exception("Can't generate unique URL!");
                }

                newShortUrl = _randomUrlProvider.GetRandomUrl();
                isDuplicate = _context.Urls.Find(newShortUrl) != null;

                retriesNumber++;
            }            

            var newUrl = new Url
            {
                Id = newShortUrl,
                OriginalUrl = request.OriginalUrl
            };

            _context.Urls.Add(newUrl);

            await _context.SaveChangesAsync();

            _logger.LogInformation("Short url: {0} created successfully for: {1}", newShortUrl, request.OriginalUrl);
        }

        public async Task<string> GetOriginalUrl(string shortUrl)
        {
            _logger.LogInformation("Trying to get original url for short: {0}", shortUrl);

            var url = await _context.Urls.FindAsync(shortUrl);

            if(url == null)
            {
                _logger.LogError("Url not found!");

                throw new ArgumentException("Given short URL doesn't exist!");
            }

            return url.OriginalUrl;
        }

        public async Task<List<ShortUrlResponse>> GetShortUrls()
        {
            var urls = await _context.Urls.ToListAsync();

            var response = urls.Select(x => new ShortUrlResponse { OriginalUrl = x.OriginalUrl, ShortUrl = x.Id}).ToList();

            return response;
        }
    }
}
