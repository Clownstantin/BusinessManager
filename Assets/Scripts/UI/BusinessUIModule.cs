using Leopotam.EcsLite;
using UnityEngine;

namespace BusinessManager.Core
{
    public class BusinessUIModule : IECSModule
    {
        public System.Type ModuleDataType => null;

        public void Init(IEcsSystems systems, ScriptableObject data = null)
        {
        }
    }
}