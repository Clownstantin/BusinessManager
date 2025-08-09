using System;
using Leopotam.EcsLite;
using UnityEngine;

namespace Core.UI
{
    public class ResourceUIModule : IECSModule
    {
        public Type ModuleDataType => null;

        public void Init(IEcsSystems systems, ScriptableObject data = null)
        {
            systems.Add(new LinkBalanceTextSystem());
        }
    }
}
