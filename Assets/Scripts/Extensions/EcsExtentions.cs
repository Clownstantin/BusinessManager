using Leopotam.EcsLite;

namespace BusinessManager.Core.UI
{
    public static class EcsExtentions
    {
        public static void CreateRequest<T>(this EcsWorld world, in T request) where T : struct
        {
            var pool = world.GetPool<T>();
            int e = world.NewEntity();
            pool.Add(e) = request;
        }
    }
}