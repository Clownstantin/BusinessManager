using Leopotam.EcsLite;
using UnityEngine;

namespace Core
{
    public sealed class IncomeProgressSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _businessFilter;
        private PoolContainer _pool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _businessFilter = _world.Filter<Business>().End();
            _pool = systems.GetShared<SharedData>().PoolContainer;
        }

        public void Run(IEcsSystems systems)
        {
            float dt = Time.deltaTime;

            foreach (int entity in _businessFilter)
            {
                ref Business business = ref _pool.Business.Get(entity);

                if (business.Level <= 0 || business.IncomeDelay <= 0f)
                    continue;

                business.Progress01 += dt / business.IncomeDelay;
                if (business.Progress01 >= 1f)
                {
                    business.Progress01 %= 1f;

                    float multiplier = 1f + Mathf.Max(0f, business.EnhancementsMultiplierSum);
                    float payout = business.Level * business.BaseIncome * multiplier;

                    int e = _world.NewEntity();
                    ref PayoutEvent payoutEvent = ref _pool.PayoutEvent.Add(e);
                    payoutEvent.Amount = payout;
                }
            }
        }
    }
}