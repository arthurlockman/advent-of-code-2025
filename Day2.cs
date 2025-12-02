using AdventOfCode2025.Shared;
using AoCHelper;

namespace AdventOfCode2025;

public class Day2 : BaseDay
{
    private readonly IEnumerable<string> Input;
    public Day2()
    {
        AsyncHelper.RunSync(async () => await Utils.GetInputData(2025, 2, InputFilePath));
        Input = Utils.ReadInput(InputFilePath).Split(',');
    }

    public override ValueTask<string> Solve_1()
    {
        return new(SumBadProductIdsPart1(Input).ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new(SumBadProductIdsPart2(Input).ToString());
    }

    public static IEnumerable<long> CreateRange(long start, long count)
    {
        var limit = start + count;

        while (start < limit)
        {
            yield return start;
            start++;
        }
    }

    private static long SumBadProductIdsPart1(IEnumerable<string> ranges)
    {
        return ranges.AsParallel().Select(range =>
        {
            var tmp = range.Split('-');
            var lowerBound = long.Parse(tmp[0]);
            var upperBound = long.Parse(tmp[1]);
            return CreateRange(lowerBound, upperBound - lowerBound + 1)
                .AsParallel()
                .Select(id =>
                {
                    var idStr = id.ToString();
                    if (idStr.Length % 2 == 0)
                    {
                        var firstHalf = idStr.Substring(0, idStr.Length / 2);
                        var secondHalf = idStr.Substring(idStr.Length / 2);
                        if (firstHalf == secondHalf)
                        {
                            return id;
                        }
                    }
                    return 0;
                }).Sum();
        }).Sum();
    }

    private static long SumBadProductIdsPart2(IEnumerable<string> ranges)
    {
        return ranges.AsParallel().Select(range =>
        {
            var tmp = range.Split('-');
            var lowerBound = long.Parse(tmp[0]);
            var upperBound = long.Parse(tmp[1]);
            return CreateRange(lowerBound, upperBound - lowerBound + 1)
                .AsParallel()
                .Select(id =>
                {
                    var idStr = id.ToString();
                    for (var i = 1; i < idStr.Length; i++)
                    {
                        var chunk = idStr.Substring(0, i);
                        if (idStr.Replace(chunk, "").Length == 0)
                        {
                            return id;
                        }
                    }
                    return 0;
                }).Sum();
        }).Sum();
    }
}