
using System.Net;
using RestSharp;
using System.Text.Json;

namespace LmsGrammarAI.Services
{
    public class GeminiGrammarService
    {
        private readonly IConfiguration _configuration;

        public GeminiGrammarService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> CorrectGrammar(string inputText)
        {
            if (string.IsNullOrWhiteSpace(inputText))
                return inputText;

            var apiKey = _configuration["Gemini:ApiKey"];
            var endpoint = _configuration["Gemini:Endpoint"];

            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(endpoint))
                throw new Exception("Gemini API configuration is missing.");

            // ✅ Gemini requires API key in query string
            var client = new RestClient($"{endpoint}?key={apiKey}");
            var request = new RestRequest("", Method.Post);

            request.AddHeader("Content-Type", "application/json");

            // ✅ Strong, explicit prompt (prevents 1-word responses)
            var body = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new
                            {
                                text =
                                "You are a grammar correction assistant.\n" +
                                "Rewrite the FULL sentence.\n" +
                                "Correct grammar, spelling, capitalization, and punctuation.\n" +
                                "Return ONLY the corrected sentence.\n\n" +
                                $"Sentence: {inputText}"
                            }
                        }
                    }
                }
            };

            request.AddJsonBody(body);

            var response = await client.ExecuteAsync(request);

            // ✅ Better error reporting (VERY IMPORTANT)
            if (!response.IsSuccessful || response.Content == null)
            {
                throw new Exception(
                    $"Gemini API Error\n" +
                    $"StatusCode: {response.StatusCode}\n" +
                    $"ErrorMessage: {response.ErrorMessage}\n" +
                    $"Content: {response.Content}"
                );
            }

            using var json = JsonDocument.Parse(response.Content);

            // ✅ Safe parsing
            if (!json.RootElement.TryGetProperty("candidates", out var candidates) ||
                candidates.GetArrayLength() == 0)
            {
                return inputText;
            }

            var correctedText = candidates[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            // ✅ Safety fallback
            if (string.IsNullOrWhiteSpace(correctedText) ||
                correctedText.Length < inputText.Length / 2)
            {
                return inputText;
            }

            return correctedText.Trim();
        }
    }

    // Optional settings class (fine to keep)
    public class GeminiSettings
    {
        public string ApiKey { get; set; } = string.Empty;
        public string Endpoint { get; set; } = string.Empty;
    }
}
