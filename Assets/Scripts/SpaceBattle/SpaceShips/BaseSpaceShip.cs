using System;
using System.Collections.Generic;
using System.Linq;
using SpaceBattle.Enums;
using SpaceBattle.Modules;
using UnityEngine;

namespace SpaceBattle.SpaceShips
{
    public class BaseSpaceShip : MonoBehaviour
    {
        [SerializeField]
        private float _healthBase;
        
        [SerializeField]
        private float _shieldBase;
        
        [SerializeField]
        private float _shieldRestorePerSecBase = 1f;

        [SerializeField] 
        private List<Slot> _slots;
        
        private readonly Dictionary<SlotType,List<Slot>> _slotsCollection = new Dictionary<SlotType, List<Slot>>();
        
        private float _health;
        private float _shield;
        private float _shieldRestorePerSec;
        
        public Dictionary<SlotType, List<Slot>> SlotsCollection => _slotsCollection;
        
        
        public void SetHealth(float value)
        {
            _health += value;
        }
        
        public void SetShield(float value)
        {
            _shield += value;
        }
        
        public void SetShieldRestorePerSec(float value)
        {
            _shieldRestorePerSec += value;
        }

        public bool AddModule(IShipModule module)
        {
            var slots = _slotsCollection[module.SlotType];

            var freeSlot = slots.FirstOrDefault(slot => slot.IsFree && (slot.Type & module.SlotType) != 0);

            if (freeSlot != null)
            {
                freeSlot.SetModule(module);
                module.OnAttachedToShip(this,freeSlot);
                return true;
            }

            return false;
        }

        public void RemoveModule(IShipModule module)
        {
            module.OnRemovedFromShip(this);
        }

        void Awake()
        {
            SlotsInit();
            
            _health = _healthBase;
            _shield = _shieldBase;
            _shieldRestorePerSec = _shieldRestorePerSecBase;
        }

        private void SlotsInit()
        {
            var slotTypeList = (SlotType[]) Enum.GetValues(typeof(SlotType));
            
            for (int i = 0; i < _slots.Count; i++)
            {
                var slot = _slots[i];

                for (int j = 0; j < slotTypeList.Length; j++)
                {
                    var slotType = slotTypeList[j];
                    
                    if ((slot.Type & slotType) != 0)
                    {
                        if (!_slotsCollection.ContainsKey(slotType))
                        {
                            var list = new List<Slot> { slot };
                            _slotsCollection.Add(slotType,list);
                        }
                        else
                        {
                            _slotsCollection[slotType].Add(slot);
                        }
                    }
                }
            }
        }
    }
}