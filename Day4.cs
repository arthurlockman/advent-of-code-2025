using AdventOfCode2025.Shared;
using JetBrains.Annotations;

namespace AdventOfCode2025;

[UsedImplicitly]
public class Day4 : Day
{
    public Day4()
    {
        Test = new TestCase("13", "43");
    }

    public override ValueTask<string> Solve_1()
    {
        var grid = InputLines.Value.Select(l => l.ToCharArray()).ToArray();
        return new ValueTask<string>(FindAndRemoveRolls(grid).removedRolls.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var grid = InputLines.Value.Select(l => l.ToCharArray()).ToArray();
        var (removedRolls, newGrid) = FindAndRemoveRolls(grid);
        var totalRemoved = 0;
        while (removedRolls > 0)
        {
            totalRemoved += removedRolls;
            (removedRolls, newGrid) = FindAndRemoveRolls(newGrid);
        }
        return new ValueTask<string>(totalRemoved.ToString());
    }

    private static (int removedRolls, char[][] newGrid) FindAndRemoveRolls(char[][] grid)
    {
        var gridWidth = grid[0].Length;
        var gridHeight = grid.Length;
        var newGrid = grid.Select(a => a.ToArray()).ToArray();
        var removedRolls = 0;
        for (var row = 0; row < gridHeight; row++)
        {
            for (var col = 0; col < gridWidth; col++)
            {
                var occupied = 0;
                if (grid[row][col] != '@') continue;
                for (var r = -1; r <= 1; r++)
                {
                    for (var c = -1; c <= 1; c++)
                    {
                        var r1 = row + r;
                        var c1 = col + c;
                        if (r1 == row && c1 == col) continue;
                        if (0 > r1 || r1 >= gridWidth || 0 > c1 || c1 >= gridHeight
                            || grid[r1][c1] != '@') continue;
                        occupied++;
                    }
                }

                if (occupied >= 4) continue;
                removedRolls++;
                newGrid[row][col] = '.';
            }
        }
        return (removedRolls, newGrid);
    }
}