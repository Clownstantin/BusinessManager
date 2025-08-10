using Leopotam.EcsLite;

namespace Core
{
    public static class EcsSystemsExtensions
    {
        public static IEcsSystems DelHere<T>(this IEcsSystems systems) where T : struct
        => systems.Add(new DelHereSystem<T>());
    }
}