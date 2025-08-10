using Leopotam.EcsLite;

namespace Core
{
    public static class EcsFilterExtensions
    {
        public static int GetFirstEntity(this EcsFilter filter)
        {
            foreach (int entity in filter)
                return entity;
            return Index.Default;
        }
    }
}
