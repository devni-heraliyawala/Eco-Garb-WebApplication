using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace templateProj.Models
{
    public class Graph
    {
        private Vector2D _sourceNode;
        private List<Vector2D> _listOfNodes;
        private List<Edge> _listOfEdges;

        #region Properties

        public List<Vector2D> AllNodes
        {
            get { return _listOfNodes; }
        }

        // Read-Write properties
        public Vector2D SourceVertex
        {
            get
            {
                return _sourceNode;
            }
            set
            {
                // SourceVertex is only valid if it is found in the graph.
                // Do not make any changes otherwise.
                for (int i = 0; i < _listOfNodes.Count; i++)
                {
                    if (_listOfNodes[i] == value)
                    {
                        _sourceNode = value;
                        break;
                    }
                }
            }
        }

        #endregion

        #region Graph Constructor
        public Graph()
        {
            _listOfEdges = new List<Edge>();
            _listOfNodes = new List<Vector2D>();

            _sourceNode = null; //_targetNode = null;

            //_totalCost = -1;
            //_optimalTraversal = new List<Vector2D>();
        }
        #endregion

        #region Add Vertex 
        /// <summary>
        /// Adds a vertex to the graph.
        /// </summary>
        /// <param name="node"></param>
        public void AddVertex(Vector2D node)
        {
            _listOfNodes.Add(node);

            // Reset stats due to a change to the graph.
            this.Reset();
        }
        #endregion

        #region Add Edge
        /// <summary>
        /// Adds an edge to the graph.
        /// </summary>
        /// <param name="edge"></param>
        public void AddEdge(Edge edge)
        {
            _listOfEdges.Add(edge);

            // Reset stats due to a change to the graph.
            this.Reset();
        }

        #endregion

        #region Calculate Shortest Paths
        /// <summary>
        /// As the name suggests, this method calculates the shortest path between the source and target node.
        /// If successful, it updates the TotalCost and the OptimalPath properties with the corresponding values.
        /// </summary>
        /// <returns>Success/Failure</returns>
        public bool CalculateShortestPath()
        {
            bool destUnreachable = false;

            if (_sourceNode == null) // || _targetNode == null)
            {
                return false;
            }

            // Algorithm starts here

            // Reset stats
            this.Reset();

            // Set the cost on the source node to 0 and flag it as visited
            _sourceNode.AggregateCost = 0;


            // if the targetnode is not the sourcenode
            // if (_targetNode.AggregateCost == Vector2D.INFINITY) {
            // Start the traversal across the graph
            this.PerformCalculationForAllNodes();
            //}


            //_totalCost = _targetNode.AggregateCost;


            if (destUnreachable)
            {
                return false;
            }

            return true;
        }
        #endregion

        #region Reterieve Shortest Path
        public List<Vector2D> RetrieveShortestPath(Vector2D targetNode)
        {
            List<Vector2D> shortestPath = new List<Vector2D>();

            if (targetNode == null)
            {
                throw new InvalidOperationException("Target node is null.");
            }
            else
            {
                Vector2D currentNode = targetNode;

                shortestPath.Add(currentNode);

                while (currentNode.EdgeWithLowestCost != null)
                {
                    currentNode = currentNode.EdgeWithLowestCost.GetTheOtherVertex(currentNode);
                    shortestPath.Add(currentNode);
                }
            }

            // reverse the order of the nodes, because we started from target node first
            shortestPath.Reverse();

            return shortestPath;
        }
        #endregion

        #region Get Connected Edges
        private List<Edge> GetConnectedEdges(Vector2D startNode)
        {
            List<Edge> connectedEdges = new List<Edge>();

            for (int i = 0; i < _listOfEdges.Count; i++)
            {
                if (_listOfEdges[i].GetTheOtherVertex(startNode) != null &&
                    !_listOfEdges[i].GetTheOtherVertex(startNode).Visited)
                {
                    connectedEdges.Add((Edge)_listOfEdges[i]);
                }
            }

            return connectedEdges;
        }
        #endregion

        #region Reset the Cost from this Instance

        /// <summary>
        /// Resets the costs from this instance.
        /// </summary>
        private void Reset()
        {
            // reset visited flag and reset the aggregate cost on all nodes
            for (int i = 0; i < _listOfNodes.Count; i++)
            {
                // The current node is now considered visited
                _listOfNodes[i].Visited = false;
                _listOfNodes[i].AggregateCost = Vector2D.INFINITY;
                _listOfNodes[i].EdgeWithLowestCost = null;
            }
        }

        #endregion

        #region Perfome Calculations for All Nodes
        private void PerformCalculationForAllNodes()
        {
            Vector2D currentNode = _sourceNode;

            // Start by marking the source node as visited
            currentNode.Visited = true;

            do
            {
                try
                {

                    Vector2D nextBestNode = null;

                    // Retrieve a list of all visited nodes and for each node, get a list of all edges
                    // that are not connected to visited nodes and update the aggregate cost on these other nodes.
                    foreach (Vector2D visitedNode in this.GetAListOfVisitedNodes())
                    {
                        foreach (Edge connectedEdge in this.GetConnectedEdges(visitedNode))
                        {

                            if (connectedEdge.Cost != int.MaxValue)
                            {
                                // Only update if the aggregate cost on the other node is infinite 
                                // or is greater and equal to the aggregate cost on the current visited node.
                                if (connectedEdge.GetTheOtherVertex(visitedNode).AggregateCost == Vector2D.INFINITY
                                    || (visitedNode.AggregateCost + connectedEdge.Cost) < connectedEdge.GetTheOtherVertex(visitedNode).AggregateCost)
                                {
                                    connectedEdge.GetTheOtherVertex(visitedNode).AggregateCost = visitedNode.AggregateCost + connectedEdge.Cost;

                                    // update the pointer to the edge with the lowest cost in the other node
                                    connectedEdge.GetTheOtherVertex(visitedNode).EdgeWithLowestCost = connectedEdge;
                                }


                                //if (nextBestNode == null || connectedEdge.GetTheOtherVertex(visitedNode).AggregateCost < nextBestNode.AggregateCost)
                                //{
                                //    nextBestNode = connectedEdge.GetTheOtherVertex(visitedNode);
                                //}
                            }
                            else
                            {
                                Console.WriteLine("cost of a edge cant be infinity");
                            }

                            if (nextBestNode == null || connectedEdge.GetTheOtherVertex(visitedNode).AggregateCost < nextBestNode.AggregateCost)
                            {
                                nextBestNode = connectedEdge.GetTheOtherVertex(visitedNode);
                            }

                        }
                    }

                    // Move the currentNode onto the next optimal node.
                    currentNode = nextBestNode;

                    // Now set the visited property of the current node to true
                    currentNode.Visited = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Message : " + ex.Message);
                    Debug.Write("Message : " + ex.Message);
                }

            } while (this.MoreVisitedNodes()); // Loop until every node's been visited.
        }
        #endregion

        #region Get List of Visited Nodes 

        private List<Vector2D> GetAListOfVisitedNodes()
        {
            List<Vector2D> listOfVisitedNodes = new List<Vector2D>();

            foreach (Vector2D node in _listOfNodes)
            {
                if (node.Visited)
                {
                    listOfVisitedNodes.Add(node);
                }
            }

            return listOfVisitedNodes;
        }

        #endregion

        #region More Visited Nodes

        private bool MoreVisitedNodes()
        {
            return GetAListOfVisitedNodes().Count < _listOfNodes.Count;
        }
        #endregion

    }
}