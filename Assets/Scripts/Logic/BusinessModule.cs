using Leopotam.EcsLite;
using UnityEngine;

namespace BusinessManager.Core
{
    public class BusinessModule : IECSModule
    {
        public System.Type ModuleDataType => typeof(BusinessModuleData);

        public void Init(IEcsSystems systems, ScriptableObject data = null)
        {
            var logicData = data as BusinessModuleData;
            systems.Add(new TestSystem());
        }
    }
}