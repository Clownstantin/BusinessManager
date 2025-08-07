using UnityEngine;

namespace BusinessManager.Core
{
    [CreateAssetMenu(fileName = Name, menuName = Strings.ModulePath + Name)]
    public class LogicModuleData : ScriptableObject
    {
        private const string Name = nameof(LogicModuleData);

        [field: Header("Logic Module Settings")]
        [field: SerializeField] public bool EnableGameLogic { get; private set; } = true;
    }
}