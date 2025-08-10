using Leopotam.EcsLite;
using UnityEngine;

namespace Core
{
    public sealed class EmitSaveOnQuitSystem : IEcsInitSystem, IEcsRunSystem
    {
        private bool _quitRequested;
        private bool _pauseRequested;
        private PoolContainer _pool;

        public void Init(IEcsSystems systems)
        {
            _pool = systems.GetShared<SharedData>().PoolContainer;
            Application.quitting += OnQuitting;
            Application.focusChanged += OnFocusChanged;
        }

        public void Run(IEcsSystems systems)
        {
            if (_quitRequested || _pauseRequested)
            {
                _pool.SaveRequest.NewEntity(out _);
                _quitRequested = false;
                _pauseRequested = false;
            }
        }

        private void OnQuitting() => _quitRequested = true;

        private void OnFocusChanged(bool hasFocus)
        {
            if (!hasFocus)
                _pauseRequested = true;
        }
    }
}