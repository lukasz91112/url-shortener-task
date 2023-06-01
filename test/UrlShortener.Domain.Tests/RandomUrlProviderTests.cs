using FluentAssertions;
using Moq;
using NUnit.Framework;
using UrlShortener.Services;
using UrlShortener.Services.Interfaces;

namespace UrlShortener.Domain.Tests
{
    [TestFixture]
    public class RandomUrlProviderTests
    {
        private RandomUrlProvider _service;       

        [SetUp]
        public void Setup() 
        {
            var numberQueue = new Queue<int>(Enumerable.Range(0, 100));
            var randomValueProvider = new Mock<IRandomValueProvider>();

            randomValueProvider.Setup(x => x.Next()).Returns(numberQueue.Dequeue);

            _service = new RandomUrlProvider(randomValueProvider.Object);
        }

        [Test]
        public void CreateRandomUrlCorrectly()
        {
            // Arrange

            // Randomizer is mocked to return characters in alphabetical order
            var expectedResult = "012345";

            // Act
            var actualResult = _service.GetRandomUrl();

            // Assert
            actualResult.Should().BeEquivalentTo(expectedResult);
        }
    }
}
