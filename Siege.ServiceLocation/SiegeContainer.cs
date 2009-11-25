using System;
using System.Collections;
using System.Collections.Generic;

namespace Siege.ServiceLocation
{
    public class SiegeContainer : IContextualServiceLocator
    {
        private readonly IServiceLocatorAdapter serviceLocator;
        private readonly IContextStore contextStore;
        private readonly Hashtable useCases = new Hashtable();
        private readonly Hashtable registeredImplementors = new Hashtable();
        private readonly Hashtable registeredTypes = new Hashtable();
        private readonly Hashtable defaultCases = new Hashtable();

        public SiegeContainer(IServiceLocatorAdapter serviceLocator, IContextStore contextStore)
        {
            this.serviceLocator = serviceLocator;
            this.contextStore = contextStore;
            this.serviceLocator.RegisterParentLocator(this);
        }

        public SiegeContainer(IServiceLocatorAdapter serviceLocator) : this(serviceLocator, new GlobalContextStore())
        {
            
        }

        public void AddContext(object contextItem)
        {
            this.contextStore.Add(contextItem);
        }

        public TService GetInstance<TService>()
        {
            return GetInstance<TService>(typeof(TService));
        }

        public TService GetInstance<TService>(Type type)
        {
            return GetInstance<TService>(type, null);
        }

        public TService GetInstance<TService>(IDictionary constructorArguments)
        {
            return GetInstance<TService>(typeof(TService), constructorArguments);
        }

        public TService GetInstance<TService>(object anonymousConstructorArguments)
        {
            return GetInstance<TService>(anonymousConstructorArguments.AnonymousTypeToDictionary());
        }

        public TService GetInstance<TService>(Type type, IDictionary constructorArguments)
        {
            return (TService)GetInstance(type, constructorArguments);
        }

        public object GetInstance(Type type, IDictionary constructorArguments)
        {
            IList<IUseCase> selectedCase = (IList<IUseCase>)useCases[type];

            if (selectedCase != null)
            {
                foreach (IUseCase useCase in selectedCase)
                {
                    object value = useCase.Resolve(serviceLocator, this.Context, constructorArguments);

                    if (value != null) return value;
                }
            }

            if (defaultCases.ContainsKey(type))
            {
                IDefaultUseCase useCase = (IDefaultUseCase)defaultCases[type];
                return serviceLocator.GetInstance(useCase.GetBoundType(), constructorArguments);
            }

            return serviceLocator.GetInstance(type, constructorArguments);
        }

        public TService GetInstance<TService>(string key)
        {
            return GetInstance<TService>(key, null);
        }

        public TService GetInstance<TService>(string key, IDictionary constructorArguments)
        {
            return serviceLocator.GetInstance<TService>(key, constructorArguments);
        }

        public object GetInstance(Type serviceType, string key, IDictionary constructorArguments)
        {
            return this.serviceLocator.GetInstance(serviceType, key, constructorArguments);
        }

        public IMinimalServiceLocator Register<TService>(IUseCase<TService> useCase)
        {
            if (useCase is IDefaultUseCase<TService>)
            {
                defaultCases.Add(typeof(TService), useCase);
            }
            else
            {
                if (!useCases.ContainsKey(typeof(TService)))
                {
                    List<IUseCase> list = new List<IUseCase>();

                    useCases.Add(typeof(TService), list);
                }

                IList<IUseCase> selectedCase = (IList<IUseCase>)useCases[typeof(TService)];

                selectedCase.Add(useCase);
            }

            if (!registeredTypes.ContainsKey(typeof(TService))) registeredTypes.Add(typeof(TService), typeof(TService));
            if (!registeredImplementors.ContainsKey(useCase.GetType())) registeredImplementors.Add(useCase.GetType(), useCase.GetType());

            serviceLocator.Register(useCase);

            return this;
        }

        public IList<object> Context
        {
            get { return this.contextStore.Items; }
        }

        public void Dispose()
        {
            this.serviceLocator.Dispose();
        }

        public object GetService(Type serviceType)
        {
            return this.serviceLocator.GetInstance(serviceType, null);
        }

        public object GetInstance(Type serviceType)
        {
            return GetInstance(serviceType, (IDictionary)null);
        }

        public object GetInstance(Type serviceType, string key)
        {
            return GetInstance(serviceType, key, null);
        }

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return this.serviceLocator.GetAllInstances(serviceType);
        }

        public IEnumerable<TService> GetAllInstances<TService>()
        {
            return this.serviceLocator.GetAllInstances<TService>();
        }
    }
}