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

        public T GetInstance<T>()
        {
            return kernel.Get<T>();
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
            return (T)kernel.Get(type);
        }

        public T GetInstance<T>(Type type, IDictionary constructorArguments)
        {
            if (constructorArguments == null || constructorArguments.Count == 0) return (T)kernel.Get(type);
            
            List<ConstructorArgument> args = new List<ConstructorArgument>();
            
            foreach (string key in constructorArguments.Keys)
            {
                ConstructorArgument argument = new ConstructorArgument(key, constructorArguments[key]);
                args.Add(argument);
            }

            return kernel.Get<T>(args.ToArray());
        }

        public T GetInstance<T>(string key)
        {
            return kernel.Get<T>(key);
        }

        public T GetInstance<T>(string name, IDictionary constructorArguments)
        {
            if (constructorArguments == null || constructorArguments.Count == 0) return kernel.Get<T>(name);

            List<ConstructorArgument> args = new List<ConstructorArgument>();

            foreach (string key in constructorArguments.Keys)
            {
                ConstructorArgument argument = new ConstructorArgument(key, constructorArguments[key]);
                args.Add(argument);
            }

            return kernel.Get<T>(name, args.ToArray());
        }

        public IServiceLocator Register<T>(IUseCase<T> useCase)
        {
            if(useCase is IConditionalUseCase<T>)
            {
                var conditionalCase = useCase as IConditionalUseCase<T>;

                conditionalCase.Bind(kernel, this);
            }
            else if (useCase is IKeyBasedUseCase<T>)
            {
                var keyCase = useCase as IKeyBasedUseCase<T>;

                keyCase.Bind(kernel);
            }
            else if (useCase is DefaultImplementationUseCase<T>)
            {
                var implementation = useCase as DefaultImplementationUseCase<T>;

                implementation.Bind(kernel, this);
            }
            else if (useCase is IDefaultUseCase<T>)
            {
                IDefaultUseCase<T> genericCase = useCase as IDefaultUseCase<T>;

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
                        NinjectFactory<TBaseType> factory = new NinjectFactory<TBaseType>(locator, kernel);
                        Register(Given<NinjectFactory<TBaseType>>.Then("Factory" + typeof(TBaseType), factory));

                        factories.Add(typeof(TBaseType), factory);
                    }
                }
            }

            return (NinjectFactory<TBaseType>)factories[typeof(TBaseType)];
        }

        public void Dispose()
        {
            this.kernel.Dispose();
        }
    }
}