using System.Collections.Generic;

namespace Metro
{
    public class Graph
    {
        private readonly Dictionary<string,Station> _stations = new Dictionary<string,Station>();

        public Dictionary<string,Station> Stations => _stations;

        public void AddStation(string stationName, MetroBranchType branch)
        {
            Stations.Add(stationName,new Station(stationName, branch));
        }

        public Station FindStation(string name)
        {
            return Stations.ContainsKey(name) ? Stations[name] : null;
        }

        public void AddEdge(string firstName, string secondName, int weight)
        {
            var v1 = FindStation(firstName);
            var v2 = FindStation(secondName);
            if (v2 != null && v1 != null)
            {
                v1.AddEdge(v2, weight);
                v2.AddEdge(v1, weight);
            }
        }
    }
}