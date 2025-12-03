using AdventOfCode2025.Shared;
using JetBrains.Annotations;

namespace AdventOfCode2025;

[UsedImplicitly]
public class Day2() : Day(2)
{
    public override ValueTask<string> Solve_1()
    {
        return new ValueTask<string>(SumBadProductIdsPart1(Input.Split(',')).ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new ValueTask<string>(SumBadProductIdsPart2(Input.Split(',')).ToString());
    }

    private static IEnumerable<long> CreateRange(long start, long count)
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
                    if (idStr.Length % 2 != 0) return 0;
                    var firstHalf = idStr[..(idStr.Length / 2)];
                    var secondHalf = idStr[(idStr.Length / 2)..];
                    return firstHalf == secondHalf ? id : 0;
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
                        var chunk = idStr[..i];
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