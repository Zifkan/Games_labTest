using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SpaceBattle
{
    public class BattleController : MonoBehaviour
    {
        [SerializeField]
        private List<Transform> _battlePoints;

        private void Start()
        {
            var ships = GameController.Instance.Ships;

            var shipsCount = ships.Count;

            if (shipsCount >= _battlePoints.Count)
            {
                Debug.LogError("There are not enough spawn Points");
                return;
            }

            for (int i = 0; i < shipsCount; i++)
            {
                var ship = ships[i];

                var index = Random.Range(0, _battlePoints.Count);

                var point = _battlePoints[index];

                ship.transform.position = point.position;

                _battlePoints.Remove(point);
                
                ship.Fight();
            }
        }

        private void Update()
        {
            
        }
    }
}