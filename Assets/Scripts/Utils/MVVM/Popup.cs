namespace Utils.MVVM
{
    public abstract class Popup<T> : View<T>
        where T : ViewModel
    {
        public void Show(T vm)
        {
            Bind(vm);
            
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            Dispose();
            
            gameObject.SetActive(false);
        }
    }
}