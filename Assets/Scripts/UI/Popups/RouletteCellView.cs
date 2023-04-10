using Utils.MVVM;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.Popups
{
    public class RouletteCellView : View<RouletteCellViewModel>
    {
        [SerializeField] private Text _level;
        [SerializeField] private Image _icon;
        
        protected override void BindInner(RouletteCellViewModel vm)
        {
            _vm.Level.Bind(UpdateLevel).AddTo(this);
            _vm.Icon.Bind(UpdateIcon).AddTo(this);
        }

        private void UpdateLevel(int value)
        {
            _level.text = value.ToString();
        }
        
        private void UpdateIcon(Sprite value)
        {
            _icon.sprite = value;
        }
    }

    public class RouletteCellViewModel : ViewModel
    {
        private readonly ReactiveProperty<int> _level = new();
        public IReactiveProperty<int> Level => _level;
        
        private readonly ReactiveProperty<Sprite> _icon = new();
        public IReactiveProperty<Sprite> Icon => _icon;

        public void SetLevel(int value)
        {
            _level.Set(value);
        }

        public void SetIcon(Sprite value)
        {
            _icon.Set(value);
        }
    }
}