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

                BuyLevelRequest buyLevelReq = new() { BusinessIndex = i };
                BuyEnhancementRequest buyEnhancmentReq = new() { BusinessIndex = i, EnhancementIndex = 0 };

                view.BuyLevelButton.onClick.AddListener(() => pool.BuyLevelRequest.NewEntity(out _) = buyLevelReq);
                view.FirstEnhancementButton.onClick.AddListener(() => pool.BuyEnhancementRequest.NewEntity(out _) = buyEnhancmentReq);
                view.SecondEnhancementButton.onClick.AddListener(() =>
                {
                    buyEnhancmentReq.EnhancementIndex = 1;
                    pool.BuyEnhancementRequest.NewEntity(out _) = buyEnhancmentReq;
                });

                foreach (int e in businessFilter)
                {
                    if (pool.Business.Get(e).Index == i)
                    {
                        pool.BusinessView.Add(e).Value = view;
                        break;
                    }
                }
            }
        }
    }
}