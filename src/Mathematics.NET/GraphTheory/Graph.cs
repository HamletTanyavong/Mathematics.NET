// <copyright file="Graph.cs" company="Mathematics.NET">
// Mathematics.NET
// https://github.com/HamletTanyavong/Mathematics.NET
//
// MIT License
//
// Copyright (c) 2023 Hamlet Tanyavong
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// </copyright>

#pragma warning disable IDE0058

using System.Collections.Immutable;

namespace Mathematics.NET.GraphTheory;

/// <summary>Represents a graph</summary>
/// <typeparam name="T">A type that inherits from <see cref="Node"/></typeparam>
/// <typeparam name="U">A type that inherits from <see cref="Edge"/></typeparam>
public class Graph<T, U>
    where T : Node
    where U : Edge
{
    private protected LinkedList<T> _nodes;

    public Graph()
    {
        _nodes = [];
    }

    //
    // Node and edge-specific methods
    //

    /// <summary>Add an edge to the graph.</summary>
    /// <param name="edge">An edge</param>
    public virtual void AddEdge(U edge)
    {
        if (_nodes.Contains(edge.Origin) && _nodes.Contains(edge.Destination))
        {
            edge.Origin.OutgoingEdges.AddLast(edge);
            edge.Destination.IncomingEdges.AddLast(edge);
        }
    }

    /// <summary>Add a node to the graph.</summary>
    /// <param name="node">A node</param>
    public virtual void AddNode(T node) => _nodes.AddLast(node);

    /// <summary>Add a list of nodes to the graph.</summary>
    /// <param name="nodes">A list of nodes</param>
    public virtual void AddNodes(IEnumerable<T> nodes)
    {
        foreach (var node in nodes)
        {
            _nodes.AddLast(node);
        }
    }

    /// <summary>Get an edge from a node by indices or <see langword="null"/> if it does not exist.</summary>
    /// <param name="i">A node index</param>
    /// <param name="j">An edge index</param>
    /// <returns>An edge if it exists; otherwise, <see langword="null"/></returns>
    public virtual U? GetEdgeOrNull(int i, int j)
    {
        if (_nodes.ElementAtOrDefault(i) is T node)
        {
            return (U?)node.OutgoingEdges.ElementAtOrDefault(j);
        }
        return null;
    }

    /// <summary>Get the node at a specific index or <see langword="null"/> if it does not exist.</summary>
    /// <param name="i">An index</param>
    /// <returns>A node if it exists; otherwise, <see langword="null"/></returns>
    public virtual T? GetNodeOrNull(int i) => _nodes.ElementAtOrDefault(i);

    /// <summary>Remove an edge from the graph if it exists.</summary>
    /// <param name="edge">The edge to remove</param>
    public virtual void RemoveEdge(U edge)
    {
        if (_nodes.FirstOrDefault(x => x.OutgoingEdges.Any(x => x == edge)) is Node origin &&
            _nodes.FirstOrDefault(x => x.IncomingEdges.Any(x => x == edge)) is Node destination)
        {
            origin.OutgoingEdges.Remove(edge);
            origin.IncomingEdges.Remove(edge);
        }
    }

    /// <summary>Remove an edge from the graph if it exists between two specified origin and destination nodes.</summary>
    /// <param name="origin">The origin node of the edge.</param>
    /// <param name="destination">The destination node of the edge.</param>
    public virtual void RemoveEdge(T origin, T destination)
    {
        if (origin.OutgoingEdges.FirstOrDefault(x => x.Destination == destination) is U edge)
        {
            origin.OutgoingEdges.Remove(edge);
            destination.IncomingEdges.Remove(edge);
        }
    }

    /// <summary>Remove a node from the graph and all edges associated with it.</summary>
    /// <param name="node">A node</param>
    public virtual void RemoveNode(T node) => _nodes.Remove(node);

    //
    // Other methods
    //

    /// <summary>Print the graph to the console.</summary>
    public virtual void Print()
    {
        const string tab = "    ";

        var nodes = _nodes.ToImmutableArray();

        for (int i = 0; i < nodes.Length; i++)
        {
            var node = nodes[i];
            var edges = node.OutgoingEdges.ToImmutableArray();
            if (edges.Length > 0)
            {
                Console.WriteLine($"Node {i}");

                for (int j = 0; j < edges.Length; j++)
                {
                    Console.WriteLine($"{tab}=> Edge {j} => Node {nodes.IndexOf((T)edges[j].Destination)}");
                }
            }
        }
    }
}
