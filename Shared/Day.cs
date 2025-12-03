using AoCHelper;

namespace AdventOfCode2025.Shared;

public abstract class Day : BaseDay
{
    private const int Year = 2025;
    protected string Input { get; }

    protected Day(int day)
    {
        AsyncHelper.RunSync(async () => await Utils.GetInputData(Year, day, InputFilePath));
        // ReSharper disable once VirtualMemberCallInConstructor
        Input = Utils.ReadInput(InputFilePath).Trim();
    }

    protected IEnumerable<string> GetInputLines()
    {
        return Input.Split("\n");
    }
}