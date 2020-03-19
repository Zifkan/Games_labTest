using System;

namespace SpaceBattle.Enums
{
    [Flags]  
    public enum SlotType
    {
        None = 0,
        Weapon = 1,
        Module = 2,
        Jopa = 4,
        Govno = 8,
    }
}