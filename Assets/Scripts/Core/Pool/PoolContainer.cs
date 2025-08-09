using Leopotam.EcsLite;

namespace Core
{
    public partial class PoolContainer
    {
        private readonly EcsWorld _world;

        public PoolContainer(EcsWorld world) => _world = world;
    }
}