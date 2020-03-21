using System;
using System.Collections.Generic;
using SpaceBattle.CustomEventArgs;
using SpaceBattle.Modules.Factory;
using SpaceBattle.SpaceShips;

namespace SpaceBattle.UI
{
    public interface IShipMenu
    {
        event EventHandler<ButtonModuleEventArgs> SetModuleEvent;
        event EventHandler<ButtonSlotEventArgs> DetachModuleEvent;
        event EventHandler<int> SelectShipEvent;

        void SetModulesCollection(List<IShipModuleFactory> modules);
        void Refresh(List<Slot> modules);
        void SetShipButtons(List<BaseSpaceShip> ships);
    }
}