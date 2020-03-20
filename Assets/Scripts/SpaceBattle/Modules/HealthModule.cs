using SpaceBattle.Enums;
using SpaceBattle.SpaceShips;
using SpaceBattle.Utils;
using UnityEditor;
using UnityEngine;

namespace SpaceBattle.Modules
{
    public class HealthModule : BaseShipModule 
    {
        public override void OnAttachedToShip(BaseSpaceShip ship, Slot slot)
        {
            ship.SetHealth(_health);
            AttachModuleToSlot(slot.TransformPlace);
        }

        public override void OnRemovedFromShip(BaseSpaceShip ship)
        {
            ship.SetHealth(-_health);
        }
    }
}