namespace SpaceBattle.Modules.Factory
{
    public interface IShipModuleFactory
    {
        IShipModule GetOrCreateModule();

        void ReleaseModule(IShipModule module);
    }
}