using Leopotam.EcsLite;
using UnityEngine;

namespace Core.UI
{
    public sealed class CreateMainWindowSystem : IEcsInitSystem
    {
        private MainWindowView _mainWindow;

        public CreateMainWindowSystem(MainWindowView windowView) => _mainWindow = windowView;

        public void Init(IEcsSystems systems)
        {
            SharedData shared = systems.GetShared<SharedData>();
            PoolContainer pool = shared.PoolContainer;
            Transform parent = shared.SceneContext.MainCanvas.transform;

            _mainWindow = Object.Instantiate(_mainWindow, parent);
            pool.Window.NewEntity(out int entity);
            pool.MainWindowView.Add(entity).Value = _mainWindow;
        }
    }
}