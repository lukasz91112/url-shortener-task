using UrlShortener.Services.Models;

namespace UrlShortener.Services.Interfaces
{
    public interface IUrlShortenerService
    {
        Task CreateShortUrl(CreateShortUrlRequest request);
        Task<string> GetOriginalUrl(string shortUrl);
        Task<List<ShortUrlResponse>> GetShortUrls();
    }
}