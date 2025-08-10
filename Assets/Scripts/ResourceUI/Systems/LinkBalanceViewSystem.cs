using Leopotam.EcsLite;
using TMPro;

namespace Core.UI
{
    public sealed class LinkBalanceTextSystem : IEcsInitSystem
    {
        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            PoolContainer pool = systems.GetShared<SharedData>().PoolContainer;
            EcsFilter balanceFilter = world.Filter<Balance>().End();
            EcsFilter mainViewFilter = world.Filter<MonoLink<MainWindowView>>().End();

            int balanceEntity = balanceFilter.GetFirstEntity();
            int mainViewEntity = mainViewFilter.GetFirstEntity();
            float amount = pool.Balance.Get(balanceEntity).Amount;
            TextMeshProUGUI balanceText = pool.MainWindowView.Get(mainViewEntity).Value.BalanceText;

            pool.TextView.Add(balanceEntity).Value = balanceText;
            balanceText.text = $"Баланс: {amount}$";
        }
    }
}