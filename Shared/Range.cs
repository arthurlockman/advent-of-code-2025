namespace AdventOfCode2025.Shared;

public record Range(long Start, long End)
{
    public bool Contains(long item) => item >= Start && item <= End;
    public bool Overlaps(Range other) => other.End >= Start && other.Start <= End;
    public Range Merge(Range other) => new(Math.Min(Start, other.Start), Math.Max(End, other.End));
    public long Length => End - Start + 1;
}