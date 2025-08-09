using Leopotam.EcsLite;
using UnityEngine;

namespace Core
{
    public sealed class ResourceModule : IECSModule
    {
        public System.Type ModuleDataType => null;

        public void Init(IEcsSystems systems, ScriptableObject data = null)
        {
            systems.Add(new CreateBalanceSystem());
        }
    }
}