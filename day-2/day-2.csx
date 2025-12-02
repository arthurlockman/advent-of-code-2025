#!/usr/bin/env dotnet-script
#load "../utils.csx"

public IEnumerable<long> CreateRange(long start, long count)
{
    var limit = start + count;

    while (start < limit)
    {
        yield return start;
        start++;
    }
}

private long SumBadProductIdsPart1(IEnumerable<string> ranges)
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

private long SumBadProductIdsPart2(IEnumerable<string> ranges)
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

// Part 1: Sample Input
WriteLine("====== PART 1 ======");
var sampleRanges = Utils.ReadInput("sample.txt").Split(',');
WriteLine($"Sample sum: {SumBadProductIdsPart1(sampleRanges)}");
var realRanges = Utils.ReadInput("input.txt").Split(',');
WriteLine($"Real sum: {SumBadProductIdsPart1(realRanges)}");

WriteLine("\n====== PART 2 ======");
WriteLine($"Sample sum: {SumBadProductIdsPart2(sampleRanges)}");
WriteLine($"Real sum: {SumBadProductIdsPart2(realRanges)}");