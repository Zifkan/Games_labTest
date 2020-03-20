using System.Collections.Generic;
using System.Linq;
using SpaceBattle.CustomEventArgs;
using SpaceBattle.Modules;
using SpaceBattle.UI;
using UnityEngine;

namespace SpaceBattle.SpaceShips
{
    public class ShipConstructor : MonoBehaviour
    {
        [SerializeField] 
        private BaseSpaceShip _currentShip;

        [SerializeField] 
        private SelectShipMenu _selectShipMenu;
        
        
        private List<IShipModule> _availableModules;
        
        public BaseSpaceShip CurrentShip => _currentShip;
        
        private IShipMenu _shipMenu;
        
        private void Awake()
        {
            _shipMenu = _selectShipMenu;
            
            _availableModules = Resources.LoadAll<BaseShipModule>("SpaceBattle/Modules/").Select(module => (IShipModule)module).ToList();
            
         
        }

        private void Start()
        {
            _shipMenu.SetModulesCollection(_availableModules);
        }

        private void OnEnable()
        {
            _shipMenu.SetModuleEvent+= OnSetModule;
            _shipMenu.DetachModuleEvent+=OnDetachModule;
        }
        
        private void OnDisable()
        {
            _shipMenu.SetModuleEvent -= OnSetModule;
            _shipMenu.DetachModuleEvent -=OnDetachModule;
        }

        private void OnDetachModule(object sender, ButtonSlotEventArgs e)
        {
            
        }

        private void OnSetModule(object sender, ButtonModuleEventArgs e)
        {
            var instance = Instantiate((BaseShipModule)e.Module);
            
            CurrentShip.AddModule(instance);
        }

      
        public void  DetachModule(int index)
        {
            //    CurrentShip.RemoveModule((IShipModule)module);
        }
    }
}