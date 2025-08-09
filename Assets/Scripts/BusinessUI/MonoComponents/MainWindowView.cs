using Leopotam.EcsLite;
using TMPro;
using UnityEngine;

namespace Core
{
    namespace UI
    {
        public sealed class MainWindowView : MonoBehaviour
        {
            [field: SerializeField] public Transform BusinessContainer { get; private set; }
            [field: SerializeField] public TextMeshProUGUI BalanceText { get; private set; }
        }
    }

    public partial class PoolContainer
    {
        private EcsPool<MonoLink<UI.MainWindowView>> _mainWindowView;
        public EcsPool<MonoLink<UI.MainWindowView>> MainWindowView => _mainWindowView ??= _world.GetPool<MonoLink<UI.MainWindowView>>();
    }
}