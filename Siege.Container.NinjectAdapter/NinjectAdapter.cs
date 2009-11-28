using System;
using System.Collections;
using System.Collections.Generic;
using Ninject;
using Ninject.Parameters;
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

        public TService GetInstance<TService>(string name, IDictionary constructorArguments)
        {
            return (TService) GetInstance(typeof(TService), name, constructorArguments);
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

        public object GetInstance(Type type, IDictionary constructorArguments)
        {
            if (constructorArguments == null || constructorArguments.Count == 0) return kernel.Get(type);

            List<ConstructorArgument> args = new List<ConstructorArgument>();

            foreach (string key in constructorArguments.Keys)
            {
                ConstructorArgument argument = new ConstructorArgument(key, constructorArguments[key]);
                args.Add(argument);
            }

            return kernel.Get(type, args.ToArray());
        }

        public object GetInstance(Type serviceType, string key, IDictionary constructorArguments)
        {
            if (constructorArguments == null || constructorArguments.Count == 0) return kernel.Get(serviceType, key);

            List<ConstructorArgument> args = new List<ConstructorArgument>();

            foreach (string name in constructorArguments.Keys)
            {
                ConstructorArgument argument = new ConstructorArgument(name, constructorArguments[key]);
                args.Add(argument);
            }

            return kernel.Get(serviceType, key, args.ToArray());
        }
    }
}