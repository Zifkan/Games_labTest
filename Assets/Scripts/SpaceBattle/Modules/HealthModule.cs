using System.Reflection;
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
        
        public void OnAttachedToShip(BaseSpaceShip ship)
        {
          ship.SetHealth(_health);
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