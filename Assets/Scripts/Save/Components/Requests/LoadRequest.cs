using Leopotam.EcsLite;

namespace Core
{
    public readonly struct LoadRequest { }

    public partial class PoolContainer
    {
        private EcsPool<LoadRequest> _loadRequest;
        public EcsPool<LoadRequest> LoadRequest => _loadRequest ??= _world.GetPool<LoadRequest>();
    }
}

