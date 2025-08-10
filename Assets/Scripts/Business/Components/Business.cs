using Leopotam.EcsLite;

namespace Core
{
    public struct Business
    {
        public int Index;
        public int Level;

        public float BaseCost;
        public float BaseIncome;
        public float IncomeDelay;

        public float Progress;
        public float EnhancementsMultiplierSum;

        // Bit mask of purchased enhancements 
        public int PurchasedEnhancementsMask;
    }

    public partial class PoolContainer
    {
        private EcsPool<Business> _business;

        public EcsPool<Business> Business => _business ??= _world.GetPool<Business>();
    }
}