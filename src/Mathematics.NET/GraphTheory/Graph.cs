// <copyright file="Graph.cs" company="Mathematics.NET">
// Mathematics.NET
// https://github.com/HamletTanyavong/Mathematics.NET
//
// MIT License
//
// Copyright (c) 2023-present Hamlet Tanyavong
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
using Microsoft.Extensions.Logging;

namespace Mathematics.NET.GraphTheory;

/// <summary>Represents a graph.</summary>
/// <typeparam name="TNode">A type that inherits from <see cref="Node"/>.</typeparam>
/// <typeparam name="TEdge">A type that inherits from <see cref="Edge"/>.</typeparam>
public class Graph<TNode, TEdge>
    where TNode : Node
    where TEdge : Edge
{
    private protected LinkedList<TNode> _nodes;

    public Graph()
    {
        _nodes = [];
    }

    //
    // Node and edge-specific methods
    //

    /// <summary>Add an edge to the graph.</summary>
    /// <param name="edge">An edge.</param>
    public virtual void AddEdge(TEdge edge)
    {
        if (_nodes.Contains(edge.Origin) && _nodes.Contains(edge.Destination))
        {
            edge.Origin.OutgoingEdges.AddLast(edge);
            edge.Destination.IncomingEdges.AddLast(edge);
        }
    }

    /// <summary>Add a list of the edges to the graph.</summary>
    /// <param name="edges">A list of edges.</param>
    public virtual void AddEdges(IEnumerable<TEdge> edges)
    {
        foreach (var edge in edges)
        {
            AddEdge(edge);
        }
    }

    /// <summary>Add a node to the graph.</summary>
    /// <param name="node">A node.</param>
    public virtual void AddNode(TNode node) => _nodes.AddLast(node);

    /// <summary>Add a list of nodes to the graph.</summary>
    /// <param name="nodes">A list of nodes.</param>
    public virtual void AddNodes(IEnumerable<TNode> nodes)
    {
        foreach (var node in nodes)
        {
            _nodes.AddLast(node);
        }
    }

    /// <summary>Get an edge from a node by indices or <see langword="default"/> if it does not exist.</summary>
    /// <param name="i">A node index.</param>
    /// <param name="j">An edge index.</param>
    /// <returns>An edge if it exists; otherwise, <see langword="default"/>.</returns>
    public virtual TEdge? GetEdgeOrDefault(int i, int j)
    {
        if (_nodes.ElementAtOrDefault(i) is TNode node)
            return (TEdge?)node.OutgoingEdges.ElementAtOrDefault(j);
        return null;
    }

    /// <summary>Get the node at a specific index or <see langword="default"/> if it does not exist.</summary>
    /// <param name="i">An index.</param>
    /// <returns>A node if it exists; otherwise, <see langword="default"/>.</returns>
    public virtual TNode? GetNodeOrDefault(int i) => _nodes.ElementAtOrDefault(i);

    /// <summary>Remove an edge from the graph if it exists.</summary>
    /// <param name="edge">The edge to remove.</param>
    public virtual void RemoveEdge(TEdge edge)
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
    public virtual void RemoveEdge(TNode origin, TNode destination)
    {
        if (origin.OutgoingEdges.FirstOrDefault(x => x.Destination == destination) is TEdge edge)
        {
            origin.OutgoingEdges.Remove(edge);
            destination.IncomingEdges.Remove(edge);
        }
    }

    /// <summary>Remove a node from the graph and all edges associated with it.</summary>
    /// <param name="node">A node.</param>
    public virtual void RemoveNode(TNode node) => _nodes.Remove(node);

    //
    // Other methods
    //

    /// <summary>Log the nodes and edges of the graph.</summary>
    public virtual void LogGraph(ILogger<Graph<TNode, TEdge>> logger)
    {
        const string template = "Node {NodeOriginNumber} => Edge {EdgeNumber} => Node {NodeDestinationNumber}";

        var nodes = _nodes.ToImmutableArray();
        for (int i = 0; i < nodes.Length; i++)
        {
            var node = nodes[i];
            var edges = node.OutgoingEdges.ToImmutableArray();
            if (edges.Length > 0)
            {
                for (int j = 0; j < edges.Length; j++)
                {
                    logger.LogInformation(template, i, j, nodes.IndexOf((TNode)edges[j].Destination));
                }
            }
        }
    }
}
