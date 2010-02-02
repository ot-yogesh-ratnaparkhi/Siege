/*   Copyright 2009 - 2010 Marcus Bratton

     Licensed under the Apache License, Version 2.0 (the "License");
     you may not use this file except in compliance with the License.
     You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

     Unless required by applicable law or agreed to in writing, software
     distributed under the License is distributed on an "AS IS" BASIS,
     WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
     See the License for the specific language governing permissions and
     limitations under the License.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Siege.ServiceLocation.Bindings;
using Siege.ServiceLocation.Exceptions;
using Siege.ServiceLocation.Stores;
using Siege.ServiceLocation.Syntax;
using Siege.ServiceLocation.TypeBuilders;
using Siege.ServiceLocation.UseCases;
using Siege.ServiceLocation.UseCases.Actions;
using Siege.ServiceLocation.UseCases.Conditional;
using Siege.ServiceLocation.UseCases.Default;

namespace Siege.ServiceLocation
{
    public class SiegeContainer : IContextualServiceLocator
    {
        private IServiceLocatorAdapter serviceLocator;
        private IContextStore contextStore;
        private IExecutionStore executionStore;
        private readonly Hashtable useCases = new Hashtable();
        private readonly Hashtable registeredImplementors = new Hashtable();
        private readonly Hashtable registeredTypes = new Hashtable();
        private readonly Hashtable defaultCases = new Hashtable();
        private readonly Hashtable factories = new Hashtable();
        private readonly Hashtable defaultActionUseCases = new Hashtable();
        private readonly Hashtable conditionalActionUseCases = new Hashtable();

        protected SiegeContainer(IServiceLocatorAdapter serviceLocator, IContextStore contextStore, IExecutionStore executionStore, ITypeBuilder typeBuilder)
        {
            this.serviceLocator = serviceLocator;
            this.contextStore = contextStore;
            this.executionStore = executionStore;
            TypeHandler.Initialize(typeBuilder);
            
            AddBinding(typeof(IActionUseCaseBinding<>), typeof(ActionUseCaseBinding<>));
            AddBinding(typeof(IConditionalUseCaseBinding<>), this.serviceLocator.ConditionalUseCaseBinding);
            AddBinding(typeof(IDefaultUseCaseBinding<>), this.serviceLocator.DefaultUseCaseBinding);
            AddBinding(typeof(IKeyBasedUseCaseBinding<>), this.serviceLocator.KeyBasedUseCaseBinding);

            Register(Given<IFactoryFetcher>.Then(this));
            Register(Given<IServiceLocator>.Then(this));
            Register(Given<IContextualServiceLocator>.Then(this));
            Register(Given<IServiceLocatorAdapter>.Then(serviceLocator));
        }

        public SiegeContainer(IServiceLocatorAdapter serviceLocator, IContextStore contextStore, ITypeBuilder typeBuilder) : this(serviceLocator, contextStore, ThreadedExecutionStore.New(), typeBuilder)
        {
            
        }

        public SiegeContainer(IServiceLocatorAdapter serviceLocator, ITypeBuilder typeBuilder) : this(serviceLocator, new GlobalContextStore(), typeBuilder)
        {
            
        }

        public SiegeContainer(IServiceLocatorAdapter serviceLocator, IContextStore contextStore)
            : this(serviceLocator, contextStore, ThreadedExecutionStore.New(), new DefaultTypeBuilder())
        {

        }

        public SiegeContainer(IServiceLocatorAdapter serviceLocator)
            : this(serviceLocator, new GlobalContextStore(), new DefaultTypeBuilder())
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
                    object value = useCase.Resolve(new ConditionalResolutionStrategy(serviceLocator, this), this);

                    if (value != null)
                    {
                        executionStore.Decrement();
                        this.executionStore = executionStore.Create();

                        IList<IUseCase> defaultActions = (IList<IUseCase>)defaultActionUseCases[useCase.GetBoundType()];

                        if (defaultActions != null)
                        {
                            foreach (IDefaultActionUseCase actionUseCase in defaultActions)
                            {
                                actionUseCase.Invoke(value);
                            }
                        }

                        IList<IUseCase> conditionalActions = (IList<IUseCase>)conditionalActionUseCases[useCase.GetBoundType()];

                        if (conditionalActions != null)
                        {
                            foreach (IConditionalActionUseCase actionUseCase in conditionalActions)
                            {
                                if (actionUseCase.IsValid(this)) actionUseCase.Invoke(value);
                            }
                        }
                        
                        return value;
                    }
                }
            }

            if (defaultCases.ContainsKey(type))
            {
                IDefaultUseCase useCase = (IDefaultUseCase)defaultCases[type];
                var value = useCase.Resolve(new DefaultResolutionStrategy(serviceLocator, this), this); 
                executionStore.Decrement();
                this.executionStore = executionStore.Create();

                IList<IUseCase> defaultActions = (IList<IUseCase>)defaultActionUseCases[useCase.GetBoundType()];

                if (defaultActions != null)
                {
                    foreach (IDefaultActionUseCase actionUseCase in defaultActions)
                    {
                        actionUseCase.Invoke(value);
                    }
                }

                IList<IUseCase> conditionalActions = (IList<IUseCase>)conditionalActionUseCases[useCase.GetBoundType()];

                if (conditionalActions != null)
                {
                    foreach (IConditionalActionUseCase actionUseCase in conditionalActions)
                    {
                        if (actionUseCase.IsValid(this)) actionUseCase.Invoke(value);
                    }
                }

                return value;
            }

            if(HasTypeRegistered(type))
            {
                return serviceLocator.GetInstance(type);
            }

            throw new RegistrationNotFoundException(type);
        }

        public bool HasTypeRegistered(Type type)
        {
            return serviceLocator.HasTypeRegistered(type);
        }

        public TService GetInstance<TService>(string key)
        {
            return (TService)GetInstance(typeof(TService), key);
        }

        public IServiceLocator Register(IUseCase useCase)
        {
            if(useCase is IDefaultActionUseCase)
            {
                if (!defaultActionUseCases.ContainsKey(useCase.GetBoundType()))
                {
                    List<IUseCase> list = new List<IUseCase>();
                    defaultActionUseCases.Add(useCase.GetBoundType(), list);
                }

                IList<IUseCase> selectedCase = (IList<IUseCase>)defaultActionUseCases[useCase.GetBoundType()];

                selectedCase.Add(useCase);
            }

            if (useCase is IConditionalActionUseCase)
            {
                if (!conditionalActionUseCases.ContainsKey(useCase.GetBoundType()))
                {
                    List<IUseCase> list = new List<IUseCase>();
                    conditionalActionUseCases.Add(useCase.GetBoundType(), list);
                }

                IList<IUseCase> selectedCase = (IList<IUseCase>)conditionalActionUseCases[useCase.GetBoundType()];

                selectedCase.Add(useCase);
            }

            if (useCase is IDefaultUseCase)
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

            if(useCase is IInstanceUseCase)
            {
                binding.BindInstance((IInstanceUseCase)useCase, this);
            }
            else
            {
                binding.Bind(useCase, this);
            }

            return this;
        }

        public IList<object> Context
        {
            get { return contextStore.Items; }
        }

        public IContextStore ContextStore
        {
            get { return contextStore; }
        }

        public IExecutionStore ExecutionStore
        {
            get { return executionStore; }
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
            this.executionStore = executionStore.Create();
            return serviceLocator.GetInstance(type, key);
        }

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            this.executionStore = executionStore.Create();
            return serviceLocator.GetAllInstances(serviceType);
        }

        public IEnumerable<TService> GetAllInstances<TService>()
        {
            return serviceLocator.GetAllInstances<TService>();
        }

        IGenericFactory<TBaseService> IFactoryFetcher.GetFactory<TBaseService>()
        {
            if (!factories.ContainsKey(typeof(TBaseService)))
            {
                lock (factories.SyncRoot)
                {
                    if (!factories.ContainsKey(typeof(TBaseService)))
                    {
                        Factory<TBaseService> factory = new Factory<TBaseService>(this);
                        Register(Given<Factory<TBaseService>>.Then("Factory" + typeof(TBaseService), factory));

                        factories.Add(typeof(TBaseService), factory);
                    }
                }
            }

            return (Factory<TBaseService>)factories[typeof(TBaseService)];
        }
    }
}