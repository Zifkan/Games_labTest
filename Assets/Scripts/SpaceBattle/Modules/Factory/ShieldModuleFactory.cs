using System.Collections.Generic;
using SpaceBattle.SpaceShips;
using UnityEngine;

namespace SpaceBattle.Modules.Factory
{
    public class ShieldModuleFactory : ScriptableObject, IShipModuleFactory
    {
        [SerializeField]
        private float _additionalShield;
        
        private readonly Queue<IShipModule> _pool = new Queue<IShipModule>();
    
        public IShipModule GetOrCreateModule()
        {
            return  _pool.Count > 0 ? _pool.Dequeue() : new ShieldModule(this,_additionalShield);
        }
        
        public void ReleaseModule(IShipModule module)
        {
            if(!module.IsValid) return;
            
            _pool.Enqueue(module);
        }
    }

    public class ShieldModule : BaseShipModule<ShieldModuleFactory>
    {
        private float _additionalShield;
        
        public float AdditionalShield => _additionalShield;

        public ShieldModule(ShieldModuleFactory factory,float value)
        {
            _factory = factory;
            _additionalShield = value;
        }

        public override void OnAttachedToShip(BaseSpaceShip ship, Slot slot)
        {
                
        }

        public override void OnRemovedFromShip(BaseSpaceShip ship)
        {
                
        }
    }
}