namespace AdventOfCode2025.Shared;

public class Point2D
{
    public long X { get; }

    public long Y { get; }

    public Point2D(long x, long y)
    {
        X = x;
        Y = y;
    }

    public Point2D(string line)
    {
        var parts = line.Split(',');
        X = long.Parse(parts[0]);
        Y = long.Parse(parts[1]);
    }

    public override string ToString() => $"{X},{Y}";
    public override int GetHashCode() => HashCode.Combine(X, Y);
    public override bool Equals(object? obj) => obj is Point2D other && X == other.X && Y == other.Y;

    public double Distance(Point2D other)
    {
        return Math.Sqrt(Math.Pow((double)X - other.X, 2)
                         + Math.Pow((double)Y - other.Y, 2));
    }
}
