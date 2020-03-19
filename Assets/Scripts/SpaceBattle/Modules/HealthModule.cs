using SpaceBattle.Enums;
using SpaceBattle.SpaceShips;
using SpaceBattle.Utils;
using UnityEditor;
using UnityEngine;

namespace SpaceBattle.Modules
{
    public class HealthModule :ShipModule , IShipModule
    {
        [SerializeField]
        private float _health = 50f;
        
        public SlotType SlotType => _slotType;

        public void OnAttachedToShip(BaseSpaceShip ship, Slot slot)
        {
            ship.SetHealth(_health);
            AttachModuleToSlot(slot.TransformPlace);
        }

        public void OnRemovedFromShip(BaseSpaceShip ship)
        {
            ship.SetHealth(-_health);
        }
        
        [MenuItem("Assets/Modules/HealthModule")]
        public static void CreateAsset ()
        {
            ScriptableObjectUtility.CreateAsset<HealthModule> ();
        }
    }
}