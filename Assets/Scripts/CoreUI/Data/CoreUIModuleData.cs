using UnityEngine;

namespace Core.UI
{
    [CreateAssetMenu(fileName = FileName, menuName = Strings.ModulePath + FileName)]
    public class CoreUIModuleData : ScriptableObject
    {
        private const string FileName = nameof(CoreUIModuleData);

        [field: SerializeField] public MainWindowView MainWindowPrefab { get; private set; }
    }
}