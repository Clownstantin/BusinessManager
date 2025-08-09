using System;
using Leopotam.EcsLite;
using UnityEngine;

namespace Core.UI
{
    public class CoreUIModule : IECSModule
    {
        public Type ModuleDataType => typeof(CoreUIModuleData);

        public void Init(IEcsSystems systems, ScriptableObject data = null)
        {
            var uiData = data as CoreUIModuleData;
            systems.Add(new CreateMainWindowSystem(uiData.MainWindowPrefab));
        }
    }
}
