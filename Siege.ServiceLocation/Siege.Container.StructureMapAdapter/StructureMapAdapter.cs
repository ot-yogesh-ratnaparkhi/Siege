using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using StructureMap.Attributes;
using StructureMap.Configuration.DSL;

namespace Siege.ServiceLocation.StructureMapAdapter
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

        public object GetInstance(Type type)
        {
            return container.GetInstance(type);
        }

        public object GetInstance(Type serviceType, string key)
        {
            return container.GetInstance(serviceType, key);
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