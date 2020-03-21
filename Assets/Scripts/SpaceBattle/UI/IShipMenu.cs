using System;
using System.Collections.Generic;
using SpaceBattle.CustomEventArgs;
using SpaceBattle.Modules;
using SpaceBattle.Modules.Factory;

namespace SpaceBattle.UI
{
    public interface IShipMenu
    {
        event EventHandler<ButtonModuleEventArgs> SetModuleEvent;
        event EventHandler<ButtonSlotEventArgs> DetachModuleEvent;

        void SetModulesCollection(List<IShipModuleFactory> modules);
    }
}