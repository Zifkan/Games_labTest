using System.Collections.Generic;
using SpaceBattle.SpaceShips;
using SpaceBattle.Utils;
using UnityEditor;
using UnityEngine;

namespace SpaceBattle.Modules.Factory
{
    public class HealthModuleFactory : ScriptableObject, IShipModuleFactory
    {
        [SerializeField] private float _additionalHealth;

        private readonly Queue<IShipModule> _pool = new Queue<IShipModule>();

        public IShipModule GetOrCreateModule()
        {
            return _pool.Count > 0 ? _pool.Dequeue() : new HealthModule(this, _additionalHealth);
        }

        public void ReleaseModule(IShipModule module)
        {
            if (!module.IsValid) return;

            _pool.Enqueue(module);
        }

        [MenuItem("Assets/Modules/HealthModuleFactory")]
        public static void CreateAsset()
        {
            ScriptableObjectUtility.CreateAsset<HealthModuleFactory>();
        }


        private class HealthModule : BaseShipModule
        {
            private readonly float _additionalHealth;

            public HealthModule(HealthModuleFactory factory, float additionalHealth)
            {
                _factory = factory;
                _additionalHealth = additionalHealth;
            }


            public override void OnAttachedToShip(BaseSpaceShip ship, Slot slot)
            {
                ship.SetHealth(_additionalHealth);
                AttachModuleToSlot(slot.TransformPlace);
            }

            public override void OnRemovedFromShip(BaseSpaceShip ship)
            {
                ship.SetHealth(-_additionalHealth);
            }
        }
    }
}