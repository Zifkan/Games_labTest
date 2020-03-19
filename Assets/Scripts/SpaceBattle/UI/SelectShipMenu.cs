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
        private GameObject _modulesPanelCatalog;
        [SerializeField] 
        private GameObject _usedModulesPanel;
  
        private ShipConstructor _shipConstructor;
        private ObjectPool<CustomButton> _buttonsPool;

        private void Awake()
        {
            _buttonsPool = new ObjectPool<CustomButton>(_button, 10, transform);
        }

        private void Start()
        {
           _shipConstructor = GetComponent<ShipConstructor>();
           
           for (int i = 0; i < _shipConstructor.AvailableModules.Count; i++)
           {
              var btn = _buttonsPool.Get();
              btn.transform.SetParent(_modulesPanelCatalog.transform);
              var index = i;
              btn.Init(_shipConstructor.AvailableModules[i].name, () => OnModuleButtonPressed(index));
           }
        }

        private void OnModuleButtonPressed(int i)
        {
           var isModuleSet = _shipConstructor.SetupModule(i);

           if (isModuleSet)
           {
               RefreshUsedModules();
           }
        }

        private void RefreshUsedModules()
        {
            var setupModules = _shipConstructor.GetSetupModules();

            for (int i = 0; i < setupModules.Count; i++)
            {
                var btn = _buttonsPool.Get();
                btn.transform.SetParent(_usedModulesPanel.transform);
                var index = i;
                btn.Init(setupModules[i].ToString(), () => OnModuleButtonPressed(index));
            }
        }
    }
}