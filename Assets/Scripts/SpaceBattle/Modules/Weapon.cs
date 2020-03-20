using SpaceBattle.Enums;
using SpaceBattle.SpaceShips;
using UnityEngine;

namespace SpaceBattle.Modules
{
    public class Weapon :BaseShipModule
    {
        [SerializeField]
        private float _coolDown;

        [SerializeField]
        private float _damage;

        public float CoolDown => _coolDown;

        public float Damage => _damage;

        public SlotType SlotType => _slotType;
        public override void OnAttachedToShip(BaseSpaceShip ship, Slot slot)
        {
           // Debug.Log("Attach "+ name);
        }

        public override void OnRemovedFromShip(BaseSpaceShip ship)
        {
        
        }
    }
}