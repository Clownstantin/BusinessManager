using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = FileName, menuName = Strings.BusinessPath + FileName)]
    public class BusinessNamesData : ScriptableObject
    {
        private const string FileName = nameof(BusinessNamesData);

        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string[] Enhancements { get; private set; }
    }
}