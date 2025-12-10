using AdventOfCode2025.Shared;
using JetBrains.Annotations;
using NetTopologySuite.Geometries;

namespace AdventOfCode2025;

internal class Rectangle2D(Point2D corner1, Point2D corner2)
{
    public Point2D Corner1 { get; } = corner1;
    public Point2D Corner2 { get; } = corner2;

    public long Area => (Math.Abs(Corner1.X - Corner2.X) + 1) * (Math.Abs(Corner1.Y - Corner2.Y) + 1);

    public override bool Equals(object? obj) => obj is Rectangle2D other &&
                                                ((Corner1 == other.Corner1 && Corner2 == other.Corner2) ||
                                                 (Corner1 == other.Corner2 && Corner2 == other.Corner1));

    public override int GetHashCode() => HashCode.Combine(Corner1, Corner2);
}

[UsedImplicitly]
public class Day9 : Day
{
    public Day9()
    {
        Test = new TestCase("50", "24");
    }

    public override ValueTask<string> Solve_1()
    {
        var corners = InputLines.Value.Select(l => new Point2D(l)).ToList();
        var rectangles = corners.SelectMany(c1 => corners.Select(c2 => new Rectangle2D(c1, c2)))
            .Distinct()
            .Where(r => !Equals(r.Corner1, r.Corner2))
            .OrderByDescending(c => c.Area)
            .ToArray();
        return new ValueTask<string>($"{rectangles[0].Area}");
    }

    public override ValueTask<string> Solve_2()
    {
        var coordinates = InputLines.Value.Select(l =>
        {
            var split = l.Split(',');
            return new Coordinate(int.Parse(split[0]), int.Parse(split[1]));
        }).ToList();
        coordinates.Add(coordinates[0]);

        var geoFactory = new GeometryFactory();
        var boundingPolygon = geoFactory.CreatePolygon(coordinates.ToArray());

        var rectangle = coordinates.SelectMany(c1 => coordinates.Select(c2 => (
                geoFactory.ToGeometry(new Envelope(c1, c2)),
                (Math.Abs(c2.X - c1.X) + 1) * (Math.Abs(c2.Y - c1.Y) + 1))))
            .Distinct()
            .OrderByDescending(c => c.Item2)
            .AsParallel()
            .First(r => r.Item1.Within(boundingPolygon));
        return new ValueTask<string>($"{rectangle.Item2}");
    }
}