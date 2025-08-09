using UnityEngine;

namespace Core
{
    [System.Serializable]
    public struct EnhancementConfigData
    {
        [field: SerializeField] public int Cost { get; private set; }
        [field: SerializeField] public float MultiplyFactor { get; private set; }
    }
}