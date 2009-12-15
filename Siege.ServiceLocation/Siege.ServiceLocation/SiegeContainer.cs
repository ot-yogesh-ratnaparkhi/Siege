using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

            AddBinding(typeof(IConditionalUseCaseBinding<>), this.serviceLocator.ConditionalUseCaseBinding);
            AddBinding(typeof(IDefaultUseCaseBinding<>), this.serviceLocator.DefaultUseCaseBinding);
            AddBinding(typeof(IDefaultInstanceUseCaseBinding<>), this.serviceLocator.DefaultInstanceUseCaseBinding);
            AddBinding(typeof(IKeyBasedUseCaseBinding<>), this.serviceLocator.KeyBasedUseCaseBinding);

            this.serviceLocator.RegisterParentLocator(this);
        }

        public SiegeContainer(IServiceLocatorAdapter serviceLocator) : this(serviceLocator, new GlobalContextStore())
        {
            
        }

        public void AddContext(object contextItem)
        {
            contextStore.Add(contextItem);
        }

        public TService GetInstance<TService>()
        {
            return GetInstance<TService>(typeof(TService));
        }

        public TService GetInstance<TService>(Type type)
        {
            return (TService)GetInstance(type);
        }

        public IServiceLocator AddBinding(Type baseBinding, Type targetBinding)
        {
            serviceLocator.RegisterBinding(baseBinding, targetBinding);
            return this;
        }

        public object GetInstance(Type type)
        {
            IList<IUseCase> selectedCase = (IList<IUseCase>)useCases[type];

            if (selectedCase != null)
            {
                foreach (IUseCase useCase in selectedCase)
                {
                    object value = useCase.Resolve(serviceLocator, Context);

                    if (value != null) return value;
                }
            }

            if (defaultCases.ContainsKey(type))
            {
                IDefaultUseCase useCase = (IDefaultUseCase)defaultCases[type];
                return serviceLocator.GetInstance(useCase.GetBoundType());
            }

            return serviceLocator.GetInstance(type);
        }

        public TService GetInstance<TService>(string key)
        {
            return (TService)GetInstance(typeof(TService), key);
        }

        public IServiceLocator Register<TService>(IUseCase<TService> useCase)
        {
            if (useCase is IDefaultUseCase<TService>)
            {
                if (!defaultCases.ContainsKey(useCase.GetBaseBindingType())) defaultCases.Add(useCase.GetBaseBindingType(), useCase);
            }
            else
            {
                if (!useCases.ContainsKey(useCase.GetBaseBindingType()))
                {
                    List<IUseCase> list = new List<IUseCase>();

                    useCases.Add(useCase.GetBaseBindingType(), list);
                }

                IList<IUseCase> selectedCase = (IList<IUseCase>)useCases[useCase.GetBaseBindingType()];

                selectedCase.Add(useCase);
            }

            if (!registeredTypes.ContainsKey(useCase.GetBaseBindingType())) registeredTypes.Add(useCase.GetBaseBindingType(), useCase.GetBaseBindingType());
            if (!registeredImplementors.ContainsKey(useCase.GetType())) registeredImplementors.Add(useCase.GetType(), useCase.GetType());

            Type bindingType = useCase.GetUseCaseBindingType().MakeGenericType(useCase.GetType().GetGenericArguments().First());

            var binding = GetInstance<IUseCaseBinding>(bindingType);

            binding.Bind(useCase);

            return this;
        }

        public IList<object> Context
        {
            get { return contextStore.Items; }
        }

        public IContextStore ContextStore
        {
            get { return this.contextStore; }
        }

        public IList<IUseCase> GetRegisteredUseCasesForType(Type type)
        {
            return (IList<IUseCase>)useCases[type];
        }

        public void Dispose()
        {
            serviceLocator.Dispose();
        }

        public object GetService(Type serviceType)
        {
            return GetInstance(serviceType);
        }

        public object GetInstance(Type type, string key)
        {
            return serviceLocator.GetInstance(type, key);
        }

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return serviceLocator.GetAllInstances(serviceType);
        }

        public IEnumerable<TService> GetAllInstances<TService>()
        {
            return serviceLocator.GetAllInstances<TService>();
        }
    }
}