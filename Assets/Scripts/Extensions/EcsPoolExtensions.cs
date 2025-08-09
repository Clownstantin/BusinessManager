using Leopotam.EcsLite;

namespace Core
{
    public static class EcsPoolExtensions
    {
        public static ref T AddWithCheck<T>(this EcsPool<T> pool, in int entity) where T : struct
        {
            if (pool.Has(entity))
                return ref pool.Get(entity);
            else
                return ref pool.Add(entity);
        }

        public static void ConditionAdd<T>(this EcsPool<T> pool, in int entity, in bool condition) where T : struct
        {
            if (condition)
                pool.Add(entity);
        }
    }
}
