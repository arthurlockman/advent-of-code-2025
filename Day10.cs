using System.Text.RegularExpressions;
using AdventOfCode2025.Shared;
using Combinatorics.Collections;
using JetBrains.Annotations;
using Microsoft.Z3;

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
        foreach (var (lightMask, buttons, _) in GetLightAndButtonMasks())
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
        // Initialize Z3 SMT solver context for constraint solving
        var z3 = new Context();
        var zero = z3.MkInt(0);
        var overallResult = 0;

        // Process each machine configuration
        foreach (var (_, buttons, joltages) in GetLightAndButtonMasks())
        {
            // Create an optimizer to find the minimum number of button presses
            var optimizer = z3.MkOptimize();
            var buttonPresses = new ArithExpr[buttons.Count];

            // Create integer variables for each button representing the number of times it's pressed
            // Add constraint that each button can only be pressed non-negative times
            for (var idx = 0; idx < buttons.Count; idx++)
            {
                var button = z3.MkIntConst($"b{idx}");
                buttonPresses[idx] = button;
                // Constraint: button presses >= 0
                var positiveConstraint = z3.MkGe(button, zero);
                optimizer.Add(positiveConstraint);
            }

            // For each joltage position (light), create a constraint that the sum of button presses
            // affecting that position must equal the target joltage value
            for (var idx = 0; idx < joltages.Count; idx++)
            {
                List<ArithExpr> jolts = [];
                // Check which buttons affect this joltage position (bit idx)
                for (var btnIdx = 0; btnIdx < buttons.Count; btnIdx++)
                {
                    // If button affects this light position (bit is set), add it to the sum
                    if ((buttons[btnIdx] & (1 << idx)) != 0)
                    {
                        jolts.Add(buttonPresses[btnIdx]);
                    }
                }
                // Constraint: sum of relevant button presses = target joltage
                var targetJolt = z3.MkInt(joltages[idx]);
                var joltLimit = z3.MkEq(z3.MkAdd(jolts), targetJolt);
                optimizer.Add(joltLimit);
            }

            // Set the objective: minimize the total number of button presses
            var sumToMinimize = z3.MkAdd(buttonPresses);
            var result = optimizer.MkMinimize(sumToMinimize);

            // Check if a solution exists and extract the minimized value
            if (optimizer.Check() != Status.SATISFIABLE) continue;
            if (result.Value is not IntNum answer) continue;

            // Add the minimum button presses for this machine to the overall result
            var value = answer.Int;
            overallResult += value;
        }
        return new ValueTask<string>(overallResult.ToString());
    }

    private IEnumerable<(int lightMask, List<int> buttons, List<int> joltages)> GetLightAndButtonMasks()
    {
        var lightRegex = new Regex(@"\[([\.\#]*)\]");
        var buttonRegex = new Regex(@"\((\d+(?:,\d+)*)\)");
        var joltageRegex = new Regex(@"{(?<joltages>(?:\d+,?)+)}");

        return InputLines.Value.Select(l =>
        {
            var lightString = lightRegex.Match(l).Groups[1].Captures.First().Value;
            var lightMask = Convert.ToInt32(new string(lightString.Reverse().ToArray()).Replace('.', '0').Replace('#', '1'), 2);

            var buttons = buttonRegex.Matches(l)
                .Select(match => match.Groups[1].Value.Split(',').Select(btn => 1 << int.Parse(btn))
                    .Aggregate(((arg1, arg2) => arg1 | arg2)))
                .ToList();

            var joltages = joltageRegex.Match(l).Groups["joltages"].Value.Split(',').Select(int.Parse).ToList();

            return (lightMask, buttons, joltages);
        });
    }
}