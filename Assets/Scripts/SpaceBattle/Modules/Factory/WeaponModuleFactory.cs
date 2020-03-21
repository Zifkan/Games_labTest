using System.Collections.Generic;
using SpaceBattle.SpaceShips;
using SpaceBattle.Utils;
using UnityEditor;
using UnityEngine;

namespace SpaceBattle.Modules.Factory
{
    public class WeaponModuleFactory : ScriptableObject, IShipModuleFactory
    {
        [SerializeField]
        private float _coolDown;

        [SerializeField]
        private float _damage;
        
        private readonly Queue<IShipModule> _pool = new Queue<IShipModule>();
    
        public IShipModule GetOrCreateModule()
        {
            return  _pool.Count > 0 ? _pool.Dequeue() : new Weapon(this,_coolDown,_damage);
        }
        
        public void ReleaseModule(IShipModule module)
        {
            if(!module.IsValid) return;
            
            _pool.Enqueue(module);
        }
        
        [MenuItem("Assets/Modules/WeaponModuleFactory")]
        public static void CreateAsset ()
        {
            ScriptableObjectUtility.CreateAsset<WeaponModuleFactory> ();
        }
    }
    public class Weapon :BaseShipModule
    {
        private float _coolDown;
        private float _damage;

        public Weapon(WeaponModuleFactory factory, float coolDown, float damage)
        {
            _factory = factory;
            _coolDown = coolDown;
            _damage = damage;
        }
        public override void OnAttachedToShip(BaseSpaceShip ship, Slot slot)
        {
           // Debug.Log("Attach "+ name);
        }

        public override void OnRemovedFromShip(BaseSpaceShip ship)
        {
        
        }
    }
}