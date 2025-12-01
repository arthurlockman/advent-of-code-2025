#!/usr/bin/env dotnet-script
#load "../utils.csx"

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

// Part 1: Sample Input
WriteLine("====== PART 1 ======");
var sampleDial = new Dial();
sampleDial.Rotate(Utils.ReadInputLines("sample.txt"));
WriteLine($"Sample safe code is {sampleDial.ZeroCounter}");

// Part 1: Actual Input
var dial = new Dial();
dial.Rotate(Utils.ReadInputLines("input.txt"));
WriteLine($"Real safe code is {dial.ZeroCounter}");

// Part 2: Already Calculated
WriteLine("\n====== PART 2 ======");
WriteLine($"Sample safe code is {sampleDial.ZeroClickCounter}");
WriteLine($"Real safe code is {dial.ZeroClickCounter}");
