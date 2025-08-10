using Leopotam.EcsLite;
using UnityEngine;

namespace Core
{
    public sealed class BuyLevelSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _reqFilter;
        private EcsFilter _businessFilter;
        private EcsFilter _balanceFilter;
        private PoolContainer _pool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _pool = systems.GetShared<SharedData>().PoolContainer;
            _reqFilter = _world.Filter<BuyLevelRequest>().End();
            _businessFilter = _world.Filter<Business>().End();
            _balanceFilter = _world.Filter<Balance>().End();
        }

        public void Run(IEcsSystems systems)
        {
            int balanceEntity = -1;
            foreach (var e in _balanceFilter) { balanceEntity = e; break; }
            if (balanceEntity == -1)
                return; // no balance yet

            ref Balance balance = ref _pool.Balance.Get(balanceEntity);

            foreach (int reqEntity in _reqFilter)
            {
                int index = _pool.BuyLevelRequest.Get(reqEntity).BusinessIndex;

                // Find business by index (linear; for small N ok; otherwise map index->entity)
                int businessEntity = FindBusinessByIndex(index);
                Debug.Log($"BuyLevel {businessEntity}");
                if (businessEntity == -1)
                {
                    _pool.BuyLevelRequest.Del(reqEntity);
                    continue;
                }

                ref Business business = ref _pool.Business.Get(businessEntity);
                float cost = (business.Level + 1) * business.BaseCost;

                if (balance.Amount >= cost)
                {
                    balance.Amount -= cost;
                    business.Level++;
                }

                _pool.BuyLevelRequest.Del(reqEntity);
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