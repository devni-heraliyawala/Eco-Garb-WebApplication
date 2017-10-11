using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using templateProj.Models;

namespace templateProj.Controllers
{
    public class DijkstrasController : Controller
    {
        RoutingAlgoController rc = new RoutingAlgoController();
        DataContext db = new DataContext();
        Dictionary<int, Vector2D> resultIndexVertexMapping = new Dictionary<int, Vector2D>();
        List<Vector2D> verticesList = new List<Vector2D>();
        List<int> bestRouteVertexIdList = new List<int>();
        List<Edge> edgesList = new List<Edge>();
        List<StringBuilder> LowestCostPathList = new List<StringBuilder>();
        Vector2D sourceVector;
        public String routeDistance;

        public const int RELATIVE_VARIENCE = 2;


        public ActionResult FindBestRouteBySpecialValue(int specialValue)
        {
            string uname = HttpContext.Session["Uname"].ToString();
            UserModel um = db.Umodel.Find(uname);

            string[] addressList, cnameList, branchList, latList, lngList, totalQtyList;

            rc.QualifiedCustomerDetailLists(um, out addressList, out cnameList, out branchList, out latList, out lngList, out totalQtyList);

            #region Check Total Trash Quantities through Special Value

            //to insert qualified lists
            String[] qualifiedAddrList = new String[addressList.Length];
            String[] qualifiedCnameList = new String[addressList.Length];
            String[] qualifiedBranchList = new String[addressList.Length];
            String[] qualifiedTotalQtyList = new String[addressList.Length];
            String[] qualifiedLatList = new String[addressList.Length];
            String[] qualifiedLngList = new String[addressList.Length];


            int count = 0;
            for (int i = 0; i < totalQtyList.Length; i++)
            {
                if (Convert.ToInt32(totalQtyList[i]) >= specialValue)
                {
                    qualifiedAddrList[i] = addressList[i];
                    qualifiedCnameList[i] = cnameList[i];
                    qualifiedBranchList[i] = branchList[i];
                    qualifiedTotalQtyList[i] = totalQtyList[i];
                    qualifiedLatList[i] = latList[i];
                    qualifiedLngList[i] = lngList[i];
                    count++;
                }
            }
            //to remove null values from the above arrays
            String[] finalAddrList = new String[count];
            String[] finalCnameList = new String[count];
            String[] finalBranchList = new String[count];
            String[] finalTotalQtyList = new String[count];
            String[] finalLatList = new String[count];
            String[] finalLngList = new String[count];

            finalAddrList = qualifiedAddrList.Where(s => !String.IsNullOrEmpty(s)).ToArray();
            finalCnameList = qualifiedCnameList.Where(s => !String.IsNullOrEmpty(s)).ToArray();
            finalBranchList = qualifiedBranchList.Where(s => !String.IsNullOrEmpty(s)).ToArray();
            finalTotalQtyList = qualifiedTotalQtyList.Where(s => !String.IsNullOrEmpty(s)).ToArray();
            finalLatList = qualifiedLatList.Where(s => !String.IsNullOrEmpty(s)).ToArray();
            finalLngList = qualifiedLngList.Where(s => !String.IsNullOrEmpty(s)).ToArray();
            #endregion

            #region Perform Shortest Path Algorithm on Above Qualified Customers Detail Lists

            //DijkstrasController dijk = new DijkstrasController();



            var recCompany = db.recCompanyModel.Where(r => r.RecCompanyName == um.CompanyName).FirstOrDefault();

            sourceVector = new Vector2D(recCompany.Lat, recCompany.Lng, false, recCompany.Address, recCompany.RecCompanyName, "", "0");
            //verticesList.Add(sourceVector);
            String sourceVertex = sourceVector.VertexID.ToString();

            List<Vector2D> quantityBasedQualifiedVerticesList = CreateVertices(finalAddrList, finalCnameList, finalBranchList, finalLatList, finalLngList, finalTotalQtyList, sourceVector);

            string[] newLatList, newLngList, newAddressList, newCnameList, newBranchList, newTotQtyList;
            string finalBestPath;
            FinalCustomerDetailsLists(quantityBasedQualifiedVerticesList, out newLatList, out newLngList, out newAddressList, out newCnameList, out newBranchList, out newTotQtyList, out finalBestPath);

            #endregion

            return Json(new { addressList = newAddressList, cnameList = newCnameList, branchList = newBranchList, totalQtyList = newTotQtyList, latList = newLatList, lngList = newLngList, finalBestPath = finalBestPath, routeDistance = routeDistance }, JsonRequestBehavior.AllowGet);
        }



        #region Set Source Vertex
        public ActionResult SetSourceVertex()
        {
            string uname = HttpContext.Session["Uname"].ToString();
            UserModel um = db.Umodel.Find(uname);

            var recCompany = db.recCompanyModel.Where(r => r.RecCompanyName == um.CompanyName).FirstOrDefault();

            sourceVector = new Vector2D(recCompany.Lat, recCompany.Lng, false, recCompany.Address, recCompany.RecCompanyName, "", "0");
            //verticesList.Add(sourceVector);
            String sourceVertex = sourceVector.VertexID.ToString();

            String[] recCompanyDetails =
            {
                recCompany.Lat,
                recCompany.Lng,
                recCompany.RecCompanyName ,
                recCompany.Address
            };

            //query for get all the vertices from the database

            string[] addressList, cnameList, branchList, latList, lngList, totalQtyList;
            rc.QualifiedCustomerDetailLists(um, out addressList, out cnameList, out branchList, out latList, out lngList, out totalQtyList);

            List<Vector2D> qualifiedVerticesList = CreateVertices(addressList, cnameList, branchList, latList, lngList, totalQtyList, sourceVector);
            string[] newLatList, newLngList, newAddressList, newCnameList, newBranchList, newTotQtyList;
            string finalBestPath;

            FinalCustomerDetailsLists(qualifiedVerticesList, out newLatList, out newLngList, out newAddressList, out newCnameList, out newBranchList, out newTotQtyList, out finalBestPath);

            return Json(new { addressList = newAddressList, cnameList = newCnameList, branchList = newBranchList, totalQtyList = newTotQtyList, latList = newLatList, lngList = newLngList, finalBestPath = finalBestPath, routeDistance= routeDistance }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Create Vertices
        public List<Vector2D> CreateVertices(string[] addressList, string[] cnameList, string[] branchList, string[] latList, string[] lngList, string[] totalQtyList, Vector2D sourceVector)
        {
            verticesList.Add(sourceVector);
            for (int i = 0; i < addressList.Count(); i++)
            {
                Vector2D newVertex = new Vector2D(latList[i], lngList[i], false, addressList[i], cnameList[i], branchList[i], totalQtyList[i] );
                verticesList.Add(newVertex);
            }

            return FindDistanceBetweenLocations(verticesList);
           

           
        }
        #endregion

        #region Find Distances Betweeen Locations

        private List<Vector2D> FindDistanceBetweenLocations(List<Vector2D> verticesList)
        {
            var earthRadius = 6371; // in KM

            for (int i = 0; i < verticesList.Count; i++)
            {
                for (int j = 0; j < verticesList.Count; j++)
                {
                    if (verticesList[i].VertexID == verticesList[j].VertexID)
                    {
                        continue;
                    }
                    else
                    {
                        var dLat = (Convert.ToDouble(verticesList[i].Lat) - Convert.ToDouble(verticesList[j].Lat)) * Math.PI / 180;
                        var dLng = (Convert.ToDouble(verticesList[i].Lng) - Convert.ToDouble(verticesList[j].Lng)) * Math.PI / 180;

                        var lat1 = (Convert.ToDouble(verticesList[i].Lat)) * Math.PI / 180;
                        var lat2 = (Convert.ToDouble(verticesList[j].Lat)) * Math.PI / 180;

                        var theta = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Sin(dLng / 2) * Math.Sin(dLng / 2) * Math.Cos(lat1) * Math.Cos(lat2);
                        var c = 2 * Math.Atan2(Math.Sqrt(theta), Math.Sqrt(1 - theta));
                        var distance = earthRadius * c;

                        int upperDistance = (int)Math.Ceiling(distance);

                        if (i == 0 || j == 0)
                        {
                            //i==0 means you are calculating distances between original sorce to each and every destination
                            // j==0 means you are calulating distances from each and every nodes to original source.
                            // when you calculate distance from original source to all other varience there is a considerable
                            // varience between arc lenth. so theta might give inaccurate results. to avoid that this constant has been used
                            // i is 1!=0 then we are calculating relative distances then theta want change much so we dont need to add a constant
                            upperDistance = upperDistance + RELATIVE_VARIENCE;
                        }

                        if (upperDistance < 10)
                        {
                            CreateEdgesForTheVertex(verticesList[i], verticesList[j], upperDistance);
                        }
                        else
                        {
                            CreateEdgesForTheVertex(verticesList[i], verticesList[j], int.MaxValue);
                        }
                    }

                }
            }
            return CalculateShortestPaths();
            
        }

        #endregion

        #region Create Edges For Each Vertex
        private void CreateEdgesForTheVertex(Vector2D pointA, Vector2D pointB, int distance)
        {

            Edge newEdge;

            newEdge = new Edge(pointA, pointB, distance);

            edgesList.Add(newEdge);
        }

        #endregion

        #region Calculate Shortest Paths 

        public List<Vector2D> CalculateShortestPaths()
        {
            // Create a brand new graph, fill it with vertices and edges, and calculate the minimal path
            Graph graph = new Graph();

            for (int i = 0; i < verticesList.Count; i++)
            {
                graph.AddVertex(verticesList[i]);
            }

            for (int i = 0; i < edgesList.Count; i++)
            {
                graph.AddEdge(edgesList[i]);
            }

            graph.SourceVertex = sourceVector;

            if (graph.CalculateShortestPath())
            {
                string directionIndicator = " -> ";
                int lowestCostPath = 0;

                foreach (Vector2D targetNode in graph.AllNodes) // Find the shortest path for every other node from the source node
                {
                    StringBuilder buildOptimalPathText = new StringBuilder();// build the string for the optimal path

                    foreach (Vector2D currentNode in graph.RetrieveShortestPath(targetNode))
                    {
                        buildOptimalPathText.Append(currentNode.VertexID);// build up the path string
                        buildOptimalPathText.Append(directionIndicator);
                    }
                    LowestCostPathList.Add(buildOptimalPathText);
                    buildOptimalPathText.Remove(buildOptimalPathText.Length - directionIndicator.Length,
                                directionIndicator.Length);  // remove the extraneous direction indicator at the end of the string

                    // display it and add the resulting index position of the new list item as a key 
                    // into the index to vertex mapping dictionary along with the targetnode.
                    resultIndexVertexMapping.Add(lowestCostPath++, targetNode);
                }
                return FindBestRouteVerticesList(graph);
            }
            else
            {
                return null;
            }
      
      
        }
        #endregion

        #region Find Best Route Vertices List
        private List<Vector2D> FindBestRouteVerticesList(Graph graph)
        {
            String bPath = BestRoute(graph);

            string[] modifiedPath = bPath.Replace(" -> ", ",").ToString().Split(',');


            for (int i = 0; i < modifiedPath.Length; i++)
            {
                bestRouteVertexIdList.Add(Convert.ToInt32(modifiedPath[i]));
            }

            List<Vector2D> bestRouteVerticesList = new List<Vector2D>();

            // from the vertices list select these values only 
            for (int i = 0; i < bestRouteVertexIdList.Count; i++)
            {
                for (int j = 0; j < verticesList.Count; j++)
                {
                    if (bestRouteVertexIdList[i] == verticesList[j].VertexID)
                    {
                        bestRouteVerticesList.Add(verticesList[j]);
                        break;
                    }
                }
            }
            return bestRouteVerticesList;
        }
        #endregion

        #region Choose the Best Route Among Shortest Paths

        private String BestRoute(Graph graph)
        {
            int[] totalDistance = new int[2000];
            for (int i = 0; i < graph.AllNodes.Count; i++)
            {
                if (resultIndexVertexMapping[i].AggregateCost != int.MaxValue)
                {
                    totalDistance[i] = resultIndexVertexMapping[i].AggregateCost;
                }
                else
                {
                    Console.WriteLine("You cannot have Infinity distances. It became infinity means that " +
                        "edge has no right to be in the graph. hence it have no nearby locations.");
                }


            }

            List<string> bestCoveragePaths = BestRouteByCoverageAndDistance();

            int maxQtyIndex = BestRouteByTrashQuantity(bestCoveragePaths);
            var finalizedBestPath = bestCoveragePaths[maxQtyIndex];
            return finalizedBestPath;
        }

        private int BestRouteByTrashQuantity(List<string> bestCoveragePaths)
        {
            // Filter the best route by considering trash quantity
            List<double> totalQtyList = new List<double>();
            for (int i = 0; i < bestCoveragePaths.Count; i++)
            {
                string[] modifiedPath = bestCoveragePaths[i].Replace(" -> ", ",").ToString().Split(',');
                List<int> bestCoveragePathsVerticesIDList = new List<int>();

                for (int j = 0; j < modifiedPath.Length; j++)
                {
                    bestCoveragePathsVerticesIDList.Add(Convert.ToInt32(modifiedPath[j]));
                }

                List<Vector2D> bestCoveragePathsVerticesList = new List<Vector2D>();

                // from the vertices list select these values only 
                for (int j = 0; j < bestCoveragePathsVerticesIDList.Count; j++)
                {
                    for (int k = 0; k < verticesList.Count; k++)
                    {
                        if (bestCoveragePathsVerticesIDList[j] == verticesList[k].VertexID)
                        {
                            bestCoveragePathsVerticesList.Add(verticesList[k]);
                            break;
                        }
                    }
                }

                //calculate the quantity along with that coverage path
                double totQty = 0.0;
                for (int j = 0; j < bestCoveragePathsVerticesList.Count; j++)
                {
                    totQty = totQty + bestCoveragePathsVerticesList[j].TrashQuantity;
                }

                totalQtyList.Add(totQty);

            }

            var maxQty = totalQtyList.ToArray().Max();
            var maxQtyIndex = totalQtyList.IndexOf(maxQty);
            return maxQtyIndex;
        }

        private List<string> BestRouteByCoverageAndDistance()
        {
            //for best path coverage 
            List<int> allPathCounts = new List<int>();
            for (int i = 0; i < LowestCostPathList.Count; i++)
            {
                allPathCounts.Add(LowestCostPathList[i].ToString().Replace(" -> ", ",").ToString().Split(',').Count());
            }

            //get all possible paths with max count
            List<int> maxPathsIndexes = new List<int>();
            var maxCount = allPathCounts.ToArray().Max();
            for (int i = 0; i < allPathCounts.Count; i++)
            {
                if (allPathCounts[i] == maxCount)
                {
                    maxPathsIndexes.Add(i);
                }
            }

            //get aggregate costs of the all max paths
            List<int> AggregateMaxPaths = new List<int>();
            for (int i = 0; i < maxPathsIndexes.Count; i++)
            {
                //if (resultIndexVertexMapping[i].AggregateCost != int.MaxValue)
                //{
                AggregateMaxPaths.Add(resultIndexVertexMapping[maxPathsIndexes[i]].AggregateCost);
                //}
                //else
                //{
                //    Console.WriteLine("You cannot have Infinity distances. It became infinity means that " +
                //        "edge has no right to be in the graph. hence it have no nearby locations.");
                //}
            }

            //get the best coverage path with best max distance
            int CoverageBestMinDistance = AggregateMaxPaths.ToArray().Min();
            routeDistance = CoverageBestMinDistance.ToString();

            List<int> intermediateIndexes = new List<int>();
            for (int i = 0; i < AggregateMaxPaths.Count; i++)
            {
                if (AggregateMaxPaths[i] == CoverageBestMinDistance)
                {
                    intermediateIndexes.Add(i);
                }
            }

            List<int> minDistIndexes = new List<int>();
            for (int i = 0; i < intermediateIndexes.Count; i++)
            {
                minDistIndexes.Add(maxPathsIndexes[intermediateIndexes[i]]);
            }

            List<String> bestCoveragePaths = new List<String>();
            for (int i = 0; i < minDistIndexes.Count; i++)
            {
                bestCoveragePaths.Add(LowestCostPathList[minDistIndexes[i]].ToString());
            }

            return bestCoveragePaths;
        }

        #endregion

        #region Arrange the Final Customers Detail Lists

        public void FinalCustomerDetailsLists(List<Vector2D> qualifiedVerticesList, out string[] newLatList, out string[] newLngList, out string[] newAddressList, out string[] newCnameList, out string[] newBranchList, out string[] newTotQtyList, out string finalBestPath)
        {
            newLatList = new String[qualifiedVerticesList.Count];
            newLngList = new String[qualifiedVerticesList.Count];
            newAddressList = new String[qualifiedVerticesList.Count];
            newCnameList = new String[qualifiedVerticesList.Count];
            newBranchList = new String[qualifiedVerticesList.Count];
            newTotQtyList = new String[qualifiedVerticesList.Count];
            finalBestPath = "Best Route  :   " + qualifiedVerticesList[0].CompanyName;
            for (int i = 0; i < qualifiedVerticesList.Count; i++)
            {
                newLatList[i] = qualifiedVerticesList[i].Lat;
                newLngList[i] = qualifiedVerticesList[i].Lng;
                newAddressList[i] = qualifiedVerticesList[i].Address;
                newCnameList[i] = qualifiedVerticesList[i].CompanyName;
                newBranchList[i] = qualifiedVerticesList[i].Branch;
                newTotQtyList[i] = qualifiedVerticesList[i].TrashQuantity.ToString();
                if (i != 0)
                {
                    finalBestPath = finalBestPath + "  - >  " + qualifiedVerticesList[i].CompanyName + " ( " + qualifiedVerticesList[i].Branch + " ) ";
                }
            }
        }

        #endregion






    }
}