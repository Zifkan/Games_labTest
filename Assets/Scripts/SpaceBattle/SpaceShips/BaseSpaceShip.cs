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

        private GameStage _gameStage;

        private float _maxHealth;
        private float _maxShield;
        
        private float _currentHealth;
        private float _currentShield;
        private float _currentShieldRestorePerSec;
        private float _coolDownFactor;
        private float _shieldRestoreFactorPercent;
        
        private readonly Dictionary<SlotType,List<Slot>> _slotsCollection = new Dictionary<SlotType, List<Slot>>();
        private List<WeaponModuleFactory.Weapon> _weapons = new List<WeaponModuleFactory.Weapon>();
        
        public float CoolDownFactorPercent => _coolDownFactor;
        public List<Slot> SlotsCollection => _slots;

        public void SetHealth(float value)
        {
            _maxHealth+=value;
            Debug.Log($"{name}: _maxHealth = "+_maxHealth);
        }
        
        public void SetShield(float value)
        {
            _maxShield += value;
        }

        public void SetReloadFactor(float value)
        {
            _coolDownFactor += value;
        }
        
        public void SetShieldRestoreFactor(float value)
        {
            _shieldRestoreFactorPercent += value;
        }
        
        public void AddModule(IShipModule module)
        {
            var slots = _slotsCollection[module.SlotType];

            var freeSlot = slots.FirstOrDefault(slot => slot.IsFree && (slot.Type & module.SlotType) != 0);

            if (freeSlot != null)
            {
                freeSlot.SetModule(module);
                module.OnAttachedToShip(this,freeSlot);
            }
        }

        public void RemoveModule(Slot slot)
        {
            slot.Module.OnRemovedFromShip(this);
            slot.SetModule(null);
        }

        public void AddWeapon(WeaponModuleFactory.Weapon weapon)
        {
            _weapons.Add(weapon);
        }

        public void RemoveWeapon(WeaponModuleFactory.Weapon weapon)
        {
            _weapons.Remove(weapon);
        }

        public void Fight()
        {
            _gameStage = GameStage.Battle;
        }

        public void Reset()
        {
            _maxHealth = _healthBase;
            _maxShield = _shieldBase;
            _currentShieldRestorePerSec = _shieldRestorePerSecBase;
            
            _slots.ForEach(slot =>
            {
                if (!slot.IsFree)
                    slot.Module.OnAttachedToShip(this, slot);
            });
            
            
            _currentHealth = _maxHealth;
            _currentShield = _maxShield;


            Debug.Log($"{name}: Health = {_currentHealth}; Shield = { _currentShield}; ShieldResore = {_currentShieldRestorePerSec}; " +
                      $"ShieldRestoreFactor = {_shieldRestoreFactorPercent}; RealodFactor = {_coolDownFactor}");
        }

        private void Awake()
        {
            _gameStage = GameStage.ShipConstruct;
            SlotsInit();
            Reset();
        }

        private void Update()
        {
            if (_gameStage == GameStage.ShipConstruct) return;
            
            ShieldRestore();
            Fire();

            Death();
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

        private void ShieldRestore()
        {
            var valPerTick = _currentShieldRestorePerSec * Time.deltaTime;
            _currentShield = Mathf.Clamp(_currentShield + valPerTick + (valPerTick * _shieldRestoreFactorPercent), 0, _maxShield);
        }

        private void Death()
        {
            if (_currentHealth <= 0)
            {
                Debug.Log("Death");
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