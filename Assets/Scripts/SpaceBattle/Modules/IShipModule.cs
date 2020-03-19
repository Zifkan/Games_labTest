using SpaceBattle.Enums;
using SpaceBattle.SpaceShips;

namespace SpaceBattle.Modules
{
    public interface IShipModule
    {
        SlotType SlotType { get; }
        void OnAttachedToShip(BaseSpaceShip ship,Slot slot);
        void OnRemovedFromShip(BaseSpaceShip ship);
    }
}