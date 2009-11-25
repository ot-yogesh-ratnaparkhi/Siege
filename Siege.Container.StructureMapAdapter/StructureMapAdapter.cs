using System;
using System.Collections;
using System.Collections.Generic;
using Siege.ServiceLocation;
using StructureMap;

namespace Siege.Container.StructureMapAdapter
{
    public class StructureMapAdapter : IServiceLocatorAdapter
    {
        private readonly StructureMap.Container container;
        private readonly Hashtable factories = new Hashtable();
        private IContextualServiceLocator locator;

        public StructureMapAdapter() : this(new StructureMap.Container(x => x.IncludeConfigurationFromConfigFile = true)) {}
        public StructureMapAdapter(StructureMap.Container container)
        {
            this.container = container;
        }

        public TService GetInstance<TService>()
        {
            return (TService)GetInstance(typeof(TService));
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
            return (TService)GetInstance(type);
        }

        public TService GetInstance<TService>(Type type, IDictionary constructorArguments)
        {
            return (TService) GetInstance(type, constructorArguments);
        }

        public TService GetInstance<TService>(string key)
        {
            return GetInstance<TService>(key, null);
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

                conditionalCase.Bind(container, this);
            }
            else if (useCase is IKeyBasedUseCase<TService>)
            {
                var keyCase = useCase as IKeyBasedUseCase<TService>;

                keyCase.Bind(container);
            }
            else if (useCase is DefaultInstanceUseCase<TService>)
            {
                var implementation = useCase as DefaultInstanceUseCase<TService>;

                implementation.Bind(container, this);
            }
            else if (useCase is IDefaultUseCase<TService>)
            {
                var genericCase = useCase as IDefaultUseCase<TService>;

                genericCase.Bind(container, this);
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
        }


        public object GetInstance(Type serviceType)
        {
            return container.GetInstance(serviceType);
        }

        public object GetInstance(Type serviceType, string key)
        {
            return GetInstance(serviceType, key, null);
        }

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return (IEnumerable<object>)container.GetAllInstances(serviceType);
        }

        public IEnumerable<TService> GetAllInstances<TService>()
        {
            return container.GetAllInstances<TService>();
        }

        public object GetInstance(Type type, IDictionary constructorArguments)
        {
            if (constructorArguments == null || constructorArguments.Count == 0) return container.GetInstance(type);

            ExplicitArgsExpression expression = null;

            foreach (string key in constructorArguments.Keys)
            {
                if (expression == null)
                {
                    expression = container.With(key).EqualTo(constructorArguments[key]);
                    continue;
                }

                expression.With(key).EqualTo(constructorArguments[key]);
            }

            return expression.GetInstance(type);
        }

        public object GetInstance(Type serviceType, string key, IDictionary constructorArguments)
        {
            if (constructorArguments == null || constructorArguments.Count == 0) return container.GetInstance(serviceType, key);

            ExplicitArgsExpression expression = null;

            foreach (string name in constructorArguments.Keys)
            {
                if (expression == null)
                {
                    expression = container.With(name).EqualTo(constructorArguments[name]);
                    continue;
                }

                expression.With(key).EqualTo(constructorArguments[key]);
            }

            return expression.GetInstance(serviceType);
        }

        public object GetService(Type serviceType)
        {
            return container.GetInstance(serviceType);
        }
    }
}