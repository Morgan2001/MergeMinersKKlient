namespace _Proxy.Data
{
    public class LocalPlayer
    {
        public string Id { get; private set; }

        public void Set(string id)
        {
            Id = id;
        }
    }
}