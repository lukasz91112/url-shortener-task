namespace UrlShortener.Domain.Entities
{
    public class Url
    {
        public string Id { get; set; } = null!;
        public string OriginalUrl { get; set; } = null!;
    }
}
