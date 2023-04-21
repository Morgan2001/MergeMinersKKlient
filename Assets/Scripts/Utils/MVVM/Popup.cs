namespace Utils.MVVM
{
    public interface IPopup : IDisposableCarrier
    {
        void HideForce();
    }
    
    public interface IPopup<in T> : IPopup
    {
        IReactiveSubscription<IPopup<T>> CloseEvent { get; }
        void Show(T vm);
    }
    
    public abstract class Popup<T> : View<T>, IPopup<T>
        where T : ViewModel
    {
        private ReactiveEvent<IPopup<T>> _closeEvent = new();
        public IReactiveSubscription<IPopup<T>> CloseEvent => _closeEvent;
        
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

        private void HideInner(bool force)
        {
            gameObject.SetActive(false);

            if (!force)
            {
                _closeEvent.Trigger(this);
            }
            
            Dispose();
        }

        protected void Hide()
        {
            HideInner(false);
        }
        
        public void HideForce()
        {
            HideInner(true);
        }
    }
}