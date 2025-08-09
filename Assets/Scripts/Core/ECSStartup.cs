using Leopotam.EcsLite;
using UnityEngine;

namespace Core
{
    public class ECSStartup : MonoBehaviour
    {
        [Header("Context")]
        [SerializeField] private SceneContext _sceneContext;
        [Header("Modules")]
        [SerializeField] private ModuleInfo[] _modules;

        private EcsWorld _world;
        private IEcsSystems _systems;
#if UNITY_EDITOR
        private IEcsSystems _editorSystems;
#endif

        private void Start()
        {
            if (_systems != null)
                return;

            _world = new EcsWorld();
            SharedData sharedData = new(new PoolContainer(_world), _sceneContext);
            _systems = new EcsSystems(_world, sharedData);

#if UNITY_EDITOR
            _editorSystems = new EcsSystems(_world);
            _editorSystems
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
                .Init();
#endif

            InitializeModules();

            _systems
#if UNITY_EDITOR
            .Add(new Leopotam.EcsLite.UnityEditor.EcsSystemsDebugSystem())
#endif
            .Init();
        }

        private void Update()
        {
            _systems?.Run();
#if UNITY_EDITOR
            _editorSystems?.Run();
#endif
        }

        private void OnDestroy()
        {
            if (_systems != null)
            {
                _systems.Destroy();
                _systems = null;
            }

#if UNITY_EDITOR
            if (_editorSystems != null)
            {
                _editorSystems.Destroy();
                _editorSystems = null;
            }
#endif

            if (_world != null)
            {
                _world.Destroy();
                _world = null;
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            foreach (ModuleInfo moduleInfo in _modules)
                moduleInfo.ValidateDataType();
        }
#endif

        private void InitializeModules()
        {
            if (_modules == null)
                return;

            foreach (ModuleInfo moduleInfo in _modules)
            {
                if (moduleInfo == null)
                    continue;

                IECSModule module = moduleInfo.ModuleInstance;
                if (module == null || module.ModuleDataType != null && moduleInfo.ModuleData == null)
                    continue;

                module.Init(_systems, moduleInfo.ModuleData);
            }
        }
    }
}