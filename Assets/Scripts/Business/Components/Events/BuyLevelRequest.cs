using Leopotam.EcsLite;

namespace Core
{
    public struct BuyLevelRequest
    {
        public int BusinessIndex;
    }

    public partial class PoolContainer
    {
        private EcsPool<BuyLevelRequest> _buyLevelRequest;
        public EcsPool<BuyLevelRequest> BuyLevelRequest => _buyLevelRequest ??= _world.GetPool<BuyLevelRequest>();
    }
}