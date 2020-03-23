using System;
using System.Collections.Generic;
using UnityEngine;

namespace Metro
{
    [Serializable]
    struct StationData
    {
        public string Name;
        public MetroBranchType Branch;
    }

    [Serializable]
    struct StationPath
    {
        public string FirstStation;
        public string SecondStation;
        public int Length;
    }
    
    public class MetroBehaviour : MonoBehaviour
    {
        [SerializeField]
        private List<StationData> _stations;

        [SerializeField]
        private List<StationPath> _stationPaths;

        
        [SerializeField]
        private string _startStation;
        
        [SerializeField]
        private string _destinationStation;

        
        private void Start()
        {
            var g = new Graph();

            for (int i = 0; i < _stations.Count; i++)
            {
                var st = _stations[i];
                g.AddStation(st.Name,st.Branch);
            }

            for (int i = 0; i < _stationPaths.Count; i++)
            {
                var pathInfo = _stationPaths[i];
                g.AddPath(pathInfo.FirstStation,pathInfo.SecondStation,pathInfo.Length);
            }
            
           

            var finder = new WayFinder(g);
            var path = finder.FindShortestPath(_startStation, _destinationStation);
            Debug.Log(path);
        }
    }
}