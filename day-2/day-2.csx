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

private long SumBadProductIds(IEnumerable<string> ranges)
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

// Part 1: Sample Input
WriteLine("====== PART 1 ======");
var sampleRanges = Utils.ReadInput("sample.txt").Split(',');
WriteLine($"Sample sum: {SumBadProductIds(sampleRanges)}");
var realRanges = Utils.ReadInput("input.txt").Split(',');
WriteLine($"Sample sum: {SumBadProductIds(realRanges)}");
