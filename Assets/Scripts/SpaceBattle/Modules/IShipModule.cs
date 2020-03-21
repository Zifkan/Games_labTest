using SpaceBattle.Enums;
using SpaceBattle.SpaceShips;

namespace SpaceBattle.Modules
{
    public interface IShipModule
    {
        string ModuleName { get; }
        SlotType SlotType { get; }
        void OnAttachedToShip(BaseSpaceShip ship,Slot slot);
        void OnRemovedFromShip(BaseSpaceShip ship);
        bool IsValid { get; }
        void Release();
    }
}