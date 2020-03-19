using SpaceBattle.SpaceShips;
using SpaceBattle.Utils;
using UnityEditor;
using UnityEngine;

namespace SpaceBattle.Modules
{
    public class Weapon :ShipModule , IShipModule
    {
        [SerializeField]
        private float _coolDown;

        [SerializeField]
        private float _damage;

        public float CoolDown => _coolDown;

        public float Damage => _damage;
        
        public void OnAttachedToShip(BaseSpaceShip ship)
        {
           
        }

        public void OnRemovedFromShip(BaseSpaceShip ship)
        {
            
        }
        
        [MenuItem("Assets/Modules/Weapon")]
        public static void CreateAsset ()
        {
            ScriptableObjectUtility.CreateAsset<Weapon> ();
        }
    }
}