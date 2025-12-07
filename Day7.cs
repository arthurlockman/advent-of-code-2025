using AdventOfCode2025.Shared;
using JetBrains.Annotations;

namespace AdventOfCode2025;

[UsedImplicitly]
public class Day7 : Day
{
    private record Beam(int Index, long Paths)
    {
        public Beam Combine(Beam other) => this with { Paths = Paths + other.Paths };
        public IEnumerable<Beam> Split() => [this with { Index = Index + 1 }, this with { Index = Index - 1 }];
        public virtual bool Equals(Beam? other) => other?.Index == Index;
        public override int GetHashCode() => Index;
    }

    public Day7()
    {
        Test = new TestCase("21", "40");
    }

    public override ValueTask<string> Solve_1()
    {
        var (grid, startPosition) = GetGrid();
        List<int> beams = [startPosition];
        var splitCounter = 0;
        for (var layer = 0; layer < grid.Length; layer++)
        {
            var nextLayerIndex = layer + 1;
            if (nextLayerIndex >= grid.Length) continue;
            var nextLayer = grid[nextLayerIndex];
            List<int> newBeams = [];
            foreach (var beam in beams)
            {
                switch (nextLayer[beam])
                {
                    case '.':
                        newBeams.Add(beam);
                        break;
                    case '^':
                        newBeams.Add(beam - 1);
                        newBeams.Add(beam + 1);
                        splitCounter++;
                        break;
                }
            }

            beams = newBeams.Distinct().OrderBy(x => x).ToList();
        }

        return new ValueTask<string>($"{splitCounter}");
    }

    public override ValueTask<string> Solve_2()
    {
        var (grid, startPosition) = GetGrid();
        List<Beam> beams = [new(startPosition, 1)];
        for (var layer = 0; layer < grid.Length; layer++)
        {
            var nextLayerIndex = layer + 1;
            if (nextLayerIndex >= grid.Length) continue;
            var nextLayer = grid[nextLayerIndex];
            List<Beam> newBeams = [];
            foreach (var beam in beams)
            {
                switch (nextLayer[beam.Index])
                {
                    case '.':
                        newBeams.Add(beam);
                        break;
                    case '^':
                        newBeams.AddRange(beam.Split());
                        break;
                }
            }

            beams = newBeams.GroupBy(x => x)
                .Select(x => x.Aggregate((a, b) => a.Combine(b)))
                .ToList();
        }

        return new ValueTask<string>($"{beams.Sum(x => x.Paths)}");
    }

    private (char[][] grid, int startPosition) GetGrid()
    {
        var grid = InputLines.Value.Select(l => l.ToCharArray()).ToArray();
        return (grid, grid[0].IndexOf('S'));
    }
}