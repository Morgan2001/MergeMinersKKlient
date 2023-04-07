using Common.DI;
using Zenject;

namespace _Proxy
{
    public class ZenjectContainer : IServiceFactory, IServiceCollection, IServiceProvider
    {
        private readonly DiContainer _container;
        
        public ZenjectContainer()
        {
            _container = new DiContainer();
        }

        public ZenjectContainer(DiContainer container)
        {
            _container = container;
        }
        
        public IServiceCollection GetServiceCollection()
        {
            return this;
        }

        public IServiceProvider GetServiceProvider()
        {
            return this;
        }

        public void Register<TConcrete>() where TConcrete : class
        {
            _container.Bind<TConcrete>().AsTransient();
        }

        public void Register<TService, TImplementation>(bool collectionItem = false) where TService : class where TImplementation : class, TService
        {
            _container.Bind<TService>().To<TImplementation>().AsTransient();
        }

        public void RegisterSingleton<TConcrete>() where TConcrete : class
        {
            _container.Bind<TConcrete>().AsSingle();
        }

        public void RegisterSingleton<TService, TImplementation>() where TService : class where TImplementation : class, TService
        {
            _container.Bind<TService>().To<TImplementation>().AsSingle();
        }

        public void RegisterInstance<TConcrete>(TConcrete instance, bool collectionItem = false) where TConcrete : class
        {
            _container.BindInstance(instance);
        }

        public void Verify()
        {
        }

        public TService Resolve<TService>() where TService : class
        {
            return _container.Resolve<TService>();
        }
    }
}