using UnityEngine;

namespace BusinessManager.Core
{
    [CreateAssetMenu(fileName = FileName, menuName = Strings.ModulePath + FileName)]
    public class BusinessModuleData : ScriptableObject
    {
        private const string FileName = nameof(BusinessModuleData);

        [field: Header("Business Module Settings")]
        [field: SerializeField] public BusinessConfigData[] Businesses { get; private set; }

#if UNITY_EDITOR
        private void OnValidate()
        {
            foreach (BusinessConfigData business in Businesses)
                business.Validate();
        }
#endif
    }
}