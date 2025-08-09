using Leopotam.EcsLite;
using UnityEngine;

namespace Core.UI
{
    public class BusinessUIModule : IECSModule
    {
        public System.Type ModuleDataType => typeof(BusinessModuleData);

        public void Init(IEcsSystems systems, ScriptableObject data = null)
        {
            var config = data as BusinessModuleData;
            systems.Add(new BusinessListBootstrapSystem(config));
            systems.Add(new BusinessHudUpdateSystem(config));
        }
    }
}