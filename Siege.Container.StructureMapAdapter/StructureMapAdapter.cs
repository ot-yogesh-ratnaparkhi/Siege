using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Siege.ServiceLocation;
using StructureMap;

namespace Siege.Container.StructureMapAdapter
{
    public class StructureMapAdapter : IServiceLocatorAdapter
    {
        private StructureMap.Container container;
        private Hashtable factories = new Hashtable();
        private IContextualServiceLocator locator;

        public StructureMapAdapter() : this(new StructureMap.Container(x => x.IncludeConfigurationFromConfigFile = true)) {}
        public StructureMapAdapter(StructureMap.Container container)
        {
            this.container = container;
        }

        public TService GetInstance<TService>()
        {
            return container.GetInstance<TService>();
        }

        public TService GetInstance<TService>(string name, IDictionary constructorArguments)
        {
            return (TService) GetInstance(typeof(TService), name, constructorArguments);
        }

        public IMinimalServiceLocator Register<TService>(IUseCase<TService> useCase)
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

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            Collection<object> objects = new Collection<object>();

            foreach (object item in container.GetAllInstances(serviceType))
            {
                objects.Add(item);
            }

            return objects;
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
    }
}