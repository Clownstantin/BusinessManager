using Leopotam.EcsLite;

namespace Core
{
    public struct BusinessViewRefreshEvent
    {
        public int BusinessIndex;
    }

    public partial class PoolContainer
    {
        private EcsPool<BusinessViewRefreshEvent> _businessViewRefreshEvent;
        public EcsPool<BusinessViewRefreshEvent> BusinessViewRefreshEvent => _businessViewRefreshEvent ??= _world.GetPool<BusinessViewRefreshEvent>();
    }
}