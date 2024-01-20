// <copyright file="Node.cs" company="Mathematics.NET">
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

using Mathematics.NET.GraphTheory.Abstractions;

namespace Mathematics.NET.GraphTheory;

/// <summary>Represents a node on a graph</summary>
public class Node : IGraphComponent
{
    public Node()
    {
        IncomingEdges = [];
        OutgoingEdges = [];
    }

    /// <summary>Get all incoming edges.</summary>
    public LinkedList<Edge> IncomingEdges { get; set; }

    /// <summary>Get all outgoing edges.</summary>
    public LinkedList<Edge> OutgoingEdges { get; set; }

    //
    // Methods
    //

    /// <summary>Get a list of child nodes of this node.</summary>
    /// <returns>A list of nodes</returns>
    /// <remarks>The returned list does not include this node if there is a cycle.</remarks>
    public IEnumerable<Node> ChildNodes() => OutgoingEdges
        .Select(x => x.Destination)
        .Where(x => x != this);

    /// <summary>Get a list of all descendant nodes of this node.</summary>
    /// <returns>A list of nodes</returns>
    public IEnumerable<Node> DescendantNodes()
    {
        Queue<Node> queue = new();
        queue.Enqueue(this);

        HashSet<Node> descendants = [];

        while (queue.Count > 0)
        {
            foreach (var node in queue.Dequeue().ChildNodes())
            {
                if (descendants.Add(node))
                {
                    queue.Enqueue(node);
                }
            }
        }

        return descendants;
    }

    /// <summary>Get a list of the parent nodes of this node.</summary>
    /// <returns>A list of nodes</returns>
    /// <remarks>The returned list does not include this node if there is a cycle.</remarks>
    public IEnumerable<Node> ParentNodes() => IncomingEdges
        .Select(x => x.Origin)
        .Where(x => x != this);

    /// <summary>Remove an incoming edge from the node if it exists.</summary>
    /// <param name="edge">An incoming edge</param>
    public virtual void RemoveIncomingEdge(Edge edge) => IncomingEdges.Remove(edge);

    /// <summary>Remove all incoming edges from the node.</summary>
    public virtual void RemoveIncomingEdges() => IncomingEdges.Clear();

    /// <summary>Remove all edges from the node.</summary>
    public virtual void RemoveEdges()
    {
        RemoveIncomingEdges();
        RemoveOutgoingEdges();
    }

    /// <summary>Remove an outgoing edge from the node if it exists.</summary>
    /// <param name="edge">An outgoing edge</param>
    public virtual void RemoveOutgoingEdge(Edge edge) => OutgoingEdges.Remove(edge);

    /// <summary>Remove all outgoing edges from the node.</summary>
    public virtual void RemoveOutgoingEdges() => OutgoingEdges.Clear();
}
