using System;
using System.Collections.Generic;
using SpaceBattle.SpaceShips;
using UnityEngine;

namespace SpaceBattle.Utils
{
    public class ShipPlacer : MonoBehaviour
    {
        [SerializeField] 
        private GameObject _shipsContainer;
        
        [SerializeField]
        private Vector2 _startPos = new Vector2(-4,0);

        [SerializeField] 
        private int _columnCount = 5;

        [SerializeField]
        private Vector2 _offset = new Vector2(2f,2f);
        
        private int _rowCount;
       
        public void PlaceShips(List<BaseSpaceShip> ships)
        {
            var objectsCount = ships.Count;
            
            _rowCount = objectsCount / _columnCount + objectsCount % _columnCount;

            var gridCount = _columnCount * _rowCount;
            
            for (int i = 0; i < gridCount ; i++)
            {
                if (i >= objectsCount) return;
                
                var tr = ships[i].transform;
                tr.SetParent(_shipsContainer.transform);
                tr.position =  new Vector3(_startPos.x + (_offset.x * (i % _columnCount)), _startPos.y + (-_offset.y * (i / _rowCount)),0);
            }
        }
    }
}