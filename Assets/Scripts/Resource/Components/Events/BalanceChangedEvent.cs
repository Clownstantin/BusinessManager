using Leopotam.EcsLite;

namespace Core
{
    public readonly struct BalanceChangedEvent { }

    public partial class PoolContainer
    {
        private EcsPool<BalanceChangedEvent> _balanceChangedEvent;
        public EcsPool<BalanceChangedEvent> BalanceChangedEvent => _balanceChangedEvent ??= _world.GetPool<BalanceChangedEvent>();
    }
}

