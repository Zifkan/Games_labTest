using SpaceBattle.Enums;
using SpaceBattle.SpaceShips;
using SpaceBattle.Utils;
using UnityEditor;
using UnityEngine;

namespace SpaceBattle.Modules
{
    public class ShieldModule : ShipModule , IShipModule
    {
        [SerializeField]
        private float _shield = 50f;
        public SlotType SlotType => _slotType;
        
        public void OnAttachedToShip(BaseSpaceShip ship)
        {
            ship.SetHealth(_shield);
        }

        public void OnRemovedFromShip(BaseSpaceShip ship)
        {
            ship.SetHealth(-_shield);
        }
        
        [MenuItem("Assets/Modules/ShieldModule")]
        public static void CreateAsset ()
        {
            ScriptableObjectUtility.CreateAsset<ShieldModule> ();
        }
    }
}