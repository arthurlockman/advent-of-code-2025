namespace AdventOfCode2025.Shared;

public class Point3D
{
    public long X { get; }

    public long Y { get; }

    public long Z { get; }

    public Point3D(long x, long y, long z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Point3D(string line)
    {
        var parts = line.Split(',');
        X = long.Parse(parts[0]);
        Y = long.Parse(parts[1]);
        Z = long.Parse(parts[2]);
    }

    public override string ToString() => $"{X},{Y},{Z}";
    public override int GetHashCode() => HashCode.Combine(X, Y, Z);
    public override bool Equals(object? obj) => obj is Point3D other && X == other.X && Y == other.Y && Z == other.Z;

    public double Distance(Point3D other)
    {
        return Math.Sqrt(Math.Pow((double)X - other.X, 2)
                         + Math.Pow((double)Y - other.Y, 2)
                         + Math.Pow((double)Z - other.Z, 2));
    }
}

public record Edge3D<T> where T : Point3D
{
    public T Point1 { get; }
    public T Point2 { get; }
    public double Cost => Point1.Distance(Point2);

    public Edge3D(T p1, T p2)
    {
        Point1 = p1;
        Point2 = p2;
    }

    public virtual bool Equals(Edge3D<T>? other) => (Point1.Equals(other?.Point1) && Point2.Equals(other.Point2)) ||
                                                    (Point1.Equals(other?.Point2) && Point2.Equals(other.Point1));

    public override int GetHashCode() => Point1.GetHashCode() ^ Point2.GetHashCode();
}