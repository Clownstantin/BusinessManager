using Leopotam.EcsLite;
using UnityEngine;

namespace BusinessManager.Core
{
    public sealed class BuyLevelSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _reqFilter;
        private EcsFilter _businessFilter;
        private EcsFilter _balanceFilter;
        private EcsPool<BuyLevelRequest> _reqPool;
        private EcsPool<Business> _businessPool;
        private EcsPool<Balance> _balancePool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _reqPool = _world.GetPool<BuyLevelRequest>();
            _businessPool = _world.GetPool<Business>();
            _balancePool = _world.GetPool<Balance>();
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

            ref Balance balance = ref _balancePool.Get(balanceEntity);

            foreach (int reqEntity in _reqFilter)
            {
                int index = _reqPool.Get(reqEntity).BusinessIndex;

                // Find business by index (linear; for small N ok; otherwise map index->entity)
                int businessEntity = FindBusinessByIndex(index);
                if (businessEntity == -1)
                {
                    _reqPool.Del(reqEntity);
                    continue;
                }

                ref Business business = ref _businessPool.Get(businessEntity);
                float cost = (business.Level + 1) * business.BaseCost;

                if (balance.Amount >= cost)
                {
                    balance.Amount -= cost;
                    business.Level++;
                }

                _reqPool.Del(reqEntity);
            }
        }

        private int FindBusinessByIndex(int index)
        {
            foreach (int e in _businessFilter)
            {
                if (_businessPool.Get(e).Index == index)
                    return e;
            }
            return -1;
        }
    }
}


