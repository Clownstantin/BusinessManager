using Leopotam.EcsLite;

namespace Core
{
    public readonly struct Bought { }

    public partial class PoolContainer
    {
        private EcsPool<Bought> _bought;

        public EcsPool<Bought> Bought => _bought ??= _world.GetPool<Bought>();
    }
}