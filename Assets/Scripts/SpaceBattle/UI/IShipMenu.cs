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

        void SetModulesCollection(List<IShipModuleFactory> modules);
        
        void SetSlotsCollection(List<Slot> modules);
    }
}