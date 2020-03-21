using System;
using System.Collections.Generic;
using System.Linq;
using SpaceBattle.CustomEventArgs;
using SpaceBattle.Modules.Factory;
using SpaceBattle.UI;
using SpaceBattle.Utils;
using UnityEngine;

namespace SpaceBattle.SpaceShips
{
    public class ShipConstructor : IDisposable
    {
        private List<BaseSpaceShip> _ships = new List<BaseSpaceShip>();
        private List<IShipModuleFactory> _availableModules;

        private BaseSpaceShip _currentShip;
        
        private IShipMenu _shipMenu;

        public List<BaseSpaceShip> Ships => _ships;
        
        public void Init(List<BaseSpaceShip> ships, IShipMenu shipMenu , ShipPlacer placer)
        {
            _ships = ships;
            _shipMenu = shipMenu;
            _availableModules = Resources.LoadAll<ScriptableObject>("SpaceBattle/Modules/").Select(o => (IShipModuleFactory)o).ToList();
            placer.PlaceShips(_ships);
            
            _shipMenu.SetModulesCollection(_availableModules);
            _shipMenu.SetShipButtons(_ships);
            SetCurrentShip(0);

            EventSubscription();
        }

        private void EventSubscription()
        {
            _shipMenu.SetModuleEvent += OnSetModule;
            _shipMenu.DetachModuleEvent += OnDetachModule;
            _shipMenu.SelectShipEvent += OnSelectShip;
        }

        private void OnSelectShip(object sender, int index)
        {
            SetCurrentShip(index);
        }

        private void SetCurrentShip( int index)
        {
            _currentShip = _ships[index];
            RefreshSlots();
        }

        private void OnDetachModule(object sender, ButtonSlotEventArgs e)
        {
            _currentShip.RemoveModule(e.Slot);
            RefreshSlots();
        }

        private void OnSetModule(object sender, ButtonModuleEventArgs e)
        { 
            _currentShip.AddModule(e.ModuleFactory.GetOrCreateModule());
            RefreshSlots();
        }

        private void RefreshSlots()
        {
            _shipMenu.Refresh(_currentShip.SlotsCollection);
        }

        public void Dispose()
        {
            _shipMenu.SetModuleEvent -= OnSetModule;
            _shipMenu.DetachModuleEvent -= OnDetachModule;
        }
    }
}