using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Practices.Unity;

namespace Siege.ServiceLocation.UnityAdapter
{
    public class UnityAdapter : IServiceLocatorAdapter
    {
        private IUnityContainer container;
        private IContextualServiceLocator locator;
        private readonly Hashtable factories = new Hashtable();

        public UnityAdapter(IUnityContainer container)
        {
            this.container = container;
        }

        public UnityAdapter() : this(new UnityContainer())
        {
            
        }

        public void Dispose()
        {
            container.Dispose();
        }

        public object GetInstance(Type type, string key)
        {
            return container.Resolve(type, key);
        }

        public object GetInstance(Type type)
        {
            return container.Resolve(type);
        }

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return container.ResolveAll(serviceType);
        }

        public IEnumerable<TService> GetAllInstances<TService>()
        {
            return container.ResolveAll<TService>();
        }

        public void RegisterParentLocator(IContextualServiceLocator locator)
        {
            this.locator = locator;

            container.RegisterInstance<IServiceLocatorAdapter>(this);
            container.RegisterInstance<IServiceLocator>(locator);
            container.RegisterInstance(locator);
        }

        public IGenericFactory<TBaseService> GetFactory<TBaseService>()
        {
            if (!factories.ContainsKey(typeof(TBaseService)))
            {
                lock (factories.SyncRoot)
                {
                    if (!factories.ContainsKey(typeof(TBaseService)))
                    {
                        Factory<TBaseService> factory = new Factory<TBaseService>(locator);
                        locator.Register(Given<Factory<TBaseService>>.Then("Factory" + typeof(TBaseService), factory));

                        factories.Add(typeof(TBaseService), factory);
                    }
                }
            }

            return (Factory<TBaseService>)factories[typeof(TBaseService)];
        }

        public void RegisterBinding(Type baseBinding, Type targetBinding)
        {
            container.RegisterType(baseBinding, targetBinding);
        }

        public Type ConditionalUseCaseBinding
        {
            get { return typeof (ConditionalUseCaseBinding<>); }
        }

        public Type DefaultUseCaseBinding
        {
            get { return typeof (DefaultUseCaseBinding<>); }
        }

        public Type DefaultInstanceUseCaseBinding
        {
            get { return typeof (DefaultInstanceUseCaseBinding<>); }
        }

        public Type KeyBasedUseCaseBinding
        {
            get { return typeof (KeyBasedUseCaseBinding<>); }
        }
    }
}
