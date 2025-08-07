using Leopotam.EcsLite;
using UnityEngine;

namespace BusinessManager.Core
{
    public class TestSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<TestComponent>().End();

            int entity = _world.NewEntity();
            _world.GetPool<TestComponent>().Add(entity);
        }

        public void Run(IEcsSystems systems)
        {
            EcsPool<TestComponent> pool = _world.GetPool<TestComponent>();
            foreach (var entity in _filter)
            {
                ref var component = ref pool.Get(entity);
                component.counter++;

                if (component.counter % 60 == 0)
                    Debug.Log($"[TestSystem] Entity {entity}: Counter = {component.counter}");
            }
        }
    }

    public struct TestComponent
    {
        public int counter;
    }
}