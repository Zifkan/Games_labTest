using System;
using System.Collections.Generic;
using SpaceBattle.Modules;
using SpaceBattle.SpaceShips;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SpaceBattle
{
    public class BattleController : MonoBehaviour
    {
        [SerializeField]
        private List<Transform> _battlePoints;

        private List<BaseSpaceShip> _ships;

        private void Start()
        {
            _ships = GameController.Instance.Ships;

            var shipsCount = _ships.Count;

            if (shipsCount >= _battlePoints.Count)
            {
                Debug.LogError("There are not enough spawn Points");
                return;
            }

            for (int i = 0; i < shipsCount; i++)
            {
                var ship = _ships[i];
                var index = Random.Range(0, _battlePoints.Count);
                var point = _battlePoints[index];

                ship.transform.position = point.position;
                _battlePoints.Remove(point);
                ship.DeadEvent+=OnDead;
                ship.Fight();
              
            }
        }

        private void Update()
        {
            for (int i = 0; i < _ships.Count; i++)
            {
                var ship = _ships[i];
                ShipLookAt(ship, i);
            }
        }

        private void OnDestroy()
        {
            _ships.ForEach(ship => ship.DeadEvent-=OnDead);
        }

        private void OnDead(object sender, EventArgs e)
        {
            _ships.Remove((BaseSpaceShip) sender);
            
            if (_ships.Count <= 1)
            {
                GameController.Instance.EndFight();
            }
        }

        private void ShipLookAt(BaseSpaceShip ship,int index)
        {
            var target = index+1 < _ships.Count ? _ships[index+1] : _ships[0];
            
            ship.transform.LookAt(target.transform, ship.transform.up);
        }
    }
}