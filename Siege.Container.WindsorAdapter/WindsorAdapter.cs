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

        public TService GetInstance<TService>(string key, IDictionary constructorArguments)
        {
            return (TService)GetInstance(typeof (TService), key, constructorArguments);
        }

        public IMinimalServiceLocator Register<TService>(IUseCase<TService> useCase)
        {
            if (useCase is IConditionalUseCase<TService>)
            {
                var conditionalCase = useCase as IConditionalUseCase<TService>;

                conditionalCase.Bind(this.kernel, this);
            }
            else if (useCase is KeyBasedInstanceUseCase<TService>)
            {
                var keyCase = useCase as KeyBasedInstanceUseCase<TService>;

                keyCase.Bind(this.kernel);
            }
            else if (useCase is KeyBasedUseCase<TService>)
            {
                var keyCase = useCase as KeyBasedUseCase<TService>;

                keyCase.Bind(this.kernel);
            }
            else if (useCase is DefaultInstanceUseCase<TService>)
            {
                var implementation = useCase as DefaultInstanceUseCase<TService>;

                implementation.Bind(kernel, this);
            }
            else if (useCase is IDefaultUseCase<TService>)
            {
                IDefaultUseCase<TService> genericCase = useCase as IDefaultUseCase<TService>;

                genericCase.Bind(kernel, this);
            }

            return this;
        }

        public void RegisterParentLocator(IContextualServiceLocator locator)
        {
            this.locator = locator;
            kernel.Register(Component.For<IServiceLocator, IContextualServiceLocator>().Instance(locator));
        }

        public IGenericFactory<TBaseType> GetFactory<TBaseType>()
        {
            if (!factories.ContainsKey(typeof(TBaseType)))
            {
                lock (factories.SyncRoot)
                {
                    if (!factories.ContainsKey(typeof(TBaseType)))
                    {
                        Factory<TBaseType> factory = new Factory<TBaseType>(this.locator);
                        Register(Given<Factory<TBaseType>>.Then("Factory" + typeof(TBaseType), factory));

                        factories.Add(typeof(TBaseType), factory);
                    }
                }
            }

            return (Factory<TBaseType>)factories[typeof(TBaseType)];
        }

        public void Dispose()
        {
            this.kernel.Dispose();
        }

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return (IEnumerable<object>)kernel.ResolveAll(serviceType);
        }

        public IEnumerable<TService> GetAllInstances<TService>()
        {
            return kernel.ResolveAll<TService>();
        }

        public object GetInstance(Type type, IDictionary constructorArguments)
        {
            if (constructorArguments == null) return kernel.Resolve(type);

            return kernel.Resolve(type, constructorArguments);
        }

        public object GetInstance(Type serviceType, string key, IDictionary constructorArguments)
        {
            if (constructorArguments == null) return kernel.Resolve(key, serviceType);

            return kernel.Resolve(key, serviceType, constructorArguments);
        }
    }
}
