using System;
using SpaceBattle.Modules;
using SpaceBattle.Modules.Factory;
using SpaceBattle.SpaceShips;

namespace SpaceBattle.CustomEventArgs
{
    public class ButtonSlotEventArgs : EventArgs
    {
        private readonly Slot _slot;

        public Slot Slot => _slot;
        
        public ButtonSlotEventArgs(Slot slot)
        {
            _slot = slot;
        }
    }
    
    public class ButtonModuleEventArgs: EventArgs
    {
        private readonly IShipModuleFactory _moduleFactory;

        public IShipModuleFactory ModuleFactory => _moduleFactory;
        
        public ButtonModuleEventArgs(IShipModuleFactory module)
        {
            _moduleFactory = module;
        }
    }
}