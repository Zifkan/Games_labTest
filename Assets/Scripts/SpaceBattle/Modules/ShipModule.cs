using System;
using SpaceBattle.Enums;
using Unity.Mathematics;
using UnityEngine;

namespace SpaceBattle.Modules
{
    public class ShipModule : ScriptableObject
    {
        [SerializeField] private UInt32 _availableCount = 10;

        [SerializeField] protected SlotType _slotType;

        public GameObject Mesh;
        //public uint AvailableCount => _availableCount;

        protected void AttachModuleToSlot(Transform slotTransform)
        {
            Mesh.transform.parent.SetParent(slotTransform);
            Mesh.transform.localPosition = Vector3.zero;
            Mesh.transform.rotation = quaternion.identity;
        }
    }
}