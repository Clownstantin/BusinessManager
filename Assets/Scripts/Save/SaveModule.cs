using System;
using Leopotam.EcsLite;
using UnityEngine;

namespace Core
{
    public class SaveModule : IECSModule
    {
        public Type ModuleDataType => null;

        public void Init(IEcsSystems systems, ScriptableObject data = null)
        {
            systems.Add(new EmitLoadOnStartSystem())
                   .Add(new LoadOnRequestSystem())
                   .Add(new EmitSaveOnQuitSystem())
                   .Add(new SaveOnRequestSystem())
                   .DelHere<LoadRequest>()
                   .DelHere<SaveRequest>();
        }
    }
}