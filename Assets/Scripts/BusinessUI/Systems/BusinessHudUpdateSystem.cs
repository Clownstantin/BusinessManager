using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public sealed class BusinessHudUpdateSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _businessFilter;
        private EcsFilter _refreshFilter;
        private PoolContainer _pool;

        private BusinessModuleData _config;

        public BusinessHudUpdateSystem(BusinessModuleData config) => _config = config;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _refreshFilter = _world.Filter<BusinessViewRefreshEvent>().End();
            _businessFilter = _world.Filter<Business>().Inc<MonoLink<BusinessView>>().End();
            _pool = systems.GetShared<SharedData>().PoolContainer;
        }

        public void Run(IEcsSystems systems)
        {
            // Update progress slider only on bought business
            foreach (int businessEntity in _businessFilter)
                if (_pool.Bought.Has(businessEntity))
                {
                    ref Business business = ref _pool.Business.Get(businessEntity);
                    Slider slider = _pool.BusinessView.Get(businessEntity).Value.ProgressSlider;

                    slider.value = Mathf.Clamp01(business.Progress);
                }

            // Refresh text and buttons only on refresh event
            foreach (int reqEntity in _refreshFilter)
            {
                int targetIndex = _pool.BusinessViewRefreshEvent.Get(reqEntity).BusinessIndex;
                foreach (int businessEntity in _businessFilter)
                {
                    ref Business business = ref _pool.Business.Get(businessEntity);
                    if (business.Index != targetIndex)
                        continue;

                    BusinessView view = _pool.BusinessView.Get(businessEntity).Value;

                    float multiplier = 1f + Mathf.Max(0f, business.EnhancementsMultiplierSum);
                    float income = business.Level * business.BaseIncome * multiplier;
                    float levelPrice = (business.Level + 1) * business.BaseCost;

                    view.LevelText.text = $"LVL\n{business.Level}";
                    view.IncomeText.text = $"Доход\n{income}$";
                    view.BuyLevelButtonText.text = $"LVL UP\n{GetPriceText(levelPrice)}";

                    BusinessConfigData businessData = _config.Businesses[business.Index];
                    EnhancementConfigData[] enh = businessData.Enhancements;
                    string[] enhancementNames = businessData.NamesData.Enhancements;

                    if (enh.Length == enhancementNames.Length)
                    {
                        view.FirstEnhanceButtonText.text = GetEnhancementText(enh[0], enhancementNames[0]);
                        view.SecondEnhanceButtonText.text = GetEnhancementText(enh[1], enhancementNames[1]);
                    }

                    bool hasFirst = enh.Length > 0;
                    bool hasSecond = enh.Length > 1;

                    bool firstPurchased = (business.PurchasedEnhancementsMask & (1 << 0)) != 0;
                    bool secondPurchased = (business.PurchasedEnhancementsMask & (1 << 1)) != 0;

                    view.FirstEnhancementButton.interactable = hasFirst && !firstPurchased;
                    view.SecondEnhancementButton.interactable = hasSecond && !secondPurchased;

                    break;
                }
            }

            static string GetPriceText(in float price) => $"Цена: {price}$";

            static string GetEnhancementText(in EnhancementConfigData enh, string enhancementName)
            => $"{enhancementName}\nДоход: +{enh.MultiplyFactor * 100}%\n {GetPriceText(enh.Cost)}";
        }
    }
}