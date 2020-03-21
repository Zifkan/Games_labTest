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
        public event EventHandler<int> SelectShipEvent;

        [SerializeField] 
        private CustomButton _button;

        [SerializeField] 
        private GameObject _modulesPanelCatalog;
        [SerializeField] 
        private GameObject _usedModulesPanel;
        [SerializeField] 
        private GameObject _shipsPanel;

        private ObjectPool<CustomButton> _buttonsPool;
        private readonly List<CustomButton> _usedSlotsButtons = new List<CustomButton>();
        
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

        public void Refresh(List<Slot> slots)
        {
            for (int i = 0; i <  _usedSlotsButtons.Count; i++)
            {
                var btn = _usedSlotsButtons[i];
                _buttonsPool.ReturnToPool(btn);
            }
            _usedSlotsButtons.Clear();
            
            for (int i = 0; i < slots.Count; i++)
            {
                var btn = _buttonsPool.Get();
                btn.transform.SetParent(_usedModulesPanel.transform);

                string btnName;

                var slot = slots[i];
                if (slot.Module == null)
                {
                    btnName = slot.Type>0?slot.Type.ToString():"Universal";
                }
                else
                {
                    btnName = slot.Module.ModuleName;
                }

                btn.Init(btnName, () => OnModuleDetachPressed(slot));
                _usedSlotsButtons.Add(btn);
            }
        }

        public void SetShipButtons(List<BaseSpaceShip> ships)
        {
            for (int i = 0; i < ships.Count; i++)
            {
                var btn = _buttonsPool.Get();
                btn.transform.SetParent(_shipsPanel.transform);
                var ship = ships[i];
                var index = i;
                btn.Init(ship.ToString(), () => OnSelectShip(index));
            }
        }

        private void Awake()
        {
            _buttonsPool = new ObjectPool<CustomButton>(_button, 10, transform);
        }

        private void OnModuleButtonPressed(IShipModuleFactory moduleFactory)
        {
            OnSetModule(moduleFactory);
        }

        private void OnModuleDetachPressed(Slot slot)
        {
            if(!slot.IsFree)
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

        protected virtual void OnSelectShip(int i)
        {
            SelectShipEvent?.Invoke(this, i);
        }
    }
}