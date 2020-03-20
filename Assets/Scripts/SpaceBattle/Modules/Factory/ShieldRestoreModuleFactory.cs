using System.Collections.Generic;
using SpaceBattle.Modules.Factory;
using SpaceBattle.SpaceShips;
using SpaceBattle.Utils;
using UnityEditor;
using UnityEngine;

namespace SpaceBattle.Modules
{
    public class ShieldRestoreModuleFactory : ScriptableObject, IShipModuleFactory
    {
        [SerializeField][Range(0,1)]
        private float _shieldRestorePercent = 0.2f;
        
        private readonly Queue<IShipModule> _pool = new Queue<IShipModule>();
    
        public IShipModule GetOrCreateModule()
        {
            return  _pool.Count > 0 ? _pool.Dequeue() : new ShieldRestoreModule(this,_shieldRestorePercent);
        }
        
        public void ReleaseModule(IShipModule module)
        {
            if(!module.IsValid) return;
            
            _pool.Enqueue(module);
        }
        
        [MenuItem("Assets/Modules/ShieldRestoreModuleFactory")]
        public static void CreateAsset ()
        {
            ScriptableObjectUtility.CreateAsset<ShieldRestoreModuleFactory> ();
        }
    }
    
    public class ShieldRestoreModule :BaseShipModule<ShieldRestoreModuleFactory>
    {
        private float _shieldRestorePercent;

        public ShieldRestoreModule(ShieldRestoreModuleFactory factory, float restoreValue)
        {
            _factory = factory;
            _shieldRestorePercent = restoreValue;
        }

        public override void OnAttachedToShip(BaseSpaceShip ship,Slot slot)
        {
            ship.SetHealth(_shieldRestorePercent);
            AttachModuleToSlot(slot.TransformPlace);
        }    

        public override void OnRemovedFromShip(BaseSpaceShip ship)
        {
            ship.SetHealth(-_shieldRestorePercent);
        }
    }
}