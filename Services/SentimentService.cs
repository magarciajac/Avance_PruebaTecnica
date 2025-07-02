using System.Linq;

namespace SentimentApi.Services
{
    public class SentimentService
    {
        private static readonly string[] Positive =
            { "excelente", "genial", "fantástico", "bueno", "increíble" };

        private static readonly string[] Negative =
            { "malo", "terrible", "problema", "defecto", "horrible" };

        public string Classify(string text)
        {
            var lower = text.ToLower();
            if (Positive.Any(w => lower.Contains(w))) return "positivo";
            if (Negative.Any(w => lower.Contains(w))) return "negativo";
            return "neutral";
        }
    }
}