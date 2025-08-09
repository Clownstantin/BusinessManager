using Leopotam.EcsLite;

namespace Core
{
    public static class EcsRequestExtensions
    {
        public static void CreateRequest<T>(this EcsWorld world, in T request) where T : struct
        {
            var pool = world.GetPool<T>();
            pool.NewEntity(out _) = request;
        }
    }
}