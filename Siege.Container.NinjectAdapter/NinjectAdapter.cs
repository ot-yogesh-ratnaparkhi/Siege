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
        public NinjectAdapter(IKernel iKernel)
        {
            kernel = iKernel;
        }

        public TService GetInstance<TService>()
        {
            return kernel.Get<TService>();
        }

        public TService GetInstance<TService>(IDictionary constructorArguments)
        {
            return GetInstance<TService>(typeof(TService), constructorArguments);
        }

        public TService GetInstance<TService>(object anonymousConstructorArguments)
        {
            return GetInstance<TService>(anonymousConstructorArguments.AnonymousTypeToDictionary());
        }

        public TService GetInstance<TService>(Type type)
        {
            return (TService) GetInstance(type);
        }

        public TService GetInstance<TService>(Type type, IDictionary constructorArguments)
        {
            return (TService) GetInstance(type, constructorArguments);
        }

        public T GetInstance<T>(string key)
        {
            return kernel.Get<T>(key);
        }

        public TService GetInstance<TService>(string name, IDictionary constructorArguments)
        {
            return (TService) GetInstance(typeof(TService), name, constructorArguments);
        }

        public IServiceLocator Register<TService>(IUseCase<TService> useCase)
        {
            if (useCase is IConditionalUseCase<TService>)
            {
                var conditionalCase = useCase as IConditionalUseCase<TService>;

                conditionalCase.Bind(kernel, this);
            }
            else if (useCase is IKeyBasedUseCase<TService>)
            {
                var keyCase = useCase as IKeyBasedUseCase<TService>;

                keyCase.Bind(kernel);
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

            Register(Given<IServiceLocator>.Then(locator));
            Register(Given<IContextualServiceLocator>.Then(locator));
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

        public object GetInstance(Type serviceType)
        {
            return kernel.Get(serviceType);
        }

        public object GetInstance(Type serviceType, string key)
        {
            return GetInstance(serviceType, key, null);
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

        public object GetService(Type serviceType)
        {
            return kernel.GetService(serviceType);
        }
    }
}