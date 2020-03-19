using SpaceBattle.Enums;
using UnityEngine;

namespace SpaceBattle.Modules
{
    public class ShipModule : ScriptableObject
    {
        [SerializeField] 
        protected SlotType _slotType;

        public GameObject Mesh;
    }
}