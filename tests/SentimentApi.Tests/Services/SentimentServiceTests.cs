using Xunit;
using SentimentApi.Services;

namespace SentimentApi.Tests.Services
{
    public class SentimentServiceTests
    {
        private readonly SentimentService _service = new();

        [Theory]
        [InlineData("Este producto es excelente", "positivo")]
        [InlineData("La experiencia fue horrible", "negativo")]
        [InlineData("No tengo opinión al respecto", "neutral")]
        public void Classify_ReturnsExpectedSentiment(string text, string expected)
        {
            // Act
            var result = _service.Classify(text);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}