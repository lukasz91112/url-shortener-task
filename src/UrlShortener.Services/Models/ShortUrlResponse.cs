namespace UrlShortener.Services.Models
{
    public class ShortUrlResponse
    {
        public string OriginalUrl { get; set; } = null!;
        public string ShortUrl { get; set; } = null!;
    }
}
