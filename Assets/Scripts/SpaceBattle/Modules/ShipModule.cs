using SpaceBattle.Enums;
using SpaceBattle.Modules.Factory;
using SpaceBattle.SpaceShips;
using Unity.Mathematics;
using UnityEngine;

namespace SpaceBattle.Modules
{
    public abstract class BaseShipModule : IShipModule
    {
        protected SlotType _slotType;
        protected IShipModuleFactory _factory;
        protected GameObject _model;
        
        protected GameObject _instantiatedModel;

        protected void AttachModuleToSlot(Transform slotTransform)
        {
            if (_model == null || slotTransform == null) return;

            _instantiatedModel = Object.Instantiate(_model);
            
            _instantiatedModel.transform.SetParent(slotTransform);
            _instantiatedModel.transform.localPosition = Vector3.zero;
            _instantiatedModel.transform.rotation = quaternion.identity;
        }

        public string ModuleName { get; set; }
        
        public SlotType SlotType => _slotType;

        public abstract void OnAttachedToShip(BaseSpaceShip ship, Slot slot);

        public abstract void OnRemovedFromShip(BaseSpaceShip ship);
        
        public bool IsValid => _factory != null;

        public void Release()
        {
            if(_factory == null) return;
            
            _factory.ReleaseModule(this);
            _factory = null;
        }
    }
    
    
    
    
    
    
    
    
    
    
}