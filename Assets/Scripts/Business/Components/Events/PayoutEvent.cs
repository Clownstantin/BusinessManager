using Leopotam.EcsLite;

namespace Core
{
    public struct PayoutEvent
    {
        public float Amount;
    }

    public partial class PoolContainer
    {
        private EcsPool<PayoutEvent> _payoutEvent;
        public EcsPool<PayoutEvent> PayoutEvent => _payoutEvent ??= _world.GetPool<PayoutEvent>();
    }
}