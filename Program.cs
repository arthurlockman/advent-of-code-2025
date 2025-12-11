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
        // Workaround for SolveLast bug with days > 9
        .OrderBy(t => t.Name.Length)
        .ThenBy(t => t.Name);

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
            // Workaround for SolveLast bug with days > 9
            var allDays = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(BaseDay)) && !t.IsAbstract)
                .Select(t => t.Name)
                .Where(name => name.StartsWith("Day") && int.TryParse(name.Substring(3), out _))
                .Select(name => int.Parse(name.Substring(3)))
                .OrderByDescending(day => day)
                .FirstOrDefault();

            if (allDays > 0)
            {
                await Solver.Solve([(uint)allDays], options);
            }
            else
            {
                await Solver.SolveLast(options);
            }
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
