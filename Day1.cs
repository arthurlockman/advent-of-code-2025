using AdventOfCode2025.Shared;
using AoCHelper;

namespace AdventOfCode2025;

public class Dial(int startingPosition = 50)
{
    private int Position { get; set; } = startingPosition;
    public int ZeroCounter { get; set; } = 0;
    public int ZeroClickCounter { get; set; } = 0;

    public void Rotate(IEnumerable<string> cmds)
    {
        cmds.ToList().ForEach(l => Rotate(l));
    }

    public void Rotate(string cmd)
    {
        var direction = cmd.StartsWith("L") ? -1 : 1;
        var clicks = int.Parse(cmd[1..]);
        while (clicks > 0)
        {
            Position += direction;
            if (Position > 99)
            {
                Position = 0;
            } else if (Position < 0) {
                Position = 99;
            }
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

public class Day1 : BaseDay
{
    private readonly IEnumerable<string> Input;
    public Day1()
    {
        AsyncHelper.RunSync(async () => await Utils.GetInputData(2025, 1, InputFilePath));
        Input = Utils.ReadInputLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var dial = new Dial();
        dial.Rotate(Input);
        return new(dial.ZeroCounter.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var dial = new Dial();
        dial.Rotate(Input);
        return new(dial.ZeroClickCounter.ToString());
    }
}