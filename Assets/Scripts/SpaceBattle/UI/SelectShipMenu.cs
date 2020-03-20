using System;
using System.Collections.Generic;
using SpaceBattle.CustomEventArgs;
using SpaceBattle.Modules;
using SpaceBattle.SpaceShips;
using SpaceBattle.Utils;
using UnityEngine;

namespace SpaceBattle.UI
{
    public class SelectShipMenu : MonoBehaviour,IShipMenu
    {
        public event EventHandler<ButtonModuleEventArgs> SetModuleEvent;
        public event EventHandler<ButtonSlotEventArgs> DetachModuleEvent;

        [SerializeField] 
        private CustomButton _button;

        [SerializeField] 
        private GameObject _modulesPanelCatalog;
        [SerializeField] 
        private GameObject _usedModulesPanel;
  
        private ShipConstructor _shipConstructor;
        private ObjectPool<CustomButton> _buttonsPool;

        public void SetModulesCollection(List<IShipModule> modules)
        {
            for (int i = 0; i < modules.Count; i++)
            {
                var btn = _buttonsPool.Get();
                btn.transform.SetParent(_modulesPanelCatalog.transform);
                var module = modules[i];
                btn.Init(modules[i].SlotType.ToString(), () => OnModuleButtonPressed(module));
            }
        }
        
        
        private void Awake()
        {
            _shipConstructor = GetComponent<ShipConstructor>();
            _buttonsPool = new ObjectPool<CustomButton>(_button, 10, transform);
        }

        private void Start()
        {
          
        }

        private void OnModuleButtonPressed(IShipModule module)
        {
            OnSetModule(module);
        }

        private void OnModuleDetachPressed(int index)
        {
            
        }


        protected virtual void OnSetModule(IShipModule module)
        {
            SetModuleEvent?.Invoke(this, new ButtonModuleEventArgs(module));
        }

        protected virtual void OnDetachModule(Slot slot)
        {
            DetachModuleEvent?.Invoke(this, new ButtonSlotEventArgs(slot));
        }

    }
}