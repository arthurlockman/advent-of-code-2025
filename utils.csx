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

    public static (T Result, long ElapsedNs) Timed<T>(Func<T> func)
    {
        var sw = Stopwatch.StartNew();
        var result = func();
        sw.Stop();
        var nanoseconds = sw.ElapsedTicks * 1_000_000_000L / Stopwatch.Frequency;
        return (result, nanoseconds);
    }
}