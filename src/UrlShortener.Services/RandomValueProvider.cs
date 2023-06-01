using UrlShortener.Services.Interfaces;

namespace UrlShortener.Services
{
    public class RandomValueProvider : IRandomValueProvider
    {
        public int Next()
        {
            return new Random().Next();
        }
    }
}
