using SpaceBattle.Enums;
using SpaceBattle.SpaceShips;

namespace SpaceBattle.Modules
{
    public interface IShipModule
    {
        SlotType SlotType { get; }
        void OnAttachedToShip(BaseSpaceShip ship);
        void OnRemovedFromShip(BaseSpaceShip ship);
    }
}