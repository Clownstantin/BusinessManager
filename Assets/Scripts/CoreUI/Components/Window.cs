using Leopotam.EcsLite;

namespace Core
{
    namespace UI
    {
        public readonly struct Window { }
    }

    public partial class PoolContainer
    {
        private EcsPool<UI.Window> _window;

        public EcsPool<UI.Window> Window => _window ??= _world.GetPool<UI.Window>();
    }
}
