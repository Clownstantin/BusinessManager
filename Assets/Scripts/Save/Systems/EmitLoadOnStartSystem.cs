using Leopotam.EcsLite;

namespace Core
{
    public sealed class EmitLoadOnStartSystem : IEcsInitSystem
    {
        public void Init(IEcsSystems systems)
        {
            PoolContainer pool = systems.GetShared<SharedData>().PoolContainer;
            pool.LoadRequest.NewEntity(out _);
        }
    }
}