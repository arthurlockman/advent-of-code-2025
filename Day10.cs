using System.Text.RegularExpressions;
using AdventOfCode2025.Shared;
using Combinatorics.Collections;
using JetBrains.Annotations;

namespace AdventOfCode2025;

[UsedImplicitly]
public class Day10 : Day
{
    public Day10()
    {
        Test = new TestCase("7", "33");
    }

    public override ValueTask<string> Solve_1()
    {
        var accum = 0;
        foreach (var (lightMask, buttons) in GetLightAndButtonMasks())
        {
            var solved = false;
            var numPresses = 0;
            while (!solved)
            {
                var buttonSequences = new Variations<int>(buttons, numPresses, GenerateOption.WithRepetition);
                foreach (var sequence in buttonSequences)
                {
                    var result = sequence.Aggregate(lightMask, (current, press) => current ^ press);
                    if (result != 0) continue;
                    solved = true;
                    accum += sequence.Count;
                    break;
                }
                numPresses++;
            }
        }

        return new ValueTask<string>(accum.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        throw new NotImplementedException();
    }

    private IEnumerable<(int lightMask, List<int> buttons)> GetLightAndButtonMasks()
    {
        var lightRegex = new Regex(@"\[([\.\#]*)\]");
        var buttonRegex = new Regex(@"\((\d+(?:,\d+)*)\)");

        return InputLines.Value.Select(l =>
        {
            var lightString = lightRegex.Match(l).Groups[1].Captures.First().Value;
            var lightMask = Convert.ToInt32(new string(lightString.Reverse().ToArray()).Replace('.', '0').Replace('#', '1'), 2);

            var buttons = buttonRegex.Matches(l)
                .Select(match => match.Groups[1].Value.Split(',').Select(btn => 1 << int.Parse(btn))
                    .Aggregate(((arg1, arg2) => arg1 | arg2)))
                .ToList();

            return (lightMask, buttons);
        });
    }
}