using Leopotam.EcsLite;

namespace Core
{
    public struct Balance
    {
        public float Amount;
    }

    public partial class PoolContainer
    {
        private EcsPool<Balance> _balance;
        public EcsPool<Balance> Balance => _balance ??= _world.GetPool<Balance>();
    }
}