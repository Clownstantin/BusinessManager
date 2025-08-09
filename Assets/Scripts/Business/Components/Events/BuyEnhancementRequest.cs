using Leopotam.EcsLite;

namespace Core
{
    public struct BuyEnhancementRequest
    {
        public int BusinessIndex;
        public int EnhancementIndex;
    }

    public partial class PoolContainer
    {
        private EcsPool<BuyEnhancementRequest> _buyEnhancementRequest;
        public EcsPool<BuyEnhancementRequest> BuyEnhancementRequest => _buyEnhancementRequest ??= _world.GetPool<BuyEnhancementRequest>();
    }
}