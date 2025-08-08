using System;
using Leopotam.EcsLite;
using UnityEngine;

namespace BusinessManager.Core
{
    public class SaveModule : IECSModule
    {
        public Type ModuleDataType => null;

        public void Init(IEcsSystems systems, ScriptableObject data = null)
        {

        }
    }
}