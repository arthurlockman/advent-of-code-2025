using AoCHelper;

Action<SolverConfiguration> options = opt =>
{
    opt.ShowOverallResults = true;
    opt.ClearConsole = false;
};

if (args is ["--day", _] && int.TryParse(args[1], out var day))
{
    if (day == 0)
    {
        await Solver.SolveLast(options);
    }
    else
    {
        await Solver.Solve([(uint)day], options);
    }
}
else
{
    await Solver.SolveAll(options);
}
