using System.Collections.Generic;
using System.Linq;
using SpaceBattle.CustomEventArgs;
using SpaceBattle.Modules.Factory;
using SpaceBattle.UI;
using SpaceBattle.Utils;
using UnityEngine;

namespace SpaceBattle.SpaceShips
{
    public class ShipConstructor : MonoBehaviour
    {
        [SerializeField] 
        private BaseSpaceShip _currentShip;

        [SerializeField] 
        private SelectShipMenu _selectShipMenu;

        [SerializeField] 
        private ShipPlacer _shipPlacer;
        
        private readonly List<BaseSpaceShip> _ships = new List<BaseSpaceShip>();
        private List<IShipModuleFactory> _availableModules;
        
        public BaseSpaceShip CurrentShip => _currentShip;
        
        private IShipMenu _shipMenu;
        
        private void Awake()
        {
            _shipMenu = _selectShipMenu;
            _availableModules = Resources.LoadAll<ScriptableObject>("SpaceBattle/Modules/").Select(o => (IShipModuleFactory)o).ToList();
            
            SetShips();
        }

        private void SetShips()
        {
            var shipResources = Resources.LoadAll<BaseSpaceShip>("SpaceBattle/Ships/");

            for (int i = 0; i < shipResources.Length; i++)
            {
                _ships.Add(Instantiate(shipResources[i],new Vector3(0,0,0) , Quaternion.identity));
            }
            _shipPlacer.PlaceShips(_ships);
        }

        private void Start()
        {
            _shipMenu.SetModulesCollection(_availableModules);
            
            _shipMenu.SetShipButtons(_ships);
        }

        private void OnEnable()
        {
            _shipMenu.SetModuleEvent += OnSetModule;
            _shipMenu.DetachModuleEvent += OnDetachModule;
            _shipMenu.SelectShipEvent += OnSelectShip;
        }

        private void OnSelectShip(object sender, int index)
        {
            _currentShip = _ships[index];
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