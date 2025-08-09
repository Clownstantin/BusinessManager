using Leopotam.EcsLite;
using UnityEngine;

namespace Core.UI
{
    public sealed class BusinessListBootstrapSystem : IEcsInitSystem
    {
        private EcsWorld _world;
        private readonly BusinessModuleData _config;

        public BusinessListBootstrapSystem(BusinessModuleData config) { _config = config; }

        public void Init(IEcsSystems systems)
        {
            if (_config == null || _config.Businesses == null || _config.BusinessWindowPrefab == null || _config.BusinessViewPrefab == null)
                return;

            SharedData shared = systems.GetShared<SharedData>();
            PoolContainer pool = shared.PoolContainer;

            Transform parent = shared.SceneContext.MainCanvas.transform;
            BusinessWindowView window = Object.Instantiate(_config.BusinessWindowPrefab, parent);
            Transform container = window.ListContainer;

            _world = systems.GetWorld();
            var businessFilter = _world.Filter<Business>().End();

            for (int i = 0; i < _config.Businesses.Length; i++)
            {
                var view = Object.Instantiate(_config.BusinessViewPrefab, container);
                view.Setup(i);
                view.SetName(_config.Businesses[i].NamesData != null ? _config.Businesses[i].NamesData.Name : $"Business {i + 1}");

                int businessIndex = i;
                view.BuyLevelButton.onClick.AddListener(() => _world.CreateRequest(new BuyLevelRequest { BusinessIndex = businessIndex }));
                view.BuyEnh1Button.onClick.AddListener(() => _world.CreateRequest(new BuyEnhancementRequest { BusinessIndex = businessIndex, EnhancementIndex = 0 }));
                view.BuyEnh2Button.onClick.AddListener(() => _world.CreateRequest(new BuyEnhancementRequest { BusinessIndex = businessIndex, EnhancementIndex = 1 }));

                // Attach link to corresponding business entity via generic MonoLink
                foreach (int e in businessFilter)
                {
                    if (pool.Business.Get(e).Index == businessIndex)
                    {
                        pool.BusinessView.Add(e).Value = view;
                        break;
                    }
                }
            }
        }
    }
}