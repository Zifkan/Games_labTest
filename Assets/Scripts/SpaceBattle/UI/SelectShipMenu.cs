using SpaceBattle.SpaceShips;
using SpaceBattle.Utils;
using UnityEngine;

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
               btn.Init(_shipConstructor.Weapons[i].name, () => OnModuleButtonPressed(index));
           }
        }

        private void OnModuleButtonPressed(int i)
        {
            _shipConstructor.SetupModule(i);
        }
        
        private void OnWeaponButtonPressed(int i)
        {
            _shipConstructor.SetupWeapon(i);
        }
    }
}