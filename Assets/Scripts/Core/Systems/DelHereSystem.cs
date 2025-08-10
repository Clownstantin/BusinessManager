using Leopotam.EcsLite;

namespace Core
{
    public sealed class DelHereSystem<T> : IEcsInitSystem, IEcsRunSystem where T : struct
    {
        private EcsFilter _filter;
        private EcsPool<T> _pool;

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _pool = world.GetPool<T>();
            _filter = world.Filter<T>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter)
                _pool.Del(entity);
        }
    }
}