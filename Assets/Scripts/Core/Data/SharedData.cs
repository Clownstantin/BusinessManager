namespace Core
{
    public class SharedData
    {
        public PoolContainer PoolContainer { get; private set; }
        public SceneContext SceneContext { get; private set; }

        public SharedData(PoolContainer poolContainer, SceneContext sceneContext)
        {
            PoolContainer = poolContainer;
            SceneContext = sceneContext;
        }
    }
}