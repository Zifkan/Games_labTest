using SpaceBattle.SpaceShips;

namespace SpaceBattle.Modules
{
    public interface IShipModule
    {
        void OnAttachedToShip(BaseSpaceShip ship);
        void OnRemovedFromShip(BaseSpaceShip ship);
    }
}