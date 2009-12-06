using System;
using System.Collections;
using System.Collections.Generic;
using Ninject;
using Siege.ServiceLocation;

namespace Siege.Container.NinjectAdapter
{
    public class NinjectAdapter : IServiceLocatorAdapter
    {
        private IKernel kernel;
        private IContextualServiceLocator locator;
        private readonly Hashtable factories = new Hashtable();

        public NinjectAdapter() : this(new StandardKernel()) {}
        public NinjectAdapter(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public void RegisterParentLocator(IContextualServiceLocator locator)
        {
            this.locator = locator;

            kernel.Bind<IServiceLocatorAdapter>().ToConstant(this);
            locator.Register(Given<IServiceLocator>.Then(locator));
            locator.Register(Given<IContextualServiceLocator>.Then(locator));
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
            kernel.Bind(baseBinding).To(targetBinding);
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

        public void Dispose()
        {
            kernel.Dispose();
        }

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        public IEnumerable<TService> GetAllInstances<TService>()
        {
            return kernel.GetAll<TService>();
        }

        public object GetInstance(Type type)
        {
            return kernel.Get(type);
        }

        public object GetInstance(Type serviceType, string key)
        {
            return kernel.Get(serviceType, key);
        }
    }
}