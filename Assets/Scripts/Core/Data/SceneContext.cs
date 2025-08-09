using UnityEngine;

namespace Core
{
    [System.Serializable]
    public class SceneContext
    {
        [field: SerializeField] public Camera MainCamera { get; private set; }
        [field: SerializeField] public Canvas MainCanvas { get; private set; }
    }
}