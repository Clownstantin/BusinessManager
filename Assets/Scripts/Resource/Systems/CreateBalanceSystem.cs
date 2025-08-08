using Leopotam.EcsLite;

namespace BusinessManager.Core
{
    public sealed class CreateBalanceSystem : IEcsInitSystem
    {
        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            var balancePool = world.GetPool<Balance>();
            var balanceFilter = world.Filter<Balance>().End();

            foreach (int _ in balanceFilter)
                return;

            int entity = world.NewEntity();
            ref Balance balance = ref balancePool.Add(entity);
            balance.Amount = 0f;
        }
    }
}