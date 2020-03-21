using System;
using System.Collections.Generic;
using System.Linq;
using SpaceBattle.Enums;
using SpaceBattle.Modules;
using SpaceBattle.Modules.Factory;
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
        
        private float _maxHealth;
        private float _maxShield;
        private float _maxShieldRestorePerSec;
        
        private List<WeaponModuleFactory.Weapon> _weapons = new List<WeaponModuleFactory.Weapon>();
        
        public Dictionary<SlotType, List<Slot>> SlotsCollection => _slotsCollection;
        
        
        public void SetHealth(float value)
        {
            _maxHealth = _healthBase + value;
        }
        
        public void SetShield(float value)
        {
            _maxShield =_shieldBase + value;
        }
        
        public void SetShieldRestorePerSec(float value)
        {
            _maxShieldRestorePerSec =_shieldRestorePerSecBase + value;
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

        public void AddWeapon(WeaponModuleFactory.Weapon weapon)
        {
            _weapons.Add(weapon);
        }

        public void RemoveWeapon(WeaponModuleFactory.Weapon weapon)
        {
            _weapons.Remove(weapon);
        }

        private void Awake()
        {
            SlotsInit();
            
            _maxHealth = _healthBase;
            _maxShield = _shieldBase;
            _maxShieldRestorePerSec = _shieldRestorePerSecBase;
        }

        private void Update()
        {
            Fire();
        }

        private void Fire()
        {
            for (int i = 0; i < _weapons.Count; i++)
            {
                var weapon = _weapons[i];

                if (weapon.IsShootReady)
                {
                    weapon.Shoot();
                }
            }
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