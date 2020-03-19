namespace SpaceBattle.Utils
{
    public interface IPoolable
    {
        void PrepareToUse();
        void ReturnToPool();
    }
}