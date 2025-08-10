using Leopotam.EcsLite;

namespace Core
{
    public readonly struct SaveRequest { }

    public partial class PoolContainer
    {
        private EcsPool<SaveRequest> _saveRequest;
        public EcsPool<SaveRequest> SaveRequest => _saveRequest ??= _world.GetPool<SaveRequest>();
    }
}

