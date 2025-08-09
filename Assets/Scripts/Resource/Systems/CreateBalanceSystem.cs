using Leopotam.EcsLite;

namespace Core
{
    public sealed class CreateBalanceSystem : IEcsInitSystem
    {
        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            PoolContainer pool = systems.GetShared<SharedData>().PoolContainer;
            var balanceFilter = world.Filter<Balance>().End();

            foreach (int _ in balanceFilter)
                return;

            pool.Balance.NewEntity(out _);
        }
    }
}