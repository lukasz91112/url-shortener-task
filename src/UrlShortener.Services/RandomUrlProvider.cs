using UrlShortener.Services.Interfaces;

namespace UrlShortener.Services
{
    public class RandomUrlProvider : IRandomUrlProvider
    {
        private readonly IRandomValueProvider _randomValueProvider;

        public RandomUrlProvider(IRandomValueProvider randomValueProvider)
        {
            _randomValueProvider = randomValueProvider;
        }

        public string GetRandomUrl()
        {
            var characters = Enumerable.Range(48, 48);

            var allowedUrlCharacters = characters.Where(x => x < 58 || x > 64 && x < 91 || x > 96);

            var randomCharacters = allowedUrlCharacters
                .OrderBy(x => _randomValueProvider.Next())
                .Select(x => Convert.ToChar(x))
                .Take(6)
                .ToArray();


            return new string(randomCharacters);
        }
    }
}
