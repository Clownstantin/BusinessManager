using Leopotam.EcsLite;
using UnityEngine;

namespace Core
{
    public sealed class ApplyPayoutSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _payoutFilter;
        private EcsFilter _balanceFilter;
        private PoolContainer _pool;
        private int _balanceEntity;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _pool = systems.GetShared<SharedData>().PoolContainer;
            _payoutFilter = _world.Filter<PayoutEvent>().End();
            _balanceFilter = _world.Filter<Balance>().End();

            _balanceEntity = _balanceFilter.GetFirstEntity();
        }

        public void Run(IEcsSystems systems)
        {
            if (_balanceEntity == Index.Default)
                return;

            ref Balance balance = ref _pool.Balance.Get(_balanceEntity);

            bool changed = false;
            foreach (int eventEntity in _payoutFilter)
            {
                float amount = _pool.PayoutEvent.Get(eventEntity).Amount;
                balance.Amount = Mathf.Max(0f, balance.Amount + amount);
                changed = true;
            }

            if (changed)
                _pool.BalanceChangedEvent.NewEntity(out _);
        }
    }
}