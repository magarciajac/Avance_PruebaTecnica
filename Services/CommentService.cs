namespace SentimentApi.Services
{
    /// <summary>
    /// Very small‑rule sentiment classifier.
    /// Returns **Positive**, **Negative** or **Neutral** strings (Pascal‑case)
    /// so that unit‑tests expect exactly those values.
    /// </summary>
    public class CommentService
    {
        public string ClassifySentiment(string commentText)
        {
            if (string.IsNullOrWhiteSpace(commentText))
                return "Neutral";

            var t = commentText.ToLowerInvariant();

            // Positive cues (English & Spanish)
            if (t.Contains("excelente") || t.Contains("muy bueno") || t.Contains("me encanta")
                || t.Contains("great") || t.Contains("awesome") || t.Contains("love"))
                return "Positive";

            // Negative cues (English & Spanish)
            if (t.Contains("malo") || t.Contains("odio") || t.Contains("horrible")
                || t.Contains("bad") || t.Contains("terrible") || t.Contains("hate"))
                return "Negative";

            return "Neutral";
        }
    }
}