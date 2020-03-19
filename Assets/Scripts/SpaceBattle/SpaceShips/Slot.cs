using System;
using SpaceBattle.Enums;
using SpaceBattle.Modules;
using UnityEngine;

namespace SpaceBattle.SpaceShips
{
    [Serializable]
    public class Slot
    {
        [SerializeField]
        private SlotType _slotType;

        [SerializeField] 
        private Transform _transformPlace;
        
        private IShipModule _module;
        
        public SlotType Type => _slotType;

        public bool IsFree => _module != null;
        public Transform TransformPlace => _transformPlace;
        public void SetModule(IShipModule module)
        {
            _module = module;
        }
    }
}