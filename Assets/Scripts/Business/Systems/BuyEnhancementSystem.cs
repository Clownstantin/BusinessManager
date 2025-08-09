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
            _businessFilter = _world.Filter<Business>().End();
            _balanceFilter = _world.Filter<Balance>().End();
        }

        public void Run(IEcsSystems systems)
        {
            if (_config == null)
                return;

            int balanceEntity = -1;
            foreach (var e in _balanceFilter) { balanceEntity = e; break; }
            if (balanceEntity == -1)
                return; // no balance yet

            ref Balance balance = ref _pool.Balance.Get(balanceEntity);

            foreach (int reqEntity in _reqFilter)
            {
                var req = _pool.BuyEnhancementRequest.Get(reqEntity);

                int businessEntity = FindBusinessByIndex(req.BusinessIndex);
                if (businessEntity == -1)
                {
                    _pool.BuyEnhancementRequest.Del(reqEntity);
                    continue;
                }

                ref Business business = ref _pool.Business.Get(businessEntity);

                // bounds and already purchased check
                if (req.EnhancementIndex < 0 || req.EnhancementIndex >= _config.Businesses.Length)
                {
                    _pool.BuyEnhancementRequest.Del(reqEntity);
                    continue;
                }

                var enhancements = _config.Businesses[business.Index].Enhancements;
                if (req.EnhancementIndex >= enhancements.Length)
                {
                    _pool.BuyEnhancementRequest.Del(reqEntity);
                    continue;
                }

                int mask = 1 << req.EnhancementIndex;
                if ((business.PurchasedEnhancementsMask & mask) != 0)
                {
                    _pool.BuyEnhancementRequest.Del(reqEntity);
                    continue; // already purchased
                }

                int cost = enhancements[req.EnhancementIndex].Cost;
                float addMultiplier = Mathf.Max(0f, enhancements[req.EnhancementIndex].MultiplyFactor);

                if (balance.Amount >= cost)
                {
                    balance.Amount -= cost;
                    business.PurchasedEnhancementsMask |= mask;
                    business.EnhancementsMultiplierSum += addMultiplier;
                }

                _pool.BuyEnhancementRequest.Del(reqEntity);
            }
        }

        private int FindBusinessByIndex(int index)
        {
            foreach (int e in _businessFilter)
            {
                if (_pool.Business.Get(e).Index == index)
                    return e;
            }
            return -1;
        }
    }
}