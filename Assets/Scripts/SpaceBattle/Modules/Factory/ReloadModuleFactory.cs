using System.Collections.Generic;
using SpaceBattle.SpaceShips;
using SpaceBattle.Utils;
using UnityEditor;
using UnityEngine;

namespace SpaceBattle.Modules.Factory
{
    public class ReloadModuleFactory : ScriptableObject, IShipModuleFactory
    {
        [SerializeField]
        private float _reloadReduceRatio;
        
        private readonly Queue<IShipModule> _pool = new Queue<IShipModule>();
    
        public IShipModule GetOrCreateModule()
        {
            return  _pool.Count > 0 ? _pool.Dequeue() : new ReloadModule(this,_reloadReduceRatio);
        }
        
        public void ReleaseModule(IShipModule module)
        {
            if(!module.IsValid) return;
            
            _pool.Enqueue(module);
        }
        
        [MenuItem("Assets/Modules/ReloadModuleFactory")]
        public static void CreateAsset ()
        {
            ScriptableObjectUtility.CreateAsset<ReloadModuleFactory> ();
        }
    }
    
    public class ReloadModule : BaseShipModule<ReloadModuleFactory>
    {
        private float _reloadReduceRatio;
        public ReloadModule(ReloadModuleFactory factory, float reloadReduceRatio)
        {
            _factory = factory;
            _reloadReduceRatio = reloadReduceRatio;
        }

        public override void OnAttachedToShip(BaseSpaceShip ship, Slot slot)
        {
           
        }

        public override void OnRemovedFromShip(BaseSpaceShip ship)
        {
           
        }
    }
}