using Microsoft.Extensions.Configuration;

namespace AdventOfCode2025.Shared;

public static class Utils
{
    public static IEnumerable<string> ReadInputLines(string filename)
    {
        return File.ReadLines(filename);
    }

    public static string ReadInput(string filename)
    {
        using StreamReader reader = new(filename);
        return reader.ReadToEnd();
    }

    public static async Task GetInputData(int year, int day, string filename)
    {
        if (!File.Exists(filename))
        {
            await DownloadAdventOfCodeInputFileAsync(year, day, filename);
        }
    }

    /// <remarks>
    /// Adapted from https://www.nuget.org/packages/AdventOfCodeSupport
    /// </remarks>
    private static async Task DownloadAdventOfCodeInputFileAsync(int year, int day, string filePath)
    {
        IConfigurationRoot config = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

        var cookie = config["session"];
        if (string.IsNullOrWhiteSpace(cookie))
        {
            throw new InvalidOperationException(
                "Unable to get input from AdventOfCode.com. No session cookie found. See ReadMe for details.");
        }

        var handler = new HttpClientHandler { UseCookies = false };
        var adventHttpClient = new HttpClient(handler) { BaseAddress = new Uri("https://adventofcode.com/") };
        adventHttpClient.DefaultRequestHeaders.Add("cookie", $"session={cookie.Trim()}");

        var result = await adventHttpClient.GetAsync($"{year}/day/{day}/input");
        if (!result.IsSuccessStatusCode)
            throw new Exception($"Input download {year} Day {day} failed. {result.ReasonPhrase}");

        var text = await result.Content.ReadAsStringAsync();
        await File.WriteAllTextAsync(filePath, text);
    }
}