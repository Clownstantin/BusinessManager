using Leopotam.EcsLite;

namespace Core
{
    public static class EcsWorldMaskExtensions
    {
        public static EcsWorld.Mask Inc<T1, T2>(this EcsWorld.Mask mask)
        where T1 : struct
        where T2 : struct
        => mask.Inc<T1>().Inc<T2>();
    }
}