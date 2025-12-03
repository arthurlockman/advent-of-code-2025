using System.Reflection;
using AdventOfCode2025.Shared;
using AoCHelper;

Action<SolverConfiguration> options = opt =>
{
    opt.ShowOverallResults = true;
    opt.ClearConsole = false;
};

var runTests = args.Contains("--test");
int? dayToRun = null;

for (var i = 0; i < args.Length; i++)
{
    if (args[i] == "--day" && i + 1 < args.Length && int.TryParse(args[i + 1], out var d))
    {
        dayToRun = d;
    }
}

if (runTests)
{
    var allDays = Assembly.GetExecutingAssembly()
        .GetTypes()
        .Where(t => t.IsSubclassOf(typeof(Day)) && !t.IsAbstract)
        .OrderBy(t => t.Name);

    if (dayToRun.HasValue)
    {
        var dayType = dayToRun.Value == 0 ? allDays.LastOrDefault() : allDays.FirstOrDefault(t => t.Name == $"Day{dayToRun}");

        if (dayType != null)
        {
            if (Activator.CreateInstance(dayType) is Day dayInstance)
            {
                await dayInstance.RunTests();
            }
        }
        else
        {
            Console.WriteLine($"Day {dayToRun} not found.");
        }
    }
    else
    {
        foreach (var type in allDays)
        {
            if (Activator.CreateInstance(type) is Day instance)
            {
                await instance.RunTests();
            }
        }
    }
}
else
{
    if (dayToRun.HasValue)
    {
        if (dayToRun.Value == 0)
        {
            await Solver.SolveLast(options);
        }
        else
        {
            await Solver.Solve([(uint)dayToRun.Value], options);
        }
    }
    else
    {
        await Solver.SolveAll(options);
    }
}
