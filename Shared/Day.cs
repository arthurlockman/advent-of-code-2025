using AoCHelper;

namespace AdventOfCode2025.Shared;

public abstract class Day : BaseDay
{
    private const int Year = 2025;

    protected string Input { get; private set; }
    public record TestCase(string? ExpectedPart1, string? ExpectedPart2);
    public TestCase? Test { get; set; }

    protected Day()
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        var day = (int)CalculateIndex();
        AsyncHelper.RunSync(async () => await Utils.GetInputData(Year, day, InputFilePath));
        // ReSharper disable once VirtualMemberCallInConstructor
        Input = Utils.ReadInput(InputFilePath).Trim();
    }

    protected IEnumerable<string> GetInputLines()
    {
        return Input.Split("\n");
    }

    public async Task RunTests()
    {
        var sampleFilePath = Path.Combine("Inputs", $"{(int)CalculateIndex():D2}Sample.txt");
        if (File.Exists(sampleFilePath))
        {
            var sampleInput = Utils.ReadInput(sampleFilePath).Trim();
            var originalInput = Input;
            Console.WriteLine($"Running tests for Day {CalculateIndex()}...");
            if (Test?.ExpectedPart1 != null)
            {
                Input = sampleInput.Trim();
                var result1 = await Solve_1();
                if (result1 != Test.ExpectedPart1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[TEST FAILED] Part 1. Expected: {Test.ExpectedPart1}, Got: {result1}");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("[TEST PASSED] Part 1.");
                }

                Console.ResetColor();
            }

            if (Test?.ExpectedPart2 != null)
            {
                Input = sampleInput.Trim();
                var result2 = await Solve_2();
                if (result2 != Test.ExpectedPart2)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[TEST FAILED] Part 2. Expected: {Test.ExpectedPart2}, Got: {result2}");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("[TEST PASSED] Part 2.");
                }

                Console.ResetColor();
            }

            Input = originalInput;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[TEST FAILED] Sample input was not found.");
            Console.ResetColor();
        }
    }
}