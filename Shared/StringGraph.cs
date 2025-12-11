using Unchase.Satsuma.Adapters;
using Unchase.Satsuma.Core;
using Unchase.Satsuma.Core.Enums;
using Unchase.Satsuma.Core.Extensions;

namespace AdventOfCode2025.Shared;

public class StringGraph
{
    public CustomGraph Graph { get; } = new();

    private readonly Dictionary<string, Node> _nameToNode = new();
    private readonly Dictionary<Node, string> _nodeToName = new();

    private readonly Dictionary<(string, string), long> _pathMemo = new();

    private Node GetOrCreateNode(string name)
    {
        if (_nameToNode.TryGetValue(name, out var value))
        {
            return value;
        }

        var newNode = Graph.AddNode();

        _nameToNode[name] = newNode;
        _nodeToName[newNode] = name;

        return newNode;
    }

    public void AddConnection(string from, string to)
    {
        var u = GetOrCreateNode(from);
        var v = GetOrCreateNode(to);

        Graph.AddArc(u, v, Directedness.Directed);
    }

    public long CountPaths(string fromString, string toString)
    {
        var from = _nameToNode[fromString];
        var to = _nameToNode[toString];
        if (_pathMemo.TryGetValue((fromString, toString), out var cached))
        {
            return cached;
        }

        if (from == to)
        {
            return _pathMemo[(fromString, toString)] = 1L;
        }

        return _pathMemo[(fromString, toString)] = Graph.Arcs(from, ArcFilter.Forward)
            .Select(arc => Graph.Other(arc, from))
            .Sum(adjacent => CountPaths(_nodeToName[adjacent], toString));
    }
}