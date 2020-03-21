using System.Collections.Generic;
using SpaceBattle.Enums;
using SpaceBattle.SpaceShips;
using SpaceBattle.Utils;
using UnityEditor;
using UnityEngine;

namespace SpaceBattle.Modules.Factory
{
    public class ReloadModuleFactory : ScriptableObject, IShipModuleFactory
    { 
        [SerializeField] private string _moduleName;
        
        [SerializeField] [Range(0, 1)] private float _reloadReduceRatio = 0.2f;
        
        [SerializeField] private SlotType _slotType;

        [SerializeField] private GameObject _model;

        private readonly Queue<IShipModule> _pool = new Queue<IShipModule>();

        public IShipModule GetOrCreateModule()
        {
            return _pool.Count > 0 ? _pool.Dequeue() : new ReloadModule(this, _reloadReduceRatio, _slotType, _model){ModuleName = _moduleName};
        }

        public void ReleaseModule(IShipModule module)
        {
            if (!module.IsValid) return;

            _pool.Enqueue(module);
        }

        [MenuItem("Assets/Modules/ReloadModuleFactory")]
        public static void CreateAsset()
        {
            ScriptableObjectUtility.CreateAsset<ReloadModuleFactory>();
        }

        private class ReloadModule : BaseShipModule
        {
            private float _reloadReduceRatio;

            public ReloadModule(ReloadModuleFactory factory, float reloadReduceRatio, SlotType slotType, GameObject model)
            {
                _factory = factory;
                _reloadReduceRatio = reloadReduceRatio;
                _slotType = slotType;
                _model = model;
            }

            public override void OnAttachedToShip(BaseSpaceShip ship, Slot slot)
            {
                AttachModuleToSlot(slot.TransformPlace);
                ship.SetReloadFactor(_reloadReduceRatio);
            }

            public override void OnRemovedFromShip(BaseSpaceShip ship)
            {

                ship.SetReloadFactor(-_reloadReduceRatio);
            }
        }
    }
}