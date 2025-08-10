using Leopotam.EcsLite;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    namespace UI
    {
        public sealed class BusinessView : MonoBehaviour
        {
            [field: SerializeField] public TextMeshProUGUI NameText { get; private set; }
            [field: SerializeField] public TextMeshProUGUI LevelText { get; private set; }
            [field: SerializeField] public TextMeshProUGUI IncomeText { get; private set; }
            [field: SerializeField] public TextMeshProUGUI BuyLevelButtonText { get; private set; }
            [field: SerializeField] public TextMeshProUGUI FirstEnhanceButtonText { get; private set; }
            [field: SerializeField] public TextMeshProUGUI SecondEnhanceButtonText { get; private set; }
            [field: SerializeField] public Slider ProgressSlider { get; private set; }
            [field: SerializeField] public Button BuyLevelButton { get; private set; }
            [field: SerializeField] public Button FirstEnhancementButton { get; private set; }
            [field: SerializeField] public Button SecondEnhancementButton { get; private set; }
        }
    }

    public partial class PoolContainer
    {
        private EcsPool<MonoLink<UI.BusinessView>> _businessView;
        public EcsPool<MonoLink<UI.BusinessView>> BusinessView => _businessView ??= _world.GetPool<MonoLink<UI.BusinessView>>();
    }
}