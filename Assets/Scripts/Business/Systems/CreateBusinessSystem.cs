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

            PoolContainer pool = systems.GetShared<SharedData>().PoolContainer;
            BusinessConfigData[] businesses = _config.Businesses;

            for (int i = 0; i < businesses.Length; i++)
            {
                BusinessConfigData configData = businesses[i];
                ref Business business = ref pool.Business.NewEntity(out int entity);
                bool isFirst = i == 0;

                business.Index = i;
                business.Level = isFirst ? 1 : 0;
                business.BaseCost = configData.BaseCost;
                business.BaseIncome = configData.BaseIncome;
                business.IncomeDelay = Mathf.Max(0.0001f, configData.IncomeDelay);
                business.Progress = 0f;
                business.EnhancementsMultiplierSum = 0f;
                business.PurchasedEnhancementsMask = 0;

                pool.Bought.ConditionAdd(entity, isFirst);
            }
        }
    }
}