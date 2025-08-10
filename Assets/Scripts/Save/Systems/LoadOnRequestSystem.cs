using Leopotam.EcsLite;
using UnityEngine;

namespace Core
{
    public sealed class LoadOnRequestSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private PoolContainer _pool;
        private EcsFilter _loadFilter;
        private EcsFilter _balanceFilter;
        private EcsFilter _businessFilter;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _pool = systems.GetShared<SharedData>().PoolContainer;
            _loadFilter = _world.Filter<LoadRequest>().End();
            _balanceFilter = _world.Filter<Balance>().End();
            _businessFilter = _world.Filter<Business>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int loadEntity in _loadFilter)
            {
                if (!JsonUtils.FromFile(Strings.SavePath, out SaveData data) || data == null)
                    break;

                // Balance
                int balanceEntity = _balanceFilter.GetFirstEntity();
                if (balanceEntity != Index.Default)
                    _pool.Balance.Get(balanceEntity).Amount = data.Balance;

                _pool.BalanceChangedEvent.NewEntity(out _);

                // Business
                if (data.Businesses != null)
                    foreach (int businessEntity in _businessFilter)
                    {
                        ref Business business = ref _pool.Business.Get(businessEntity);
                        for (int i = 0; i < data.Businesses.Length; i++)
                        {
                            if (data.Businesses[i].Index != business.Index)
                                continue;

                            business.Level = data.Businesses[i].Level;
                            business.EnhancementsMultiplierSum = data.Businesses[i].EnhancementsMultiplierSum;
                            business.PurchasedEnhancementsMask = data.Businesses[i].PurchasedEnhancementsMask;
                            business.Progress = Mathf.Clamp01(data.Businesses[i].Progress);

                            bool wasBought = data.Businesses[i].Bought;
                            if (wasBought)
                                _pool.Bought.AddWithCheck(businessEntity);
                            else if (_pool.Bought.Has(businessEntity))
                                _pool.Bought.Del(businessEntity);

                            _pool.BusinessViewRefreshEvent.NewEntity(out _).BusinessIndex = business.Index;

                            break;
                        }
                    }

                break;
            }
        }
    }
}