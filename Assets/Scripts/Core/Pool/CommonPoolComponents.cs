using Leopotam.EcsLite;
using TMPro;
using UnityEngine.UI;

namespace Core
{
    public partial class PoolContainer
    {
        private EcsPool<MonoLink<TextMeshProUGUI>> _textView;
        private EcsPool<MonoLink<Slider>> _sliderView;
        private EcsPool<MonoLink<Image>> _imageView;

        public EcsPool<MonoLink<TextMeshProUGUI>> TextView => _textView ??= _world.GetPool<MonoLink<TextMeshProUGUI>>();
        public EcsPool<MonoLink<Slider>> SliderView => _sliderView ??= _world.GetPool<MonoLink<Slider>>();
        public EcsPool<MonoLink<Image>> ImageView => _imageView ??= _world.GetPool<MonoLink<Image>>();
    }
}
