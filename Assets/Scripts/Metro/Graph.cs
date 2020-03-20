using System.Collections.Generic;

namespace Metro
{
    public class Graph
    {
        public List<Station> Vertices { get; }

        public Graph()
        {
            Vertices = new List<Station>();
        }

        public void AddVertex(string vertexName)
        {
            Vertices.Add(new Station(vertexName));
        }

        public Station FindVertex(string vertexName)
        {
            foreach (var v in Vertices)
            {
                if (v.Name.Equals(vertexName))
                {
                    return v;
                }
            }

            return null;
        }

        public void AddEdge(string firstName, string secondName, int weight)
        {
            var v1 = FindVertex(firstName);
            var v2 = FindVertex(secondName);
            if (v2 != null && v1 != null)
            {
                v1.AddEdge(v2, weight);
                v2.AddEdge(v1, weight);
            }
        }
    }
}