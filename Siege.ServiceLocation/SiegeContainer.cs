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

            this.AddBinding(typeof(IConditionalUseCaseBinding<>), this.serviceLocator.ConditionalUseCaseBinding);
            this.AddBinding(typeof(IDefaultUseCaseBinding<>), this.serviceLocator.DefaultUseCaseBinding);
            this.AddBinding(typeof(IDefaultInstanceUseCaseBinding<>), this.serviceLocator.DefaultInstanceUseCaseBinding);
            this.AddBinding(typeof(IKeyBasedUseCaseBinding<>), this.serviceLocator.KeyBasedUseCaseBinding);

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
            return (TService)GetInstance(type);
        }

        public IServiceLocator AddBinding(Type baseBinding, Type targetBinding)
        {
            this.serviceLocator.RegisterBinding(baseBinding, targetBinding);
            return this;
        }

        public object GetInstance(Type type)
        {
            IList<IUseCase> selectedCase = (IList<IUseCase>)useCases[type];

            if (selectedCase != null)
            {
                foreach (IUseCase useCase in selectedCase)
                {
                    object value = useCase.Resolve(serviceLocator, this.Context);

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

            Type bindingType = useCase.GetUseCaseBindingType().MakeGenericType(useCase.GetType().GetGenericArguments().First());

            var binding = GetInstance<IUseCaseBinding>(bindingType);

            binding.Bind(useCase);

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
            return GetInstance(serviceType);
        }

        public object GetInstance(Type type, string key)
        {
            return this.serviceLocator.GetInstance(type, key);
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