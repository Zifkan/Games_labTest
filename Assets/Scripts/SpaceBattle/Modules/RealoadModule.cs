using SpaceBattle.Enums;
using SpaceBattle.SpaceShips;
using SpaceBattle.Utils;
using UnityEditor;

namespace SpaceBattle.Modules
{
    public class RealoadModule : ShipModule , IShipModule
    {
        public SlotType SlotType => _slotType;
        
        public void OnAttachedToShip(BaseSpaceShip ship)
        {
            throw new System.NotImplementedException();
        }

        public void OnRemovedFromShip(BaseSpaceShip ship)
        {
            throw new System.NotImplementedException();
        }
        
        [MenuItem("Assets/Modules/RealoadModule")]
                 public static void CreateAsset ()
                 {
                     ScriptableObjectUtility.CreateAsset<RealoadModule> ();
                 }
    }
}