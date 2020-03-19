using System.Collections.Generic;

namespace Metro
{
    public class StationInfo
    {
        public Station Vertex { get; set; }
        public bool IsUnvisited { get; set; }
        public int EdgesWeightSum { get; set; }
        public Station PreviousVertex { get; set; }
     
        public StationInfo(Station vertex)
        {
            Vertex = vertex;
            IsUnvisited = true;
            EdgesWeightSum = int.MaxValue;
            PreviousVertex = null;
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

        public Station(string vertexName)
        {
            Name = vertexName;
            Paths = new List<Path>();
        }

        public void AddEdge(Path newEdge)
        {
            Paths.Add(newEdge);
        }

        public void AddEdge(Station vertex, int edgeWeight)
        {
            AddEdge(new Path(vertex, edgeWeight));
        }

        public override string ToString() => Name;
    }
}