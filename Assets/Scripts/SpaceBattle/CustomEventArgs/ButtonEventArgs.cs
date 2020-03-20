using System;
using SpaceBattle.Modules;
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
        private readonly IShipModule _module;

        public IShipModule Module => _module;
        
        public ButtonModuleEventArgs(IShipModule module)
        {
            _module = module;
        }
    }
}