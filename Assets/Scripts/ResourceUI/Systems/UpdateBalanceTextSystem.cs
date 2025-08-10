using Leopotam.EcsLite;
using TMPro;

namespace Core.UI
{
    public sealed class UpdateBalanceTextSystem : IEcsInitSystem, IEcsRunSystem
    {
        private PoolContainer _pool;
        private EcsFilter _payoutFilter;
        private EcsFilter _balanceChangedFilter;
        private EcsFilter _balanceFilter;

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _pool = systems.GetShared<SharedData>().PoolContainer;
            _payoutFilter = world.Filter<PayoutEvent>().End();
            _balanceChangedFilter = world.Filter<BalanceChangedEvent>().End();
            _balanceFilter = world.Filter<Balance>().Inc<MonoLink<TextMeshProUGUI>>().End();
        }

        public void Run(IEcsSystems systems)
        {
            bool needUpdate = false;
            foreach (int _ in _payoutFilter)
                needUpdate = true;

            foreach (int _ in _balanceChangedFilter)
                needUpdate = true;

            if (!needUpdate)
                return;

            int balanceEntity = _balanceFilter.GetFirstEntity();
            float amount = _pool.Balance.Get(balanceEntity).Amount;
            TextMeshProUGUI textView = _pool.TextView.Get(balanceEntity).Value;
            textView.text = $"Баланс: {amount}$";
        }
    }
}