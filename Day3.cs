using AdventOfCode2025.Shared;
using JetBrains.Annotations;

namespace AdventOfCode2025;

[UsedImplicitly]
public class Day3 : Day
{
    public Day3()
    {
        Test = new TestCase("357", "3121910778619");
    }

    public override ValueTask<string> Solve_1()
    {
        var result = InputLines.Value.AsParallel().Select(line =>
        {
            var batteries = line.ToCharArray().Select(c => int.Parse(c.ToString())).ToArray();
            var highestIdx = FindHighestIndex(batteries, 0, 1);
            var nextHighestIdx = FindHighestIndex(batteries, highestIdx + 1, 0);
            return long.Parse($"{batteries[highestIdx]}{batteries[nextHighestIdx]}");
        }).Sum().ToString();
        return new ValueTask<string>(result);
    }

    public override ValueTask<string> Solve_2()
    {
        var result = InputLines.Value.AsParallel().Select(line =>
        {
            var batteries = line.ToCharArray().Select(c => int.Parse(c.ToString())).ToArray();
            var result = "";
            var highestIdx = -1;
            var cutoff = 11;
            do // sue me, I don't care
            {
                highestIdx = FindHighestIndex(batteries, highestIdx + 1, cutoff);
                result += batteries[highestIdx].ToString();
                cutoff--;
            } while (cutoff >= 0);

            return long.Parse(result);
        }).Sum().ToString();
        return new ValueTask<string>(result);
    }

    private static int FindHighestIndex(int[] batteries, int startIndex, int cutoff)
    {
        var highestIdx = startIndex;
        for (var i = highestIdx; i < batteries.Length - cutoff; i++)
        {
            if (batteries[i] > batteries[highestIdx]) highestIdx = i;
        }

        return highestIdx;
    }
}