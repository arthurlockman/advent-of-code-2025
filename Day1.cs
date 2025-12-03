using AdventOfCode2025.Shared;
using JetBrains.Annotations;

namespace AdventOfCode2025;

public class Dial(int startingPosition = 50)
{
    private int Position { get; set; } = startingPosition;
    public int ZeroCounter { get; private set; }
    public int ZeroClickCounter { get; private set; }

    public void Rotate(IEnumerable<string> cmds)
    {
        cmds.ToList().ForEach(l => Rotate(l));
    }

    private void Rotate(string cmd)
    {
        var direction = cmd.StartsWith('L') ? -1 : 1;
        var clicks = int.Parse(cmd[1..]);
        while (clicks > 0)
        {
            Position += direction;
            Position = Position switch
            {
                > 99 => 0,
                < 0 => 99,
                _ => Position
            };
            clicks--;
            if (Position == 0) {
                ZeroClickCounter++;
            }
        }
        if (Position == 0) {
            ZeroCounter++;
        }
    }
}

[UsedImplicitly]
public class Day1() : Day(1)
{
    public override ValueTask<string> Solve_1()
    {
        var dial = new Dial();
        dial.Rotate(GetInputLines());
        return new ValueTask<string>(dial.ZeroCounter.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var dial = new Dial();
        dial.Rotate(GetInputLines());
        return new ValueTask<string>(dial.ZeroClickCounter.ToString());
    }
}