using System.Text.RegularExpressions;
using AdventOfCode2025.Shared;
using JetBrains.Annotations;

namespace AdventOfCode2025;

[UsedImplicitly]
public partial class Day6 : Day
{
    public Day6()
    {
        Test = new TestCase("4277556", "3263827");

    }

    public override ValueTask<string> Solve_1()
    {
        var numbers = InputLines.Value[..^1].AsEnumerable().Select(l =>
            l.Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray()).ToArray();
        var operands = InputLines.Value.Last().Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries);
        long problemSum = 0;
        for (var p = 0; p < numbers[0].Length; p++)
        {
            var problemComponents = numbers.Select(n => n[p]).ToArray();
            switch (operands[p])
            {
                case "+":
                    problemSum += problemComponents.Sum();
                    break;
                case "*":
                    problemSum += problemComponents.Aggregate(1L, (a, b) => a * b);
                    break;
            }
        }
        return new ValueTask<string>(problemSum.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var numbers = InputLines.Value[..^1].Select(n => n.ToCharArray()).ToArray();
        var operands = RawInput.Split("\n", StringSplitOptions.RemoveEmptyEntries).Last();
        long problemSum = 0;
        while (operands.Length > 0)
        {
            var match = ProblemRegex().Match(operands);
            var problemSize = match.Length;
            var operand = match.Value.Trim();
            List<string> problemComponents = [];
            for (var i = problemSize - 1; i >= 0; i--)
            {
                var tmp = new string(numbers.Select(n => n[i]).ToArray());
                if (string.IsNullOrWhiteSpace(tmp)) continue;
                problemComponents.Add(tmp);
            }
            switch (operand)
            {
                case "+":
                    problemSum += problemComponents.Select(long.Parse).Sum();
                    break;
                case "*":
                    problemSum += problemComponents.Select(long.Parse).Aggregate(1L, (a, b) => a * b);
                    break;
            }
            numbers = numbers.Select(n => n[problemSize..].ToArray()).ToArray();
            operands = operands[problemSize..];
        }
        return new ValueTask<string>(problemSum.ToString());
    }

    [GeneratedRegex(@"^(\*|\+)\s+")]
    private static partial Regex ProblemRegex();
}