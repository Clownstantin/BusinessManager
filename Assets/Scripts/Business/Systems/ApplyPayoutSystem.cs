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
        private int _balanceEntity = -1; // кэшируем singleton

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _pool = systems.GetShared<SharedData>().PoolContainer;
            _payoutFilter = _world.Filter<PayoutEvent>().End();
            _balanceFilter = _world.Filter<Balance>().End();

            foreach (var e in _balanceFilter)
            {
                _balanceEntity = e;
                break;
            }
        }

        public void Run(IEcsSystems systems)
        {
            if (_balanceEntity == -1)
                return;

            ref var balance = ref _pool.Balance.Get(_balanceEntity);
            foreach (var e in _payoutFilter)
            {
                balance.Amount = Mathf.Max(0f, balance.Amount + _pool.PayoutEvent.Get(e).Amount);
                _pool.PayoutEvent.Del(e);
            }
        }
    }
}