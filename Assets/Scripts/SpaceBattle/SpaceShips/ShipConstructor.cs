using System.Collections.Generic;
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
        public List<ShipModule> Weapons => _weapons;

        public BaseSpaceShip CurrentShip => _currentShip;

        public void SetupModule(int index)
        {
            CurrentShip.AddModule((IShipModule)Modules[index]); 
        }
        
        public void SetupWeapon(int index)
        {
          //  CurrentShip.AddWeapon((IShipModule)Weapons[index]); 
        }
    }
}