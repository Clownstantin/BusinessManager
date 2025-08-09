using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    namespace UI
    {
        public sealed class BusinessView : MonoBehaviour
        {
            [SerializeField] private Text _name;
            [SerializeField] private Text _level;
            [SerializeField] private Text _income;
            [SerializeField] private Slider _progress;
            [SerializeField] private Button _buyLevelButton;
            [SerializeField] private Text _buyLevelPrice;
            [SerializeField] private Button _buyEnh1Button;
            [SerializeField] private Text _buyEnh1Price;
            [SerializeField] private Button _buyEnh2Button;
            [SerializeField] private Text _buyEnh2Price;

            public int BusinessIndex { get; private set; }

            public void Setup(int businessIndex)
            {
                BusinessIndex = businessIndex;
            }

            public void SetName(string value) => _name.text = value;
            public void SetLevel(int value) => _level.text = $"Lv. {value}";
            public void SetIncome(float value) => _income.text = value.ToString("0.##");
            public void SetProgress01(float value) => _progress.value = Mathf.Clamp01(value);
            public void SetLevelPrice(float value) => _buyLevelPrice.text = value.ToString("0.##");
            public void SetEnh1Price(float value) => _buyEnh1Price.text = value.ToString("0.##");
            public void SetEnh2Price(float value) => _buyEnh2Price.text = value.ToString("0.##");

            public Button BuyLevelButton => _buyLevelButton;
            public Button BuyEnh1Button => _buyEnh1Button;
            public Button BuyEnh2Button => _buyEnh2Button;
        }
    }

    public partial class PoolContainer
    {
        private EcsPool<MonoLink<UI.BusinessView>> _businessView;
        public EcsPool<MonoLink<UI.BusinessView>> BusinessView => _businessView ??= _world.GetPool<MonoLink<UI.BusinessView>>();
    }
}