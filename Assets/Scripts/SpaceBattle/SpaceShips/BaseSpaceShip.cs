using System.Collections.Generic;
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
        private int _modulesCount;
        
        [SerializeField] 
        private int _weponsCount;
        
        private List<IShipModule> _weapons = new List<IShipModule>();
        
        private List<IShipModule> _modules = new List<IShipModule>();

        void Awake()
        {
            _health = _healthBase;
            _shield = _shieldBase;
            _shieldRestorePerSec = _shieldRestorePerSecBase;
        }

        private float _health;
        private float _shield;
        private float _shieldRestorePerSec;

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
            if (_modules.Count < _modulesCount)
            {
                _modules.Add(module);
                module.OnAttachedToShip(this);
                return true;
            }
            
            return false;
        }
        
        public bool AddWeapon(IShipModule weapon)
        {
            if (_weapons.Count < _modulesCount)
            {
                _weapons.Add(weapon);
                weapon.OnAttachedToShip(this);
                return true;
            }
            
            return false;
        }

        public void RemoveModule(IShipModule module)
        {
            _modules.Remove(module);
            module.OnRemovedFromShip(this);
        }


        private void Start()
        {
            
        }

        private void InitModules()
        {
            for (int i = 0; i < _modules.Count; i++)
            {
                var module = _modules[i];
                
                module.OnAttachedToShip(this);
            }
        }
    }
}