using System.Collections.Generic;
using System.Reflection;
using SpaceBattle.Modules;
using UnityEngine;

namespace SpaceBattle.SpaceShips
{
    public class ShipConstructor : MonoBehaviour
    {
        [SerializeField]
        private List<ShipModule> _weapons;
        
        [SerializeField]
        private List<ShipModule> _modules;
        
        [SerializeField] 
        private BaseSpaceShip _currentShip;

        public List<ShipModule> Modules => _modules;

        public void SetupModule(int index)
        {
            _currentShip.AddModule((IShipModule) Modules[index]);
        }
    }
}