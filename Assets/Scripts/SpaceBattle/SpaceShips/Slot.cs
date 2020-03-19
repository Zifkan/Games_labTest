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

        public SlotType Type => _slotType;

        public bool IsFree => _module != null;

        private IShipModule _module;

        public void SetModule(IShipModule module)
        {
            _module = module;
        }
    }
}