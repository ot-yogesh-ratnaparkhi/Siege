using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Siege.ServiceLocation;
using StructureMap;
using StructureMap.Attributes;
using StructureMap.Configuration.DSL;

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

        public void RegisterParentLocator(IContextualServiceLocator locator)
        {
            this.locator = locator;

            Registry registry = new Registry();

            registry.ForRequestedType<IServiceLocatorAdapter>().TheDefault.IsThis(this);
            registry.ForRequestedType<StructureMap.Container>().TheDefault.IsThis(container);

            container.Configure(x => x.AddRegistry(registry));

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
            Registry registry = new Registry();
            registry.ForRequestedType(baseBinding).CacheBy(InstanceScope.PerRequest).TheDefaultIsConcreteType(targetBinding);
            container.Configure(x => x.AddRegistry(registry));
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