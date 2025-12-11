using AoCHelper;

namespace AdventOfCode2025.Shared;

public abstract class Day : BaseDay
{
    private const int Year = 2025;

    protected string Input { get; private set; }
    protected string RawInput { get; private set; }
    public record TestCase(string? ExpectedPart1, string? ExpectedPart2);
    public TestCase? Test { get; set; }
    public bool TestMode { get; set; }
    protected Lazy<string[]> InputLines => new(() => Input.Split("\n"));

    protected Day()
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        var day = (int)CalculateIndex();
        AsyncHelper.RunSync(async () => await Utils.GetInputData(Year, day, InputFilePath));
        // ReSharper disable once VirtualMemberCallInConstructor
        RawInput = Utils.ReadInput(InputFilePath);
        Input = RawInput.Trim();
    }

    public async Task RunTests()
    {
        var day = (int)CalculateIndex();

        var sampleFilePath1 = Path.Combine("Inputs", $"{day:D2}Sample.txt");
        var sampleFilePath2 = Path.Combine("Inputs", $"{day:D2}Sample2.txt");

        if (!File.Exists(sampleFilePath2))
        {
            sampleFilePath2 = sampleFilePath1;
        }

        var originalInput = Input;
        var originalRawInput = RawInput;
        TestMode = true;
        Console.WriteLine($"Running tests for Day {CalculateIndex()}...");

        if (Test?.ExpectedPart1 != null)
        {
            if (File.Exists(sampleFilePath1))
            {
                var sampleInput = Utils.ReadInput(sampleFilePath1);
                Input = sampleInput.Trim();
                RawInput = sampleInput;
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
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[TEST FAILED] Sample input file not found: {sampleFilePath1}");
                Console.ResetColor();
            }
        }

        if (Test?.ExpectedPart2 != null)
        {
            if (File.Exists(sampleFilePath2))
            {
                var sampleInput = Utils.ReadInput(sampleFilePath2);
                Input = sampleInput.Trim();
                RawInput = sampleInput;
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
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[TEST FAILED] Sample input file not found: {sampleFilePath2}");
                Console.ResetColor();
            }
        }

        Input = originalInput;
        RawInput = originalRawInput;
        TestMode = false;
    }
}