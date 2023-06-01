using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using UrlShortener.Data;
using UrlShortener.Domain.Entities;
using UrlShortener.Services;
using UrlShortener.Services.Interfaces;
using UrlShortener.Services.Models;

namespace UrlShortener.Domain.Tests
{
    [TestFixture]
    public class UrlShortenerServiceTests
    {
        private UrlShortenerContext _context;
        private UrlShortenerService _service;
        private Mock<IRandomUrlProvider> _randomUrlProviderMock;

        [SetUp]
        public void Setup()
        {
            var builder = new DbContextOptionsBuilder<UrlShortenerContext>();
            builder.UseInMemoryDatabase("UnitTests");

            _context = new UrlShortenerContext(builder.Options);

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _randomUrlProviderMock = new Mock<IRandomUrlProvider>();            

            var loggerMock = new Mock<ILogger<UrlShortenerService>>(MockBehavior.Loose);

            _service = new UrlShortenerService(_context, _randomUrlProviderMock.Object, loggerMock.Object);
        }

        [Test]
        public async Task CreateShortUrl_CreatesShortUrlCorrectly()
        {
            // Arrange
            var existingUrl = new Url
            {
                Id = "012345",
                OriginalUrl = "https://alreadyexistingurl.com"
            };

            _context.Add(existingUrl);

            _randomUrlProviderMock.SetupSequence(x => x.GetRandomUrl())
                .Returns("012345")
                .Returns("abcdef");

            var request = new CreateShortUrlRequest
            {
                OriginalUrl = "https://somenewurl.com"
            };

            var expectedUrls = new List<Url>
            {
                existingUrl,
                new Url
                {
                    Id = "abcdef",
                    OriginalUrl = "https://somenewurl.com"
                }
            };

            // Act
            await _service.CreateShortUrl(request);

            // Assert
            var actualUrls = _context.Urls.ToList();

            actualUrls.Should().BeEquivalentTo(expectedUrls);
            actualUrls.Count.Should().Be(expectedUrls.Count);
        }

        [Test]
        public async Task CreateShortUrl_ThrowsException_WhenRetriesReachedLimit()
        {
            // Arrange
            var existingUrl = new Url
            {
                Id = "012345",
                OriginalUrl = "https://alreadyexistingurl.com"
            };

            _context.Add(existingUrl);

            _randomUrlProviderMock.Setup(x => x.GetRandomUrl())
                .Returns("012345");

            var request = new CreateShortUrlRequest
            {
                OriginalUrl = "https://somenewurl.com"
            };

            // Act
            Func<Task> act = async () => await _service.CreateShortUrl(request);

            // Assert
            await act.Should().ThrowAsync<Exception>().Where(e => e.Message.Equals("Can't generate unique URL!"));
        }

        [Test]
        public async Task GetOriginalUrl_ReturnsOriginalUrlCorrectly()
        {
            // Arrange
            var existingUrls = new Url[]
            {
                new Url
                {
                    Id = "012345",
                    OriginalUrl = "https://alreadyexistingurl.com"
                },
                new Url
                {
                    Id = "abcdef",
                    OriginalUrl = "https://alreadyexistingurl2.com"
                }
            };            

            _context.AddRange(existingUrls);

            var expectedResult = "https://alreadyexistingurl2.com";

            // Act
            var actualResult = await _service.GetOriginalUrl("abcdef");

            // Assert
            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [Test]        
        public async Task GetOriginalUrls_ThrowsException_WhenUrlDoesntExists()
        {
            // Arrange
            var existingUrls = new Url[]
            {
                new Url
                {
                    Id = "012345",
                    OriginalUrl = "https://alreadyexistingurl.com"
                },
                new Url
                {
                    Id = "abcdef",
                    OriginalUrl = "https://alreadyexistingurl2.com"
                }
            };

            _context.AddRange(existingUrls);

            // Act
            Func<Task> act = async () => await _service.GetOriginalUrl("notexistingshorturl");

            // Assert
            await act.Should().ThrowAsync<ArgumentException>().Where(e => e.Message.Equals("Given short URL doesn't exist!"));
        }

        [Test]
        public async Task GetShortUrls_ReturnsUrlsCorrectly()
        {
            // Arrange
            var existingUrls = new Url[]
            {
                new Url
                {
                    Id = "012345",
                    OriginalUrl = "https://alreadyexistingurl.com"
                },
                new Url
                {
                    Id = "abcdef",
                    OriginalUrl = "https://alreadyexistingurl2.com"
                }
            };

            _context.AddRange(existingUrls);

            await _context.SaveChangesAsync();

            var expectedResult = new List<ShortUrlResponse>
            {
                new ShortUrlResponse
                {
                    OriginalUrl = "https://alreadyexistingurl.com",
                    ShortUrl = "012345"
                },
                new ShortUrlResponse
                {
                    OriginalUrl = "https://alreadyexistingurl2.com",
                    ShortUrl = "abcdef"
                }
            };

            // Act
            var actualResult = await _service.GetShortUrls();

            // Assert
            actualResult.Should().BeEquivalentTo(expectedResult);
        }
    }
}