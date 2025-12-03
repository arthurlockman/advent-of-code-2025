using AoCHelper;

namespace AdventOfCode2025.Shared;

public abstract class Day : BaseDay
{
    private const int Year = 2025;

    private string _input;
    protected string Input
    {
        get => _input;
        set => _input = value;
    }

    public record TestCase(string? ExpectedPart1, string? ExpectedPart2);
    public TestCase Test { get; set; }
    protected string SampleInput { get; private set; } = "";

    protected Day()
    {
        var day = (int)CalculateIndex();
        AsyncHelper.RunSync(async () => await Utils.GetInputData(Year, day, InputFilePath));
        // ReSharper disable once VirtualMemberCallInConstructor
        _input = Utils.ReadInput(InputFilePath).Trim();

        var sampleFilePath = Path.Combine("Inputs", $"{day:D2}Sample.txt");
        if (File.Exists(sampleFilePath))
        {
            SampleInput = Utils.ReadInput(sampleFilePath).Trim();
        }
    }

    protected IEnumerable<string> GetInputLines()
    {
        return Input.Split("\n");
    }

    public async Task RunTests()
    {
        var originalInput = _input;
        Console.WriteLine($"Running tests for Day {CalculateIndex()}...");
        if (Test.ExpectedPart1 != null)
        {
            _input = SampleInput.Trim();
            var result1 = await Solve_1();
            if (result1 != Test.ExpectedPart1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[TEST FAILED] Part 1. Expected: {Test.ExpectedPart1}, Got: {result1}");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[TEST PASSED] Part 1.");
                Console.ResetColor();
            }
        }

        if (Test.ExpectedPart2 != null)
        {
            _input = SampleInput.Trim();
            var result2 = await Solve_2();
            if (result2 != Test.ExpectedPart2)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[TEST FAILED] Part 2. Expected: {Test.ExpectedPart2}, Got: {result2}");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[TEST PASSED] Part 2.");
                Console.ResetColor();
            }
        }
        _input = originalInput;
    }
}