using Leopotam.EcsLite;
using UnityEngine;

namespace Core
{
    public sealed class CreateBusinessSystem : IEcsInitSystem
    {
        private readonly BusinessModuleData _config;

        public CreateBusinessSystem(BusinessModuleData config) => _config = config;

        public void Init(IEcsSystems systems)
        {
            if (_config == null || _config.Businesses == null || _config.Businesses.Length == 0)
                return;

            EcsWorld world = systems.GetWorld();
            PoolContainer pool = systems.GetShared<SharedData>().PoolContainer;

            var businesses = _config.Businesses;
            for (int i = 0; i < businesses.Length; i++)
            {
                var cfg = businesses[i];

                int entity = world.NewEntity();
                ref Business business = ref pool.Business.Add(entity);

                business.Index = i;
                business.Level = 0;
                business.BaseCost = cfg.BaseCost;
                business.BaseIncome = cfg.BaseIncome;
                business.IncomeDelay = Mathf.Max(0.0001f, cfg.IncomeDelay);
                business.Progress01 = 0f;
                business.EnhancementsMultiplierSum = 0f;
                business.PurchasedEnhancementsMask = 0;
            }
        }
    }
}