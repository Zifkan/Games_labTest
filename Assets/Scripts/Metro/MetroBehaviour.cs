using UnityEngine;

namespace Metro
{
    public class MetroBehaviour : MonoBehaviour
    {
        private void Start()
        {
            var g = new Graph();
          
            g.AddStation("A",MetroBranchType.Red);
            g.AddStation("B",MetroBranchType.Red | MetroBranchType.Black);
            g.AddStation("C", MetroBranchType.Red | MetroBranchType.Green);
            g.AddStation("D",MetroBranchType.Red | MetroBranchType.Blue);
            g.AddStation("E", MetroBranchType.Red | MetroBranchType.Green);
            g.AddStation("F",MetroBranchType.Red | MetroBranchType.Black);
            g.AddStation("G",MetroBranchType.Black);
            
            g.AddStation("H",MetroBranchType.Black);
            g.AddStation("J",MetroBranchType.Black | MetroBranchType.Blue);
            g.AddStation("K", MetroBranchType.Green);
            g.AddStation("L",MetroBranchType.Green | MetroBranchType.Blue);
            g.AddStation("M", MetroBranchType.Green);
            g.AddStation("N",MetroBranchType.Blue);
            g.AddStation("O",MetroBranchType.Blue);
            

            g.AddEdge("A", "B", 22);
            g.AddEdge("B", "C", 33);
            
            g.AddEdge("C", "D", 11);
            g.AddEdge("C", "K", 1);
            g.AddEdge("C", "J", 63);
            
            g.AddEdge("D", "L", 41);
            g.AddEdge("D", "J", 35);

            g.AddEdge("E", "J", 17);
            g.AddEdge("E", "M", 17);    
            
            
            g.AddEdge("F", "J", 84);
            g.AddEdge("F", "G", 25);
            
            g.AddEdge("L", "N", 5);
            g.AddEdge("K", "L", 10);
            // g.AddEdge("A", "D", 61);
            // g.AddEdge("B", "C", 47);
            // g.AddEdge("B", "E", 93);
            //
            //
            // g.AddEdge("E", "F", 17);
            // g.AddEdge("E", "G", 58);

            var dijkstra = new FindWayAlgorithm(g);
            var path = dijkstra.FindShortestPath("A", "N");
            Debug.Log(path);
        }
    }
}