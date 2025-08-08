using Leopotam.EcsLite;
using UnityEngine;

namespace BusinessManager.Core
{
    public sealed class ApplyPayoutSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _payoutFilter;
        private EcsFilter _balanceFilter;
        private EcsPool<PayoutEvent> _payoutPool;
        private EcsPool<Balance> _balancePool;
        private int _balanceEntity = -1; // кэшируем singleton

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _payoutPool = _world.GetPool<PayoutEvent>();
            _balancePool = _world.GetPool<Balance>();
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
            if (_balanceEntity == -1) return; // баланс ещё не создан

            ref var balance = ref _balancePool.Get(_balanceEntity);
            foreach (var e in _payoutFilter)
            {
                balance.Amount = Mathf.Max(0f, balance.Amount + _payoutPool.Get(e).Amount);
                _payoutPool.Del(e);
            }
        }
    }
}