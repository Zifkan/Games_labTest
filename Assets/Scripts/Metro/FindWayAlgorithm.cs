using System.Collections.Generic;

namespace Metro
{
    public class WayFinder
    {
        readonly Graph _graph;

        readonly Dictionary<Station,StationInfo> _infos = new Dictionary<Station,StationInfo>();
    
        public WayFinder(Graph graph)
        {
            _graph = graph;
        }

        void InitInfo()
        {
            foreach (var v in _graph.Stations)
            {
                _infos.Add(v.Value,new StationInfo(v.Value));
            }
        }

    
        StationInfo GetVertexInfo(Station v)
        {
            return _infos.ContainsKey(v) ? _infos[v] : null;
        }

        private StationInfo FindUnvisitedVertexWithMinSum()
        {
            var minValue = int.MaxValue;
            StationInfo minVertexInfo = null;
            
            foreach (var stationInfo in _infos.Values)
            {
                if (stationInfo.IsUnvisited && stationInfo.EdgesWeightSum < minValue)
                {
                    minVertexInfo = stationInfo;
                    minValue = stationInfo.EdgesWeightSum;
                }
            }

            return minVertexInfo;
        }

        
        public string FindShortestPath(string startName, string finishName)
        {
            return FindShortestPath(_graph.FindStation(startName), _graph.FindStation(finishName));
        }

     
        private string FindShortestPath(Station start, Station finishVertex)
        {
            InitInfo();
            var first = GetVertexInfo(start);
            first.EdgesWeightSum = 0;
            while (true)
            {
                var current = FindUnvisitedVertexWithMinSum();
                if (current == null)
                {
                    break;
                }

                SetSumToNextVertex(current);
            }

            return GetPath(start, finishVertex);
        }

     
        void SetSumToNextVertex(StationInfo info)
        {
            info.IsUnvisited = false;
            foreach (var path in info.Station.Paths)
            {
                var nextInfo = GetVertexInfo(path.ConnectedVertex);
                var sum = info.EdgesWeightSum + path.EdgeWeight;
                if (sum < nextInfo.EdgesWeightSum)
                {
                    nextInfo.EdgesWeightSum = sum;
                    nextInfo.PreviousStation = info.Station;
                }
            }
        }

       
        string GetPath(Station start, Station end)
        {
            var path = end.ToString();

            var transferCount = 0;
            
            while (start != end)
            {
                var previous = GetVertexInfo(end).PreviousStation;
                
                if (previous.IsPart(end.Branch))
                {
                    transferCount++;
                }

                end = previous;
                path = end + path;
            }

            return path + "; Transfer count: "+transferCount;
        }
    }
}
