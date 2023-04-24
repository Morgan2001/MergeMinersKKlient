using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.MVVM;

namespace UI.MissionsScreen
{
    public class MissionView : View<MissionViewModel>
    {
        [SerializeField] private Text _description;
        [SerializeField] private float _progressWidth;
        [SerializeField] private Image _progress;
        [SerializeField] private Text _progressText;
        [SerializeField] private Button _collectButton;

        private ReactiveEvent<string> _collectEvent = new();
        public IReactiveSubscription<string> CollectEvent => _collectEvent;

        protected override void BindInner(MissionViewModel vm)
        {
            _description.text = _vm.Description;
            _vm.Value.Bind(UpdateProgress).AddTo(this);
            
            _collectButton.Subscribe(OnCollectClick).AddTo(this);
        }

        private void UpdateProgress(int value)
        {
            var progress = Mathf.Clamp01((float) value / _vm.Total);
                
            var size = _progress.rectTransform.sizeDelta;
            size.x = _progressWidth * progress;
            _progress.rectTransform.sizeDelta = size;

            _progressText.text = $"{value}/{_vm.Total}";
            
            _collectButton.gameObject.SetActive(value >= _vm.Total);
        }

        private void OnCollectClick()
        {
            _collectEvent.Trigger(_vm.Id);
        }
    }

    public class MissionViewModel : ViewModel
    {
        public string Id { get; }
        public string Description { get; }
        public int Total { get; }

        private ReactiveProperty<int> _value = new();
        public IReactiveProperty<int> Value => _value;

        public MissionViewModel(string id, string description, int total)
        {
            Id = id;
            Description = description;
            Total = total;
        }

        public void SetProgress(int value)
        {
            _value.Set(value);
        }
    }
}