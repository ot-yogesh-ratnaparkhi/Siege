using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Builder;

namespace Siege.ServiceLocation.AutofacAdapter
{
    public class AutofacAdapter : IServiceLocatorAdapter
    {
        private IContainer container;
        private IContextualServiceLocator locator;
        private readonly Hashtable factories = new Hashtable();

        public AutofacAdapter(IContainer container)
        {
            this.container = container;
        }

        public AutofacAdapter() : this(new Container())
        {
            
        }
        public void Dispose()
        {
            container.Dispose();
        }

        public object GetInstance(Type type, string key)
        {
            return container.Resolve(key);
        }

        public object GetInstance(Type type)
        {
            return container.Resolve(type);
        }

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            var type = typeof(IEnumerable<>).MakeGenericType(serviceType);
            object instance;
            if (container.TryResolve(type, out instance))
            {
                return ((IEnumerable)instance).Cast<object>();
            }

            return new List<object>();
        }

        public IEnumerable<TService> GetAllInstances<TService>()
        {
            var type = typeof(IEnumerable<>).MakeGenericType(typeof(TService));
            object instance;
            if (container.TryResolve(type, out instance))
            {
                return ((IEnumerable)instance).Cast<TService>();
            }

            return new List<TService>();
        }

        public void RegisterParentLocator(IContextualServiceLocator locator)
        {
            this.locator = locator;

            var builder = new ContainerBuilder();

            builder.Register<IServiceLocatorAdapter>(c => this);
            builder.Register<IServiceLocator>(c => locator);
            builder.Register(c => locator);

            builder.Build(container);
        }

        public IGenericFactory<TBaseService> GetFactory<TBaseService>()
        {
            if (!factories.ContainsKey(typeof(TBaseService)))
            {
                lock (factories.SyncRoot)
                {
                    if (!factories.ContainsKey(typeof(TBaseService)))
                    {
                        Factory<TBaseService> factory = new Factory<TBaseService>(locator);
                        locator.Register(Given<Factory<TBaseService>>.Then("Factory" + typeof(TBaseService), factory));

                        factories.Add(typeof(TBaseService), factory);
                    }
                }
            }

            return (Factory<TBaseService>)factories[typeof(TBaseService)];
        }

        public void RegisterBinding(Type baseBinding, Type targetBinding)
        {
            var builder = new ContainerBuilder();
            builder.RegisterGeneric(targetBinding).As(baseBinding);
            builder.Build(container);
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
    }
}
