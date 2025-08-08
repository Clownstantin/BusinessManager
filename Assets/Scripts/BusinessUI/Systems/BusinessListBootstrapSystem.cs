using Leopotam.EcsLite;
using UnityEngine;

namespace BusinessManager.Core.UI
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

            Transform parent = systems.GetShared<SceneContext>().MainCanvas.transform;
            BusinessWindowView window = Object.Instantiate(_config.BusinessWindowPrefab, parent);
            Transform container = window.ListContainer;

            // Pools / filters
            _world = systems.GetWorld();
            var businessPool = _world.GetPool<Business>();
            var businessFilter = _world.Filter<Business>().End();

            for (int i = 0; i < _config.Businesses.Length; i++)
            {
                var view = Object.Instantiate(_config.BusinessViewPrefab, container);
                view.Setup(i);
                view.SetName(_config.Businesses[i].NamesData != null ? _config.Businesses[i].NamesData.Name : $"Business {i + 1}");

                int bi = i;
                view.BuyLevelButton.onClick.AddListener(() => EcsExtentions.CreateRequest(_world, new BuyLevelRequest { BusinessIndex = bi }));
                view.BuyEnh1Button.onClick.AddListener(() => EcsExtentions.CreateRequest(_world, new BuyEnhancementRequest { BusinessIndex = bi, EnhancementIndex = 0 }));
                view.BuyEnh2Button.onClick.AddListener(() => EcsExtentions.CreateRequest(_world, new BuyEnhancementRequest { BusinessIndex = bi, EnhancementIndex = 1 }));

                // Attach link to corresponding business entity via generic MonoLink
                foreach (int e in businessFilter)
                {
                    if (businessPool.Get(e).Index == bi)
                    {
                        _world.SetLink<BusinessView>(e, view);
                        break;
                    }
                }
            }
        }
    }
}