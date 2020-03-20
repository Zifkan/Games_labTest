using System.Collections.Generic;

namespace Metro
{
    public class FindWayAlgorithm
    {
        Graph graph;

        List<StationInfo> infos;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="graph">Граф</param>
        public FindWayAlgorithm(Graph graph)
        {
            this.graph = graph;
        }

        void InitInfo()
        {
            infos = new List<StationInfo>();
            foreach (var v in graph.Vertices)
            {
                infos.Add(new StationInfo(v));
            }
        }

    
        StationInfo GetVertexInfo(Station v)
        {
            foreach (var i in infos)
            {
                if (i.Station.Equals(v))
                {
                    return i;
                }
            }

            return null;
        }

        /// <summary>
        /// Поиск непосещенной вершины с минимальным значением суммы
        /// </summary>
        /// <returns>Информация о вершине</returns>
        public StationInfo FindUnvisitedVertexWithMinSum()
        {
            var minValue = int.MaxValue;
            StationInfo minVertexInfo = null;
            foreach (var i in infos)
            {
                if (i.IsUnvisited && i.EdgesWeightSum < minValue)
                {
                    minVertexInfo = i;
                    minValue = i.EdgesWeightSum;
                }
            }

            return minVertexInfo;
        }

        
        public string FindShortestPath(string startName, string finishName)
        {
            return FindShortestPath(graph.FindVertex(startName), graph.FindVertex(finishName));
        }

     
        public string FindShortestPath(Station startVertex, Station finishVertex)
        {
            InitInfo();
            var first = GetVertexInfo(startVertex);
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

            return GetPath(startVertex, finishVertex);
        }

     
        void SetSumToNextVertex(StationInfo info)
        {
            info.IsUnvisited = false;
            foreach (var e in info.Station.Paths)
            {
                var nextInfo = GetVertexInfo(e.ConnectedVertex);
                var sum = info.EdgesWeightSum + e.EdgeWeight;
                if (sum < nextInfo.EdgesWeightSum)
                {
                    nextInfo.EdgesWeightSum = sum;
                    nextInfo.PreviousVertex = info.Station;
                }
            }
        }

       
        string GetPath(Station startVertex, Station endVertex)
        {
            var path = endVertex.ToString();
            while (startVertex != endVertex)
            {
                endVertex = GetVertexInfo(endVertex).PreviousVertex;
                path = endVertex.ToString() + path;
            }

            return path;
        }
    }
}
