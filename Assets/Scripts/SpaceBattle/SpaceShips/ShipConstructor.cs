using System.Collections.Generic;
using System.Linq;
using SpaceBattle.CustomEventArgs;
using SpaceBattle.Modules.Factory;
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
        
        
        private List<IShipModuleFactory> _availableModules;
        
        public BaseSpaceShip CurrentShip => _currentShip;
        
        private IShipMenu _shipMenu;
        
        private void Awake()
        {
            _shipMenu = _selectShipMenu;
            _availableModules = Resources.LoadAll<ScriptableObject>("SpaceBattle/Modules/").Select(o => (IShipModuleFactory)o).ToList();
           // _availableModules[0].GetOrCreateModule()
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
            CurrentShip.RemoveModule(e.Slot.Module);
        }

        private void OnSetModule(object sender, ButtonModuleEventArgs e)
        { 
            CurrentShip.AddModule( e.ModuleFactory.GetOrCreateModule());

            RefreshSlots();
        }

        private void RefreshSlots()
        {
            _shipMenu.SetSlotsCollection(CurrentShip.SlotsCollection.Values.SelectMany(list => list.Where(slot => !slot.IsFree)).ToList());
        }
    }
}