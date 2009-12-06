using System;
using System.Collections;
using System.Collections.Generic;
using Castle.Facilities.FactorySupport;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Siege.ServiceLocation;

namespace Siege.Container.WindsorAdapter
{
    public class WindsorAdapter : IServiceLocatorAdapter
    {
        private IKernel kernel;
        private IContextualServiceLocator locator;
        private readonly Hashtable factories = new Hashtable();

        public WindsorAdapter() : this(new DefaultKernel()) {}

        public WindsorAdapter(IKernel kernel)
        {
            this.kernel = kernel;
            this.kernel.AddFacility<FactorySupportFacility>();
        }

        public void RegisterParentLocator(IContextualServiceLocator locator)
        {
            this.locator = locator;
            
            kernel.Register(Component.For<IServiceLocator, IContextualServiceLocator>().Instance(locator));
            kernel.Register(Component.For<IServiceLocatorAdapter>().Instance(this));
        }

        public IGenericFactory<TBaseType> GetFactory<TBaseType>()
        {
            if (!factories.ContainsKey(typeof(TBaseType)))
            {
                lock (factories.SyncRoot)
                {
                    if (!factories.ContainsKey(typeof(TBaseType)))
                    {
                        Factory<TBaseType> factory = new Factory<TBaseType>(locator);
                        locator.Register(Given<Factory<TBaseType>>.Then("Factory" + typeof(TBaseType), factory));

                        factories.Add(typeof(TBaseType), factory);
                    }
                }
            }

            return (Factory<TBaseType>)factories[typeof(TBaseType)];
        }

        public void RegisterBinding(Type baseBinding, Type targetBinding)
        {
            kernel.Register(Component.For(baseBinding).ImplementedBy(targetBinding));
        }

        public void Dispose()
        {
            kernel.Dispose();
        }

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return (IEnumerable<object>)kernel.ResolveAll(serviceType);
        }

        public IEnumerable<TService> GetAllInstances<TService>()
        {
            return kernel.ResolveAll<TService>();
        }

        public object GetInstance(Type type)
        {
            return kernel.Resolve(type);
        }

        public object GetInstance(Type type, string key)
        {
            return kernel.Resolve(key, type);
        }
        
        public Type ConditionalUseCaseBinding
        {
            get { return typeof(ConditionalUseCaseBinding<>); }
        }

        public Type DefaultUseCaseBinding
        {
            get { return typeof(DefaultUseCaseBinding<>); }
        }

        public Type DefaultInstanceUseCaseBinding
        {
            get { return typeof(DefaultInstanceUseCaseBinding<>); }
        }

        public Type KeyBasedUseCaseBinding
        {
            get { return typeof(KeyBasedUseCaseBinding<>); }
        }
    }
}
