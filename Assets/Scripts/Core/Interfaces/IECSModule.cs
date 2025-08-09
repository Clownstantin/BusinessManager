using Leopotam.EcsLite;
using UnityEngine;

namespace Core
{
    public interface IECSModule
    {
        System.Type ModuleDataType { get; }
        void Init(IEcsSystems systems, ScriptableObject data = null);
    }
}