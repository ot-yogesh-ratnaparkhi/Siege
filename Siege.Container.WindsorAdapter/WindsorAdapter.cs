using System;
using System.Collections;
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

        public T GetInstance<T>()
        {
            return kernel.Resolve<T>();
        }

        public T GetInstance<T>(IDictionary constructorArguments)
        {
            return GetInstance<T>(typeof (T), constructorArguments);
        }

        public T GetInstance<T>(object anonymousConstructorArguments)
        {
            return GetInstance<T>(anonymousConstructorArguments.AnonymousTypeToDictionary());
        }

        public T GetInstance<T>(Type type)
        {
            return (T)kernel.Resolve(type);
        }

        public T GetInstance<T>(Type type, IDictionary constructorArguments)
        {
            if (constructorArguments == null) return (T) kernel.Resolve(type);
            return (T)kernel.Resolve(type, constructorArguments);
        }

        public T GetInstance<T>(string key)
        {
            return GetInstance<T>(key, null);
        }

        public T GetInstance<T>(string key, IDictionary constructorArguments)
        {
            if (constructorArguments == null) return kernel.Resolve<T>(key);

            return (T)kernel.Resolve(key, constructorArguments);
        }

        public IServiceLocator Register<T>(IUseCase<T> useCase)
        {
            if(useCase is IConditionalUseCase<T>)
            {
                var conditionalCase = useCase as IConditionalUseCase<T>;

                conditionalCase.Bind(this.kernel, this);
            }
            else if (useCase is KeyBasedImplementationUseCase<T>)
            {
                var keyCase = useCase as KeyBasedImplementationUseCase<T>;

                keyCase.Bind(this.kernel);
            }
            else if (useCase is KeyBasedUseCase<T>)
            {
                var keyCase = useCase as KeyBasedUseCase<T>;

                keyCase.Bind(this.kernel);
            }
            else if (useCase is IDefaultUseCase<T>)
            {
                IDefaultUseCase<T> genericCase = useCase as IDefaultUseCase<T>;

                genericCase.Bind(kernel, this);
            }
            else if (useCase is ImplementationUseCase<T>)
            {
                var implementation = useCase as ImplementationUseCase<T>;

                implementation.Bind(kernel);
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
                        WindsorFactory<TBaseType> factory = new WindsorFactory<TBaseType>(this.locator);
                        Register(Given<WindsorFactory<TBaseType>>.Then("Factory" + typeof(TBaseType), factory));

                        factories.Add(typeof(TBaseType), factory);
                    }
                }
            }

            return (WindsorFactory<TBaseType>)factories[typeof(TBaseType)];
        }

        public void Dispose()
        {
            this.kernel.Dispose();
        }
    }
}
