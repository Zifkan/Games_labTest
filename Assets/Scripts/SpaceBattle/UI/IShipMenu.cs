using System;
using System.Collections.Generic;
using SpaceBattle.CustomEventArgs;
using SpaceBattle.Modules;

namespace SpaceBattle.UI
{
    public interface IShipMenu
    {
        event EventHandler<ButtonModuleEventArgs> SetModuleEvent;
        event EventHandler<ButtonSlotEventArgs> DetachModuleEvent;

        void SetModulesCollection(List<IShipModule> modules);
    }
}