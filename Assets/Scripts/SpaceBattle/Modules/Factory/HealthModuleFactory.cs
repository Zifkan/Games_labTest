using System.Collections.Generic;
using SpaceBattle.Enums;
using SpaceBattle.SpaceShips;
using SpaceBattle.Utils;
using UnityEditor;
using UnityEngine;

namespace SpaceBattle.Modules.Factory
{
    public class HealthModuleFactory : ScriptableObject, IShipModuleFactory
    {
        [SerializeField] 
        private string _moduleName;
        
        [SerializeField] 
        private float _additionalHealth;

        [SerializeField] 
        private SlotType _slotType;

        [SerializeField] 
        private GameObject _model;
        
        private readonly Queue<IShipModule> _pool = new Queue<IShipModule>();

        public IShipModule GetOrCreateModule()
        {
            if (_pool.Count > 0)
            {
                return _pool.Dequeue();
            }

            return new HealthModule(this, _additionalHealth, _slotType, _model){ModuleName = _moduleName};
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

            public HealthModule(HealthModuleFactory factory, float additionalHealth, SlotType slotType, GameObject model)
            {
                _factory = factory;
                _additionalHealth = additionalHealth;
                _slotType = slotType;
                _model = model;
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