using Leopotam.EcsLite;
using UnityEngine;

namespace Core
{
    public class BusinessModule : IECSModule
    {
        public System.Type ModuleDataType => typeof(BusinessModuleData);

        public void Init(IEcsSystems systems, ScriptableObject data = null)
        {
            var businessData = data as BusinessModuleData;

            systems.Add(new CreateBusinessSystem(businessData))
                   .Add(new IncomeProgressSystem())
                   .Add(new ApplyPayoutSystem())
                   .Add(new BuyLevelSystem())
                   .Add(new BuyEnhancementSystem(businessData));
        }
    }
}