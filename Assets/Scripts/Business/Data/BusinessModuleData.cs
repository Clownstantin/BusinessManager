using Core.UI;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = FileName, menuName = Strings.ModulePath + FileName)]
    public class BusinessModuleData : ScriptableObject
    {
        private const string FileName = nameof(BusinessModuleData);

        [field: Header("Business Module Settings")]
        [field: SerializeField] public BusinessConfigData[] Businesses { get; private set; }

        [field: Header("Business UI")]
        [field: SerializeField] public BusinessWindowView BusinessWindowPrefab { get; private set; }
        [field: SerializeField] public BusinessView BusinessViewPrefab { get; private set; }

#if UNITY_EDITOR
        private void OnValidate()
        {
            foreach (BusinessConfigData business in Businesses)
                business.Validate();
        }
#endif
    }
}