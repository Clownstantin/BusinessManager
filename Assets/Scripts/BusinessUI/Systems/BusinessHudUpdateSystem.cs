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
            _businessFilter = _world.Filter<Business>().End();
            _pool = systems.GetShared<SharedData>().PoolContainer;
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int e in _businessFilter)
            {
                ref var b = ref _pool.Business.Get(e);
                if (!_pool.BusinessView.Has(e))
                    continue;

                var view = _pool.BusinessView.Get(e).Value;
                if (view == null)
                    continue;

                float multiplier = 1f + Mathf.Max(0f, b.EnhancementsMultiplierSum);
                float income = b.Level * b.BaseIncome * multiplier;
                float levelPrice = (b.Level + 1) * b.BaseCost;

                view.SetLevel(b.Level);
                view.SetIncome(income);
                view.SetProgress01(b.Progress01);
                view.SetLevelPrice(levelPrice);

                if (_config != null && _config.Businesses != null && b.Index < _config.Businesses.Length)
                {
                    var enh = _config.Businesses[b.Index].Enhancements;
                    if (enh != null && enh.Length > 0)
                    {
                        view.SetEnh1Price(enh.Length > 0 ? enh[0].Cost : 0);
                        view.SetEnh2Price(enh.Length > 1 ? enh[1].Cost : 0);
                    }
                }
            }
        }
    }
}