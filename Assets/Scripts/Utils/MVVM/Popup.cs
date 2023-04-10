namespace Utils.MVVM
{
    public interface IPopup
    {
        IReactiveSubscription<IPopup> CloseEvent { get; }
        void Hide();
    }
    
    public interface IPopup<in T> : IPopup
    {
        void Show(T vm);
    }
    
    public abstract class Popup<T> : View<T>, IPopup<T>
        where T : ViewModel
    {
        private ReactiveEvent<IPopup> _closeEvent = new();
        public IReactiveSubscription<IPopup> CloseEvent => _closeEvent;
        
        private void Awake()
        {
            Create();
        }

        protected virtual void Create()
        {
        }
        
        public void Show(T vm)
        {
            gameObject.SetActive(true);
            
            Bind(vm);
        }

        public void Hide()
        {
            Dispose();
            
            gameObject.SetActive(false);
            
            _closeEvent.Trigger(this);
        }
    }
}