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
            _businessFilter = _world.Filter<Business>().Inc<Bought>().End();
            _pool = systems.GetShared<SharedData>().PoolContainer;
        }

        public void Run(IEcsSystems systems)
        {
            float dt = Time.deltaTime;

            foreach (int entity in _businessFilter)
            {
                ref Business business = ref _pool.Business.Get(entity);

                business.Progress += dt / business.IncomeDelay;
                if (business.Progress >= 1f)
                {
                    business.Progress %= 1f;

                    float multiplier = 1f + Mathf.Max(0f, business.EnhancementsMultiplierSum);
                    float payout = business.Level * business.BaseIncome * multiplier;

                    _pool.PayoutEvent.NewEntity(out _).Amount = payout;
                }
            }
        }
    }
}