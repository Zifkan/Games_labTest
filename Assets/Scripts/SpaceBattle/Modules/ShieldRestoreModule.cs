using SpaceBattle.Enums;
using SpaceBattle.SpaceShips;
using SpaceBattle.Utils;
using UnityEditor;
using UnityEngine;

namespace SpaceBattle.Modules
{
    public class ShieldRestoreModule :ShipModule , IShipModule
    {
        [SerializeField][Range(0,1)]
        private float _shieldRestorePercent = 0.2f;
        
        public SlotType SlotType => _slotType;
        
        public void OnAttachedToShip(BaseSpaceShip ship,Slot slot)
        {
            ship.SetHealth(_shieldRestorePercent);
            AttachModuleToSlot(slot.TransformPlace);
        }    

        public void OnRemovedFromShip(BaseSpaceShip ship)
        {
            ship.SetHealth(-_shieldRestorePercent);
        }
        
        [MenuItem("Assets/Modules/ShieldRestoreModule")]
        public static void CreateAsset ()
        {
            ScriptableObjectUtility.CreateAsset<ShieldRestoreModule> ();
        }
    }
}