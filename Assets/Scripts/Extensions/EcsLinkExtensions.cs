using Leopotam.EcsLite;
using UnityEngine;

namespace BusinessManager.Core
{
    public static class EcsLinkExtensions
    {
        public static ref MonoLink<T> SetLink<T>(this EcsWorld world, int entity, T value) where T : Object
        {
            var pool = world.GetPool<MonoLink<T>>();
            if (!pool.Has(entity))
            {
                ref var link = ref pool.Add(entity);
                link.Value = value;
                return ref link;
            }
            else
            {
                ref var link = ref pool.Get(entity);
                link.Value = value;
                return ref link;
            }
        }

        public static bool TryGetLink<T>(this EcsWorld world, int entity, out T value) where T : Object
        {
            var pool = world.GetPool<MonoLink<T>>();
            if (pool.Has(entity))
            {
                value = pool.Get(entity).Value as T;
                return value != null;
            }
            value = null;
            return false;
        }

        public static void DelLink<T>(this EcsWorld world, int entity) where T : Object
        {
            var pool = world.GetPool<MonoLink<T>>();
            if (pool.Has(entity)) pool.Del(entity);
        }
    }
}