using System.Collections.Generic;

namespace Metro
{
    public class StationInfo
    {
        public Station Station { get; }
        public bool IsUnvisited { get; set; }
        public int EdgesWeightSum { get; set; }
        public Station PreviousStation { get; set; }
     
        public StationInfo(Station station)
        {
            Station = station;
            IsUnvisited = true;
            EdgesWeightSum = int.MaxValue;
            PreviousStation = null;
        }
    }
    
    public struct Path
    {
        public Station ConnectedVertex { get; }
        public int EdgeWeight { get; }
        public Path(Station connectedVertex, int weight)
        {
            ConnectedVertex = connectedVertex;
            EdgeWeight = weight;
        }
    }
    
    public class Station
    {
        public string Name { get; }
        public List<Path> Paths { get; }

        public MetroBranchType Branch{ get; }
        
        public Station(string name, MetroBranchType branch)
        {
            Name = name;
            Branch = branch;
            Paths = new List<Path>();
        }
        
        public bool IsPart(MetroBranchType t)
        {
            return (Branch & t) == Branch;
        }
       
        public void AddEdge(Station vertex, int edgeWeight)
        {
            Paths.Add(new Path(vertex, edgeWeight));
        }

        public override string ToString() => Name;
    }
}