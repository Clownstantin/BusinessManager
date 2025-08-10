using Leopotam.EcsLite;

namespace Core
{
    public sealed class SaveOnRequestSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private PoolContainer _pool;
        private EcsFilter _saveFilter;
        private EcsFilter _balanceFilter;
        private EcsFilter _businessFilter;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _pool = systems.GetShared<SharedData>().PoolContainer;
            _saveFilter = _world.Filter<SaveRequest>().End();
            _balanceFilter = _world.Filter<Balance>().End();
            _businessFilter = _world.Filter<Business>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int _ in _saveFilter)
            {
                SaveData data = new();

                int balanceEntity = _balanceFilter.GetFirstEntity();
                data.Balance = balanceEntity != Index.Default ? _pool.Balance.Get(balanceEntity).Amount : 0f;

                int count = _businessFilter.GetEntitiesCount();
                data.Businesses = new BusinessSaveData[count];

                int i = 0;
                foreach (int businessEntity in _businessFilter)
                {
                    ref Business b = ref _pool.Business.Get(businessEntity);
                    data.Businesses[i++] = new BusinessSaveData
                    {
                        Index = b.Index,
                        Level = b.Level,
                        EnhancementsMultiplierSum = b.EnhancementsMultiplierSum,
                        PurchasedEnhancementsMask = b.PurchasedEnhancementsMask,
                        Progress = b.Progress,
                        Bought = _pool.Bought.Has(businessEntity),
                    };
                }

                JsonUtils.ToFile(Strings.SavePath, data);
                break;
            }
        }
    }
}