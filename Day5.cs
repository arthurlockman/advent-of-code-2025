using AdventOfCode2025.Shared;
using JetBrains.Annotations;
using Range = AdventOfCode2025.Shared.Range;

namespace AdventOfCode2025;

[UsedImplicitly]
public class Day5 : Day
{
    public Day5()
    {
        Test = new TestCase("3", "14");
    }

    public override ValueTask<string> Solve_1()
    {
        var (ranges, ingredients) = ParseDatabase();
        var result = ingredients.Count(i => ranges.Any(r => r.Contains(i)));
        return new ValueTask<string>(result.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var (ranges, _) = ParseDatabase();
        var rangeList = ranges.ToList().OrderBy(r => r.Start).ToList();
        var changed = true;
        while (changed)
        {
            changed = false;
            for (var i = 0; i < rangeList.Count - 1; i++)
            {
                var range = rangeList[i];
                var range2 = rangeList[i + 1];
                if (!range.Overlaps(range2)) continue;
                changed = true;
                rangeList[i] = range.Merge(range2);
                rangeList.RemoveAt(i + 1);
            }
        }

        var result = rangeList.Select(r => r.Length).Sum();
        return new ValueTask<string>(result.ToString());
    }

    private (IEnumerable<Range> Ranges, IEnumerable<long> Ingredients) ParseDatabase()
    {
        var tmp = Input.Split("\n\n");
        return (tmp[0].Split("\n").Select(r =>
        {
            var split = r.Split('-');
            return new Range(long.Parse(split[0]), long.Parse(split[1]));
        }), tmp[1].Split("\n").Select(long.Parse));
    }
}