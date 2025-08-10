using Leopotam.EcsLite;
using UnityEngine;

namespace Core
{
    public sealed class BuyEnhancementSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly BusinessModuleData _config;

        private EcsWorld _world;
        private EcsFilter _reqFilter;
        private EcsFilter _businessFilter;
        private EcsFilter _balanceFilter;
        private PoolContainer _pool;

        public BuyEnhancementSystem(BusinessModuleData config) => _config = config;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _pool = systems.GetShared<SharedData>().PoolContainer;
            _reqFilter = _world.Filter<BuyEnhancementRequest>().End();
            _businessFilter = _world.Filter<Business>().Inc<Bought>().End();
            _balanceFilter = _world.Filter<Balance>().End();
        }

        public void Run(IEcsSystems systems)
        {
            int balanceEntity = _balanceFilter.GetFirstEntity();
            if (balanceEntity == Index.Default)
                return; // no balance yet

            ref Balance balance = ref _pool.Balance.Get(balanceEntity);

            foreach (int reqEntity in _reqFilter)
            {
                BuyEnhancementRequest req = _pool.BuyEnhancementRequest.Get(reqEntity);

                int businessEntity = FindBusinessByIndex(req.BusinessIndex);
                if (businessEntity == Index.Default)
                    continue;

                ref Business business = ref _pool.Business.Get(businessEntity);

                // bounds and already purchased check
                EnhancementConfigData[] enhancements = _config.Businesses[business.Index].Enhancements;
                if (req.EnhancementIndex < 0 || req.EnhancementIndex >= enhancements.Length)
                    continue;

                int mask = 1 << req.EnhancementIndex;
                if ((business.PurchasedEnhancementsMask & mask) != 0)
                    continue; // already purchased

                int cost = enhancements[req.EnhancementIndex].Cost;
                float addMultiplier = Mathf.Max(0f, enhancements[req.EnhancementIndex].MultiplyFactor);

                if (balance.Amount >= cost)
                {
                    balance.Amount -= cost;
                    business.PurchasedEnhancementsMask |= mask;
                    business.EnhancementsMultiplierSum += addMultiplier;

                    _pool.BalanceChangedEvent.NewEntity(out _);
                    _pool.BusinessViewRefreshEvent.NewEntity(out _).BusinessIndex = business.Index;
                }
            }
        }

        private int FindBusinessByIndex(int index)
        {
            foreach (int e in _businessFilter)
            {
                if (_pool.Business.Get(e).Index == index)
                    return e;
            }
            return Index.Default;
        }
    }
}