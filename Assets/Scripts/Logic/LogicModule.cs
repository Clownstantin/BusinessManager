using Leopotam.EcsLite;
using UnityEngine;

namespace BusinessManager.Core
{
    public class LogicModule : IECSModule
    {
        public System.Type ModuleDataType => typeof(LogicModuleData);

        public void Init(IEcsSystems systems, ScriptableObject data = null)
        {
            var logicData = data as LogicModuleData;
            systems.Add(new TestSystem());
        }
    }
}