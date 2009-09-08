using System;
using System.Collections;
using System.Collections.Generic;
using Siege.ServiceLocation;

namespace Siege.Container
{
    public class SiegeContainer : IContextualServiceLocator
    {
        private readonly IServiceLocatorAdapter serviceLocator;
        private readonly IContextStore contextStore;
        private readonly Hashtable useCases = new Hashtable();
        private readonly Hashtable registeredImplementors = new Hashtable();
        private readonly Hashtable registeredTypes = new Hashtable();
        private readonly Hashtable defaultCases = new Hashtable();
        private readonly Hashtable conditionalFactories = new Hashtable();

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

        public ConditionalFactory<TBaseType> GetConditionalFactory<TBaseType>()
        {
            if(!conditionalFactories.ContainsKey(typeof(TBaseType)))
            {
                lock(conditionalFactories.SyncRoot)
                {
                    if(!conditionalFactories.ContainsKey(typeof(TBaseType)))
                    {
                        ConditionalFactory<TBaseType> factory = new ConditionalFactory<TBaseType>(this);
                        Register(Given<ConditionalFactory<TBaseType>>.Then("conditionalFactory"+typeof(TBaseType), factory));

                        conditionalFactories.Add(typeof(TBaseType), factory);
                    }
                }
            }

            return (ConditionalFactory<TBaseType>) conditionalFactories[typeof (TBaseType)];
        }

        public TOutput GetInstance<TOutput>()
        {
            return GetInstance<TOutput>(typeof(TOutput));
        }

        public TOutput GetInstance<TOutput>(Type type)
        {
            return GetInstance<TOutput>(type, null);
        }

        public TOutput GetInstance<TOutput>(IDictionary constructorArguments)
        {
            return GetInstance<TOutput>(typeof(TOutput), constructorArguments);
        }

        public T GetInstance<T>(object anonymousConstructorArguments)
        {
            return GetInstance<T>(anonymousConstructorArguments.AnonymousTypeToDictionary());
        }

        public TOutput GetInstance<TOutput>(Type type, IDictionary constructorArguments)
        {
            IList<IUseCase> selectedCase = (IList<IUseCase>)useCases[type];

            if (selectedCase != null)
            {
                foreach (IUseCase<TOutput> useCase in selectedCase)
                {
                    TOutput value = useCase.Resolve(serviceLocator, this.Context, constructorArguments);

                    if (!Equals(value, default(TOutput))) return value;
                }
            }

            if (defaultCases.ContainsKey(type))
            {
                IDefaultUseCase useCase = (IDefaultUseCase)defaultCases[type];
                return serviceLocator.GetInstance<TOutput>(useCase.GetBoundType(), constructorArguments);
            }

            return serviceLocator.GetInstance<TOutput>(type, constructorArguments);
        }

        public T GetInstance<T>(string key)
        {
            return GetInstance<T>(key, null);
        }

        public T GetInstance<T>(string key, IDictionary constructorArguments)
        {
            return serviceLocator.GetInstance<T>(key, constructorArguments);
        }

        public IServiceLocator Register<T>(IUseCase<T> useCase)
        {
            if (useCase is IDefaultUseCase<T>)
            {
                defaultCases.Add(typeof(T), useCase);
            }
            else
            {
                if (!useCases.ContainsKey(typeof(T)))
                {
                    List<IUseCase> list = new List<IUseCase>();

                    useCases.Add(typeof(T), list);
                }

                IList<IUseCase> selectedCase = (IList<IUseCase>)useCases[typeof(T)];

                selectedCase.Add(useCase);
            }

            if (!registeredTypes.ContainsKey(typeof(T))) registeredTypes.Add(typeof(T), typeof(T));
            if (!registeredImplementors.ContainsKey(useCase.GetType())) registeredImplementors.Add(useCase.GetType(), useCase.GetType());

            serviceLocator.Register(useCase);

            return this;
        }

        public IList<object> Context
        {
            get { return this.contextStore.Items; }
        }
    }
}