using System.Collections.Generic;
using SpaceBattle.Enums;
using SpaceBattle.SpaceShips;
using SpaceBattle.Utils;
using UnityEditor;
using UnityEngine;

namespace SpaceBattle.Modules.Factory
{
    public class ShieldRestoreModuleFactory : ScriptableObject, IShipModuleFactory
    {
        [SerializeField] private string _moduleName;
        
        [SerializeField] [Range(0, 1)] private float _shieldRestorePercent = 0.2f;

        [SerializeField] private SlotType _slotType;

        [SerializeField] private GameObject _model;

        private readonly Queue<IShipModule> _pool = new Queue<IShipModule>();

        public IShipModule GetOrCreateModule()
        {
            return _pool.Count > 0 ? _pool.Dequeue() : new ShieldRestoreModule(this, _shieldRestorePercent, _slotType,_model){ModuleName = _moduleName};
        }

        public void ReleaseModule(IShipModule module)
        {
            if (!module.IsValid) return;

            _pool.Enqueue(module);
        }

        [MenuItem("Assets/Modules/ShieldRestoreModuleFactory")]
        public static void CreateAsset()
        {
            ScriptableObjectUtility.CreateAsset<ShieldRestoreModuleFactory>();
        }

        private class ShieldRestoreModule : BaseShipModule
        {
            private readonly float _shieldRestorePercent;

            public ShieldRestoreModule(ShieldRestoreModuleFactory factory, float restoreValue, SlotType slotType, GameObject model)
            {
                _factory = factory;
                _shieldRestorePercent = restoreValue;
                _slotType = slotType;
                _model = model;
            }

            public override void OnAttachedToShip(BaseSpaceShip ship, Slot slot)
            {
                ship.SetShieldRestoreFactor(_shieldRestorePercent);
                AttachModuleToSlot(slot.TransformPlace);
            }

            public override void OnRemovedFromShip(BaseSpaceShip ship)
            {
                ship.SetShieldRestoreFactor(-_shieldRestorePercent);
            }
        }
    }
}