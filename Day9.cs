using AdventOfCode2025.Shared;

namespace AdventOfCode2025;

internal class Rectangle2D
{
    public Point2D Corner1 { get; }
    public Point2D Corner2 { get; }

    public Rectangle2D(Point2D corner1, Point2D corner2)
    {
        Corner1 = corner1;
        Corner2 = corner2;
    }

    public long Area => (Math.Abs(Corner1.X - Corner2.X) + 1) * (Math.Abs(Corner1.Y - Corner2.Y) + 1);

    public override bool Equals(object? obj) => obj is Rectangle2D other &&
                                                ((Corner1 == other.Corner1 && Corner2 == other.Corner2) ||
                                                 (Corner1 == other.Corner2 && Corner2 == other.Corner1));

    public override int GetHashCode() => HashCode.Combine(Corner1, Corner2);

    public Point2D[] GetCorners() => [Corner1, Corner2, new(Corner1.X, Corner2.Y), new(Corner2.X, Corner1.Y)];
}

internal class Polygon2D
{
    public Point2D[] Corners { get; }

    private readonly List<(Point2D, Point2D)> _lineSegments = [];

    public Polygon2D(Point2D[] corners)
    {
        Corners = corners;
        for (var i = 0; i < corners.Length - 1; i++)
        {
            _lineSegments.Add(new ValueTuple<Point2D, Point2D>(corners[i], corners[i + 1]));
        }
        _lineSegments.Add(new ValueTuple<Point2D, Point2D>(corners[^1], corners[0]));
    }

    public bool ContainsPoint(Point2D point)
    {
        // the basic premise here is the conjecture that if a ray cast from a point in a fixed direction
        // intersects the polygon an odd number of times, the point is within the polygon. if it intersects
        // an even number of times, it's outside the polygon.
        // what we do then is turn this arbitrary polygon into a collection of line segments, then cast a ray
        // from our point in a fixed direction (in this case toward 0,0) and see how many line segments it intersects.
        // https://en.wikipedia.org/wiki/Point_in_polygon#Ray_casting_algorithm
        // https://www.reddit.com/r/algorithms/comments/9moad4/what_is_the_simplest_to_implement_line_segment/
        long Cross(Point2D a, Point2D b)
        {
            return a.X * b.Y - a.Y * b.X;
        }

        long Orient(Point2D a, Point2D b, Point2D c)
        {
            return Cross(new Point2D(b.X - a.X, b.Y - a.Y), new Point2D(c.X - a.X, c.Y - a.Y));
        }

        bool Intersects(Point2D a, Point2D b, Point2D c, Point2D d)
        {
            long oa = Orient(c, d, a),
                ob = Orient(c, d, b),
                oc = Orient(a, b, c),
                od = Orient(a, b, d);
            // Proper intersection exists iff opposite signs
            return (oa * ob < 0 && oc * od < 0);
        }

        // if the point is one of the corners of the polygon it's obviously in the polygon
        if (Corners.Any(p => p.Equals(point))) return true;

        // if it's not then we need to check if it's contained using the raycasting method
        var intersections = _lineSegments.Count(l => Intersects(l.Item1, l.Item2, point, new Point2D(0, 0)));
        return intersections % 2 != 0;
    }
}

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
        var corners = InputLines.Value.Select(l => new Point2D(l)).ToList();
        var boundingPolygon = new Polygon2D(corners.ToArray());
        var rectangles = corners.SelectMany(c1 => corners.Select(c2 => new Rectangle2D(c1, c2)))
            .Distinct()
            .AsParallel()
            .Where(r => !Equals(r.Corner1, r.Corner2))
            .Where(r => r.GetCorners().All(c => boundingPolygon.ContainsPoint(c)))
            .OrderByDescending(c => c.Area)
            .ToArray();
        return new ValueTask<string>($"{rectangles[0].Area}");
    }
}