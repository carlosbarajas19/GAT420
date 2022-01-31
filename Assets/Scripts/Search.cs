using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Priority_Queue;


public static class Search
{
    public delegate bool SearchAlgorithm(GraphNode source, GraphNode destination, ref List<GraphNode> path, int maxSteps);

    static public bool BuildPath(SearchAlgorithm searchAlgorithm, GraphNode source, GraphNode destination, ref List<GraphNode> path, int steps = int.MaxValue)
    {
        if (source == null || destination == null) return false;

        // reset graph nodes
        GraphNode.ResetNodes();

        // search for path from source to destination nodes        
        bool found = searchAlgorithm(source, destination, ref path, steps);

        return found;
    }

    public static bool AStar(GraphNode source, GraphNode destination, ref List<GraphNode> path, int maxSteps)
    {
        bool found = false;
        var nodes = new SimplePriorityQueue<GraphNode>();
        source.cost = 0;
        float heuristic = Vector3.Distance(source.transform.position, destination.transform.position);
        nodes.Enqueue(source, source.cost + heuristic);

        int steps = 0;
        while (!found && nodes.Count > 0 && steps++ < maxSteps)
        {
            // dequeue node
            var node = nodes.Dequeue();
        
            // check if node is the destination node
            if (node == destination)
	{
                // set found to true
                found = true;
                break;
            }

            foreach (var neighbor in node.neighbors)
            {
                neighbor.visited = true; // not needed for algorithm (debug)

                // calculate cost to neighbor = node cost + distance to neighbor				
                float cost = node.cost + node.DistanceTo(neighbor);
                // if cost < neighbor cost, add to priority queue
                if (cost < neighbor.cost)
                {
                    // set neighbor cost to cost
                    neighbor.cost = cost;
                    // set neighbor parent to node
                    neighbor.parent = node;
                    // calculate heuristic = distance from neighbor to destination
                    heuristic = Vector3.Distance(neighbor.transform.position, destination.transform.position);

                    // enqueue without duplicates, neighbor cost + heuristic as priority
                    // the closer the neighbor to the destination the higher the priority
                    nodes.EnqueueWithoutDuplicates(neighbor, cost + heuristic);
        
                }
            }
        }

        if (found)
        {
            // create path from destination to source using node parents
            path = new List<GraphNode>();
            CreatePathFromParents(destination, ref path);
        }
        else
        {
            path = nodes.ToList();
        }

        return found;
    }

    public static bool Dijkstra(GraphNode source, GraphNode destination, ref List<GraphNode> path, int maxSteps)
    {
        bool found = false;
        var nodes = new SimplePriorityQueue<GraphNode>();
        source.cost = 0;
        nodes.Enqueue(source, source.cost);

        int steps = 0;
        while (!found && nodes.Count > 0 && steps++ < maxSteps)
        {
            // dequeue node
            var node = nodes.Dequeue();
        
            // check if node is the destination node
            if (node = destination)
	        {
                // set found to true
                found = true;
                break;
            }

            foreach (var neighbor in node.neighbors)
            {
                neighbor.visited = true; // not needed for algorithm (debug)

                // calculate cost to neighbor = node cost + distance to neighbor
                float cost = node.cost + node.DistanceTo(neighbor);
                // if cost < neighbor cost, add to priority queue
                if (cost < neighbor.cost)
                {
                    // set neighbor cost to cost
                    neighbor.cost = cost;
                    // set neighbor parent to node
                    neighbor.parent = node;
                    // enqueue without duplicates, neighbor with cost as priority
                    nodes.EnqueueWithoutDuplicates(neighbor, cost);

                }
            }
        }

        if (found)
        {
            // create path from destination to source using node parents
            path = new List<GraphNode>();
            CreatePathFromParents(destination, ref path);
        }
        else
        {
            path = nodes.ToList();
        }

        return found;
    }

    public static bool DFS(GraphNode source, GraphNode destination, ref List<GraphNode> path, int maxSteps)
    {
        bool found = false;

        var nodes = new Stack<GraphNode>();

        nodes.Push(source);
        int steps = 0;
        while(!found & nodes.Count > 0 && steps++ < maxSteps)
        {
            var node = nodes.Peek();
            node.visited = true;

            bool forward = false;

            foreach(var neighbor in node.neighbors)
            {
                if(!neighbor.visited)
                {
                    nodes.Push(neighbor);
                    forward = true;

                    if(neighbor == destination)
                    {
                        found = true;
                    }
                    break;

                }
            }

            if (!forward) nodes.Pop();
        }

        path = new List<GraphNode>(nodes);
        path.Reverse();
        return found;
    }

    public static bool BFS(GraphNode source, GraphNode destination, ref List<GraphNode> path, int maxSteps)
    {
        bool found = false;
        var nodes = new Queue<GraphNode>();
        source.visited = true;

        nodes.Enqueue(source);

        int steps = 0;
        while (!found && nodes.Count > 0 && steps++ < maxSteps)
        {
            // dequeue node
            var node = nodes.Dequeue();
            // iterate through neighbors of node
            foreach (var neighbor in node.neighbors)
            {
                // check if neighbor has not been visited
                if (!neighbor.visited)
                {
                    // set neighbor visited to true
                    neighbor.visited = true;
                    // set neighbor parent to node
                    neighbor.parent = node;
                    // enqueue neighbor
                    nodes.Enqueue(neighbor);
        
                }
                // check if neighbor is the destination node
                if (neighbor == destination)
		        {
                    // set found to true
                    found = true;
                    break;
                }
            }
        }

        if (found)
        {
            // create path from destination to source using node parents
            path = new List<GraphNode>();
            CreatePathFromParents(destination, ref path);
        }
        else
        {
            // did not find destination, convert nodes queue to path
            path = nodes.ToList();
        }

        return found;

        //swap all edge.nodeB to neighbor
    }

    public static void CreatePathFromParents(GraphNode node, ref List<GraphNode> path)
    {
        // while node not null
        while (node != null)
        {
            // add node to list path
            path.Add(node);
            // set node to node parent
            node = node.parent;
        }

        // reverse path
        path.Reverse();
    }

}