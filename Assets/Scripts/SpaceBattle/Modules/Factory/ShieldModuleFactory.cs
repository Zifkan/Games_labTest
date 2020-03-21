using System.Collections.Generic;
using SpaceBattle.Enums;
using SpaceBattle.SpaceShips;
using SpaceBattle.Utils;
using UnityEditor;
using UnityEngine;

namespace SpaceBattle.Modules.Factory
{
    public class ShieldModuleFactory : ScriptableObject, IShipModuleFactory
    {
        [SerializeField] private string _moduleName;
        
        [SerializeField] private float _additionalShield;

        [SerializeField] private SlotType _slotType;

        [SerializeField] private GameObject _model;

        private readonly Queue<IShipModule> _pool = new Queue<IShipModule>();

        public IShipModule GetOrCreateModule()
        {
            return _pool.Count > 0 ? _pool.Dequeue() : new ShieldModule(this, _additionalShield, _slotType, _model){ModuleName = _moduleName};
        }

        public void ReleaseModule(IShipModule module)
        {
            if (!module.IsValid) return;

            _pool.Enqueue(module);
        }

        [MenuItem("Assets/Modules/ShieldModuleFactory")]
        public static void CreateAsset()
        {
            ScriptableObjectUtility.CreateAsset<ShieldModuleFactory>();
        }

        private class ShieldModule : BaseShipModule
        {
            private readonly float _additionalShield;

            public ShieldModule(ShieldModuleFactory factory, float value, SlotType slotType, GameObject model)
            {
                _factory = factory;
                _additionalShield = value;
                _slotType = slotType;
                _model = model;
            }

            public override void OnAttachedToShip(BaseSpaceShip ship, Slot slot)
            {
                AttachModuleToSlot(slot.TransformPlace);
                ship.SetShield(_additionalShield);
            }

            public override void OnRemovedFromShip(BaseSpaceShip ship)
            {
                ship.SetShield(-_additionalShield);
            }
        }
    }
}