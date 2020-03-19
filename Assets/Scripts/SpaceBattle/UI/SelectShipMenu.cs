using SpaceBattle.SpaceShips;
using SpaceBattle.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceBattle.UI
{
    public class SelectShipMenu : MonoBehaviour
    {
        [SerializeField] 
        private CustomButton _button;

        [SerializeField] 
        private GameObject _modulesPanel;
        [SerializeField] 
        private GameObject _weaponsPanel;

        [SerializeField] 
        private Text _weaponSlotsCountText;
        [SerializeField] 
        private Text _moduleSlotsCountText;
        
        private ShipConstructor _shipConstructor;
        private ObjectPool<CustomButton> _buttonsPool;

        private void Awake()
        {
            _buttonsPool = new ObjectPool<CustomButton>(_button, 10, transform);
        }

        private void Start()
        {
           _shipConstructor = GetComponent<ShipConstructor>();
           
           for (int i = 0; i < _shipConstructor.Modules.Count; i++)
           {
              var btn = _buttonsPool.Get();
              btn.transform.SetParent(_modulesPanel.transform);
              var index = i;
              btn.Init(_shipConstructor.Modules[i].name, () => OnModuleButtonPressed(index));
           }
           
           for (int i = 0; i < _shipConstructor.Weapons.Count; i++)
           {
               var btn = _buttonsPool.Get();
               btn.transform.SetParent(_weaponsPanel.transform);
               var index = i;
               btn.Init(_shipConstructor.Weapons[i].name, () => OnWeaponButtonPressed(index));
           }
           
           
           // _moduleSlotsCountText.text = _shipConstructor.CurrentShip.FreeModulesSlots().ToString();
           // _weaponSlotsCountText.text = _shipConstructor.CurrentShip.FreeWeaponSlots().ToString();
        }

        private void OnModuleButtonPressed(int i)
        {
            _shipConstructor.SetupModule(i);
           // _moduleSlotsCountText.text = _shipConstructor.CurrentShip.FreeModulesSlots().ToString();
        }
        
        private void OnWeaponButtonPressed(int i)
        {
            _shipConstructor.SetupWeapon(i);
          //  _weaponSlotsCountText.text = _shipConstructor.CurrentShip.FreeWeaponSlots().ToString();
        }
    }
}