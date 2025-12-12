using System.Text.RegularExpressions;
using AdventOfCode2025.Shared;

namespace AdventOfCode2025;

public class Day12 : Day
{
    public Day12()
    {
        Test = new TestCase("2", null);
    }

    public override ValueTask<string> Solve_1()
    {
        var parseRegex = new Regex(@"\n(?<length>\d+)x(?<width>\d+): (?<reqs>(?:\d| )+)");
        var tmp = parseRegex.Matches(Input).Select(match => (int.Parse(match.Groups["length"].Value),
            int.Parse(match.Groups["width"].Value),
            match.Groups["reqs"].Value.Split(" ").Select(int.Parse)));
        var result = tmp.Count(req =>
        {
            var l = req.Item1;
            var w = req.Item2;
            return w * l >= req.Item3.Sum() * 9;
        });
        return new ValueTask<string>(result.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new ValueTask<string>("Merry Christmas!");
    }
}