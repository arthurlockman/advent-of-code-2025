using System.Runtime.CompilerServices;

public static class Utils
{
    public static IEnumerable<string> ReadInputLines(
        string filename,
        [CallerFilePath] string scriptFile = "")
    {
        var scriptDir = Path.GetDirectoryName(scriptFile) ?? Directory.GetCurrentDirectory();
        var filePath = Path.Combine(scriptDir, filename);
        return File.ReadLines(filePath);
    }

    public static string ReadInput(
        string filename,
        [CallerFilePath] string scriptFile = "")
    {
        var scriptDir = Path.GetDirectoryName(scriptFile) ?? Directory.GetCurrentDirectory();
        var filePath = Path.Combine(scriptDir, filename);
        using StreamReader reader = new(filePath);
        return reader.ReadToEnd();
    }
}