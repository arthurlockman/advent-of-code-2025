using AdventOfCode2025.Shared;
using JetBrains.Annotations;

namespace AdventOfCode2025;

[UsedImplicitly]
public class Day11 : Day
{
    public Day11()
    {
        Test = new TestCase("5", "2");
    }

    public override ValueTask<string> Solve_1()
    {
        var graph = GetGraph();
        var paths = graph.CountPaths("you", "out");
        return new ValueTask<string>(paths.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var graph = GetGraph();
        var paths = graph.CountPaths("svr", "fft") * graph.CountPaths("fft", "dac") * graph.CountPaths("dac", "out")
            + graph.CountPaths("svr", "dac") * graph.CountPaths("dac", "fft") * graph.CountPaths("fft", "out");
        return new ValueTask<string>(paths.ToString());
    }

    private StringGraph GetGraph()
    {
        var graph = new StringGraph();
        foreach (var line in InputLines.Value)
        {
            var nodeString = line.Split(":")[0];
            var edges = line.Split(" ")[1..];
            foreach (var edge in edges)
            {
                graph.AddConnection(nodeString, edge);
            }
        }
        return graph;
    }
}