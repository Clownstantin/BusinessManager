using Leopotam.EcsLite;
using UnityEngine;

namespace Core.UI
{
    public sealed class BusinessListBootstrapSystem : IEcsInitSystem
    {
        private readonly BusinessModuleData _config;

        public BusinessListBootstrapSystem(BusinessModuleData config) => _config = config;

        public void Init(IEcsSystems systems)
        {
            if (_config == null || _config.Businesses == null || _config.BusinessViewPrefab == null)
                return;

            EcsWorld world = systems.GetWorld();
            SharedData shared = systems.GetShared<SharedData>();
            PoolContainer pool = shared.PoolContainer;
            EcsFilter businessFilter = world.Filter<Business>().End();
            EcsFilter mainViewFilter = world.Filter<MonoLink<MainWindowView>>().End();

            int mainViewEntity = mainViewFilter.GetFirstEntity();
            Transform container = pool.MainWindowView.Get(mainViewEntity).Value.BusinessContainer;

            for (int i = 0; i < _config.Businesses.Length; i++)
            {
                BusinessView view = Object.Instantiate(_config.BusinessViewPrefab, container);
                view.NameText.text = _config.Businesses[i].NamesData.Name;

                int businessIndex = i;
                BuyLevelRequest buyLevelReq = new() { BusinessIndex = businessIndex };

                view.BuyLevelButton.onClick.AddListener(() => pool.BuyLevelRequest.NewEntity(out _) = buyLevelReq);
                view.FirstEnhancementButton.onClick.AddListener(() => CreateEnhancementRequest(new() { BusinessIndex = businessIndex, EnhancementIndex = 0 }));
                view.SecondEnhancementButton.onClick.AddListener(() => CreateEnhancementRequest(new() { BusinessIndex = businessIndex, EnhancementIndex = 1 }));

                foreach (int businessEntity in businessFilter)
                {
                    int index = pool.Business.Get(businessEntity).Index;
                    if (index == i)
                    {
                        pool.BusinessView.Add(businessEntity).Value = view;
                        pool.BusinessViewRefreshEvent.NewEntity(out _).BusinessIndex = index;
                        break;
                    }
                }
            }

            void CreateEnhancementRequest(BuyEnhancementRequest enhancementReq)
            => pool.BuyEnhancementRequest.NewEntity(out _) = enhancementReq;
        }
    }
}