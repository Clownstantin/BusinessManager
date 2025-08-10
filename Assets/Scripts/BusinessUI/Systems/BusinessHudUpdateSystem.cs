using Leopotam.EcsLite;
using UnityEngine;

namespace Core.UI
{
    public sealed class BusinessHudUpdateSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _businessFilter;
        private PoolContainer _pool;

        private BusinessModuleData _config;

        public BusinessHudUpdateSystem(BusinessModuleData config) => _config = config;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _businessFilter = _world.Filter<Business>().Inc<MonoLink<BusinessView>>().End();
            _pool = systems.GetShared<SharedData>().PoolContainer;
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int e in _businessFilter)
            {
                ref Business business = ref _pool.Business.Get(e);
                BusinessView view = _pool.BusinessView.Get(e).Value;
                if (view == null)
                    continue;

                float multiplier = 1f + Mathf.Max(0f, business.EnhancementsMultiplierSum);
                float income = business.Level * business.BaseIncome * multiplier;
                float levelPrice = (business.Level + 1) * business.BaseCost;

                view.LevelText.text = $"LVL\n{business.Level}";
                view.IncomeText.text = $"Доход\n{income}$";
                view.ProgressSlider.value = Mathf.Clamp01(business.Progress01);
                view.BuyLevelButtonText.text = $"LVL UP\n{GetPriceText(levelPrice)}";

                BusinessConfigData businessData = _config.Businesses[business.Index];
                EnhancementConfigData[] enh = businessData.Enhancements;
                string[] enhancementNames = businessData.NamesData.Enhancements;

                if (enh.Length == enhancementNames.Length)
                {
                    view.FirstEnhanceButtonText.text = GetEnhancementText(enh[0], enhancementNames[0]);
                    view.SecondEnhanceButtonText.text = GetEnhancementText(enh[1], enhancementNames[1]);
                }
            }

            static string GetPriceText(float price) => $"Цена: {price}$";

            static string GetEnhancementText(EnhancementConfigData enh, string enhancementName)
            => $"{enhancementName}\nДоход: +{enh.MultiplyFactor}%\n {GetPriceText(enh.Cost)}";
        }
    }
}