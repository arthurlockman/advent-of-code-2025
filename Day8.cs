using AdventOfCode2025.Shared;
using JetBrains.Annotations;

namespace AdventOfCode2025;

internal class JunctionBox(string line) : Point3D(line)
{
    private readonly List<JunctionBox> _connections = [];

    public bool IsConnectedTo(JunctionBox other)
    {
        return _connections.Contains(other);
    }

    public void ConnectTo(JunctionBox other)
    {
        _connections.Add(other);
        other._connections.Add(this);
    }

    public HashSet<JunctionBox> GetCircuit()
    {
        var visited = new HashSet<JunctionBox>();
        var queue = new Queue<JunctionBox>();
        queue.Enqueue(this);
        visited.Add(this);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            foreach (var connection in current._connections.Where(connection => visited.Add(connection)))
            {
                queue.Enqueue(connection);
            }
        }

        return visited.ToHashSet();
    }

    public override int GetHashCode() => HashCode.Combine(X, Y, Z);
    public override bool Equals(object? obj) => obj is Point3D other && X == other.X && Y == other.Y && Z == other.Z;
}

[UsedImplicitly]
public class Day8 : Day
{
    public Day8()
    {
        Test = new TestCase("40", "25272");
    }

    public override ValueTask<string> Solve_1()
    {
        var points = InputLines.Value.Select(l => new JunctionBox(l)).ToList();
        var edges = new Queue<Edge3D<JunctionBox>>(GenerateEdges(points).OrderBy(p => p.Cost));
        var connections = 0;
        while (connections < (TestMode ? 10 : 1000))
        {
            var nextEdge = edges.Dequeue();
            if (nextEdge.Point1.IsConnectedTo(nextEdge.Point2)) continue;
            nextEdge.Point1.ConnectTo(nextEdge.Point2);
            connections++;
        }

        var circuits = points.Select(p => p.GetCircuit()).Distinct(HashSet<JunctionBox>.CreateSetComparer()).ToArray();
        var circuitLengths = circuits.Select(c => c.Count).OrderByDescending(c => c).ToArray();
        var result = circuitLengths.Take(3).Aggregate((x, y) => x * y);
        return new ValueTask<string>($"{result}");
    }

    public override ValueTask<string> Solve_2()
    {
        var points = InputLines.Value.Select(l => new JunctionBox(l)).ToHashSet();
        var edges = new Queue<Edge3D<JunctionBox>>(GenerateEdges(points).OrderBy(p => p.Cost));
        while (edges.Count != 0)
        {
            var nextEdge = edges.Dequeue();
            if (nextEdge.Point1.IsConnectedTo(nextEdge.Point2)) continue;
            nextEdge.Point1.ConnectTo(nextEdge.Point2);
            var circuit = nextEdge.Point1.GetCircuit();
            if (circuit.SetEquals(points))
            {
                return new ValueTask<string>($"{nextEdge.Point1.X * nextEdge.Point2.X}");
            }
        }
        return new ValueTask<string>($"Unable to connect all junction boxes.");
    }

    private static List<Edge3D<JunctionBox>> GenerateEdges(IEnumerable<JunctionBox> points)
    {
        var junctionBoxes = points.ToArray();
        return junctionBoxes.SelectMany(point1 => junctionBoxes.Select(point2 => new Edge3D<JunctionBox>(point1, point2))).Distinct()
            .Where(e => !Equals(e.Point1, e.Point2)).ToList();
    }
}