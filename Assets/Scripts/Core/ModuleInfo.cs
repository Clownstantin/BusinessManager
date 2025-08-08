using UnityEngine;

namespace BusinessManager.Core
{
    [System.Serializable]
    public class ModuleInfo
    {
        [field: SerializeField] public bool IsEnabled { get; private set; } = true;
        [field: SerializeReference] public IECSModule ModuleInstance { get; private set; }
        [field: SerializeField] public ScriptableObject ModuleData { get; private set; }

#if UNITY_EDITOR
        public void ValidateDataType()
        {
            if (ModuleInstance == null)
            {
                ModuleData = null;
                return;
            }

            var expected = ModuleInstance.ModuleDataType;
            if (ModuleData != null && expected != null && ModuleData.GetType() != expected)
                ModuleData = null;
        }
#endif
    }
}