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

            systems.Add(new CreateBusinessSystem(businessData));
            systems.Add(new IncomeProgressSystem());
            systems.Add(new ApplyPayoutSystem());
            systems.Add(new BuyLevelSystem());
            systems.Add(new BuyEnhancementSystem(businessData));
        }
    }
}