using System;
using System.Collections.Generic;
using SpaceBattle.CustomEventArgs;
using SpaceBattle.Modules.Factory;
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

        public void SetModulesCollection(List<IShipModuleFactory> modules)
        {
            for (int i = 0; i < modules.Count; i++)
            {
                var btn = _buttonsPool.Get();
                btn.transform.SetParent(_modulesPanelCatalog.transform);
                var module = modules[i];
                btn.Init(modules[i].ToString(), () => OnModuleButtonPressed(module));
            }
        }

        public void SetSlotsCollection(List<Slot> slots)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                var btn = _buttonsPool.Get();
                btn.transform.SetParent(_usedModulesPanel.transform);
                var slot = slots[i];
                btn.Init(slot.ToString(), () => OnModuleDetachPressed(slot));
            }
        }


        private void Awake()
        {
            _shipConstructor = GetComponent<ShipConstructor>();
            _buttonsPool = new ObjectPool<CustomButton>(_button, 10, transform);
        }

        private void OnModuleButtonPressed(IShipModuleFactory moduleFactory)
        {
            OnSetModule(moduleFactory);
        }

        private void OnModuleDetachPressed(Slot slot)
        {
            OnDetachModule(slot);
        }


        protected virtual void OnSetModule(IShipModuleFactory moduleFactory)
        {
            SetModuleEvent?.Invoke(this, new ButtonModuleEventArgs(moduleFactory));
        }

        protected virtual void OnDetachModule(Slot slot)
        {
            DetachModuleEvent?.Invoke(this, new ButtonSlotEventArgs(slot));
        }

    }
}