using Leopotam.EcsLite;

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
            int balanceEntity = _balanceFilter.GetFirstEntity();
            if (balanceEntity == Index.Default)
                return;

            ref Balance balance = ref _pool.Balance.Get(balanceEntity);

            foreach (int reqEntity in _reqFilter)
            {
                int index = _pool.BuyLevelRequest.Get(reqEntity).BusinessIndex;
                int businessEntity = FindBusinessByIndex(index);
                if (businessEntity == Index.Default)
                    continue;

                ref Business business = ref _pool.Business.Get(businessEntity);
                float cost = (business.Level + 1) * business.BaseCost;

                if (balance.Amount >= cost)
                {
                    _pool.Bought.ConditionAdd(businessEntity, business.Level == 0);

                    balance.Amount -= cost;
                    business.Level++;
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