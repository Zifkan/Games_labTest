using System.Collections.Generic;
using System.Linq;
using SpaceBattle.Modules;
using UnityEngine;
using Object = System.Object;

namespace SpaceBattle.SpaceShips
{
    public class ShipConstructor : MonoBehaviour
    {
        [SerializeField] 
        private BaseSpaceShip _currentShip;

        private List<ShipModule> _availableModules;
        
        private List<ShipModule> _setupModules;
        
        public BaseSpaceShip CurrentShip => _currentShip;

        public List<ShipModule> AvailableModules => _availableModules;

        private void Awake()
        {
            _availableModules = Resources.LoadAll<ShipModule>("SpaceBattle/Modules/").OrderByDescending(module => module).ToList();
        }

        public bool  SetupModule(int index)
        {
            var instance = Instantiate(AvailableModules[index]);
            
            var isModuleSet = CurrentShip.AddModule((IShipModule)instance);

            //TODO: May be check for available count 

            return isModuleSet;
        }

        public List<IShipModule> GetSetupModules()
        {
            return _currentShip.UsedModules.ToList();
        }
    }
}