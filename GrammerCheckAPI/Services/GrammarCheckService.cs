using System.Text.Json;

public class GrammarCheckService
{
    private readonly HttpClient _httpClient;

    public GrammarCheckService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<GrammarResult> CheckGrammarAsync(string text)
    {
        var form = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "text", text },
            { "language", "en-US" }
        });

        var response = await _httpClient.PostAsync(
            "https://api.languagetool.org/v2/check", form);

        var json = await response.Content.ReadAsStringAsync();
        var doc = JsonDocument.Parse(json);

        var matches = doc.RootElement.GetProperty("matches");

        string correctedText = text;
        int errorCount = matches.GetArrayLength();

        foreach (var match in matches.EnumerateArray())
        {
            var replacements = match.GetProperty("replacements");
            if (replacements.GetArrayLength() > 0)
            {
                var replacement = replacements[0].GetProperty("value").GetString();
                var context = match.GetProperty("context").GetProperty("text").GetString();

                correctedText = correctedText.Replace(context!, replacement!);
            }
        }

        return new GrammarResult
        {
            OriginalText = text,
            CorrectedText = correctedText,
            ErrorsFound = errorCount
        };
    }
}

public class GrammarResult
{
    public string OriginalText { get; set; } = string.Empty;
    public string CorrectedText { get; set; } = string.Empty;
    public int ErrorsFound { get; set; }
}

