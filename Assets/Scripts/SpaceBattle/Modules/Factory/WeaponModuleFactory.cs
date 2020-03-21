using System.Collections.Generic;
using SpaceBattle.Enums;
using SpaceBattle.SpaceShips;
using SpaceBattle.Utils;
using UnityEditor;
using UnityEngine;

namespace SpaceBattle.Modules.Factory
{
    public class WeaponModuleFactory : ScriptableObject, IShipModuleFactory
    {
        [SerializeField] private string _moduleName;
        
        [SerializeField] private float _coolDown;

        [SerializeField] private float _damage;

        [SerializeField] private SlotType _slotType;
        
        [SerializeField] private GameObject _model;
        

        private readonly Queue<IShipModule> _pool = new Queue<IShipModule>();

        public IShipModule GetOrCreateModule()
        {
            return _pool.Count > 0 ? _pool.Dequeue() : new Weapon(this, _coolDown, _damage, _slotType, _model){ModuleName = _moduleName};
        }

        public void ReleaseModule(IShipModule module)
        {
            if (!module.IsValid) return;

            _pool.Enqueue(module);
        }

        [MenuItem("Assets/Modules/WeaponModuleFactory")]
        public static void CreateAsset()
        {
            ScriptableObjectUtility.CreateAsset<WeaponModuleFactory>();
        }

        public class Weapon : BaseShipModule
        {
            private float _damage;
            private float _lastShootTime;
            private readonly float _coolDown;
            private BaseSpaceShip _ship;
            
            public bool IsShootReady => Time.time - _lastShootTime >= _coolDown - _coolDown*_ship.CoolDownFactorPercent;
            
            public Weapon(WeaponModuleFactory factory, float coolDown, float damage, SlotType slotType, GameObject model)
            {
                _factory = factory;
                _coolDown = coolDown;
                _damage = damage;
                _slotType = slotType;
                _model = model;
            }

            public override void OnAttachedToShip(BaseSpaceShip ship, Slot slot)
            {
                _ship = ship;
                AttachModuleToSlot(slot.TransformPlace);
                ship.AddWeapon(this);
            }

            public override void OnRemovedFromShip(BaseSpaceShip ship)
            {
                ship.RemoveWeapon(this);
            }

            public void Shoot()
            {
                _lastShootTime = Time.time;
                Debug.Log("Weapon damage: "+_damage);
            }
        }
    }
}