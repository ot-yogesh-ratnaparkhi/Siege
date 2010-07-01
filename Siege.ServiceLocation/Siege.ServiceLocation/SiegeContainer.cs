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
using Siege.ServiceLocation.Bindings;
using Siege.ServiceLocation.Bindings.Action;
using Siege.ServiceLocation.Bindings.Conditional;
using Siege.ServiceLocation.Bindings.Default;
using Siege.ServiceLocation.Bindings.Named;
using Siege.ServiceLocation.Bindings.OpenGenerics;
using Siege.ServiceLocation.EventHandlers;
using Siege.ServiceLocation.Resolution;
using Siege.ServiceLocation.Stores;
using Siege.ServiceLocation.Stores.UseCases;
using Siege.ServiceLocation.Syntax;
using Siege.ServiceLocation.TypeBuilders;
using Siege.ServiceLocation.UseCases;
using Siege.ServiceLocation.UseCases.Actions;
using Siege.ServiceLocation.ExtensionMethods;
using Siege.ServiceLocation.UseCases.Default;
using Siege.ServiceLocation.UseCases.Managers;

namespace Siege.ServiceLocation
{
    public class SiegeContainer : IContextualServiceLocator
    {
        private IServiceLocatorAdapter serviceLocator;
        private IServiceLocatorStore store;
        private UseCaseStore useCaseStore = new UseCaseStore();
        private Hashtable factories = new Hashtable();
        private IResolver resolver;

        public SiegeContainer(IServiceLocatorAdapter serviceLocator, IServiceLocatorStore store, ITypeBuilder typeBuilder)
        {
            this.serviceLocator = serviceLocator;
            this.store = store;
            this.resolver = new Resolver(serviceLocator, this.store, this.useCaseStore);

            TypeHandler.Initialize(typeBuilder);

            serviceLocator.RegisterInstance(typeof(IServiceLocatorAdapter), serviceLocator);
            
            AddBinding(new DefaultUseCaseBinding(serviceLocator));
            AddBinding(new ConditionalUseCaseBinding(serviceLocator));
            AddBinding(new NamedUseCaseBinding(serviceLocator));
            AddBinding(new OpenGenericUseCaseBinding(serviceLocator));
            AddBinding(new ActionUseCaseBinding());

            InitializeUseCaseStore();

            Bind(Given<UseCaseStore>.Then(useCaseStore));
            Bind(Given<IResolver>.Then(resolver));

            Register(Given<IFactoryFetcher>.Then(this));
            Register(Given<IServiceLocator>.Then(this));
            Register(Given<IContextualServiceLocator>.Then(this));
        }

        public SiegeContainer(IServiceLocatorAdapter serviceLocator, IServiceLocatorStore store) : this(serviceLocator, store, new DefaultTypeBuilder())
        {
            
        }

        public void AddContext(object contextItem)
        {
            store.ContextStore.Add(contextItem);
        }

        private void AddBinding<TBinding>(TBinding instance)
        {
			serviceLocator.RegisterInstance(typeof(TBinding), instance);
        }

        public TService GetInstance<TService>()
        {
            return GetInstance<TService>(typeof (TService), new IResolutionArgument[] {});
        }

        public TService GetInstance<TService>(params IResolutionArgument[] arguments)
        {
            return GetInstance<TService>(typeof (TService), arguments);
        }

        public TService GetInstance<TService>(Type type, params IResolutionArgument[] arguments)
        {
            return (TService) GetInstance(type, arguments);
        }

        public object GetInstance(Type type)
        {
            return GetInstance(type, new IResolutionArgument[] {});
        }

        public object GetInstance(Type type, params IResolutionArgument[] arguments)
		{
            this.store.ResolutionStore.Add(new List<IResolutionArgument>(arguments));
            
            var instance = resolver.Resolve(type);

            return instance;
        }


        public TService GetInstance<TService>(string key)
        {
            return (TService) GetInstance(typeof (TService), key, new IResolutionArgument[] {});
        }

        public TService GetInstance<TService>(string key, params IResolutionArgument[] arguments)
        {
            return (TService) GetInstance(typeof (TService), key, arguments);
        }

        public object GetInstance(Type type, string key, params IResolutionArgument[] arguments)
        {
			this.store.ResolutionStore.Add(new List<IResolutionArgument>(arguments));

            return serviceLocator.GetInstance(type, key, this.store.ResolutionStore.Items.OfType<ConstructorParameter, IResolutionArgument>());
        }

        public object GetService(Type serviceType)
        {
            return GetInstance(serviceType, new IResolutionArgument[] {});
        }

        public object GetInstance(Type type, string key)
        {
            return GetInstance(type, key, new IResolutionArgument[] {});
        }

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return serviceLocator.GetAllInstances(serviceType);
        }

        public IEnumerable<TService> GetAllInstances<TService>()
        {
            return serviceLocator.GetAllInstances<TService>();
        }

        public bool HasTypeRegistered(Type type)
        {
            return serviceLocator.HasTypeRegistered(type);
        }

        public IServiceLocator Register(List<IUseCase> useCases)
        {
            foreach (IUseCase useCase in useCases) Register(useCase);

            return this;
        }

        public IServiceLocator Register(IUseCase useCase)
        {
            var caseStore = GetInstance<IUseCaseManager>(new ContextArgument(useCase));
            caseStore.Add(useCase);

            Bind(useCase);

            return this;
        }

        private void InitializeUseCaseStore()
        {
            var conditionalCases = Given<IUseCaseManager>.Then<ConditionalUseCaseManager>();
            var defaultCases = Given<IUseCaseManager>.When<IUseCase>(useCase => useCase is IDefaultUseCase).Then<DefaultUseCaseManager>();
            var defaultActionCases = Given<IUseCaseManager>.When<IUseCase>(useCase => useCase is IDefaultActionUseCase).Then<DefaultActionUseCaseManager>();
            var conditionalActionCases = Given<IUseCaseManager>.When<IUseCase>(useCase => useCase is IConditionalActionUseCase).Then<ConditionalActionUseCaseManager>();

            useCaseStore.Conditional.ResolutionCases.Add(typeof(IUseCaseManager), defaultCases);
            useCaseStore.Default.ResolutionCases.Add(typeof(IUseCaseManager), conditionalCases);
            useCaseStore.Conditional.ResolutionCases.Add(typeof(IUseCaseManager), defaultActionCases);
            useCaseStore.Conditional.ResolutionCases.Add(typeof(IUseCaseManager), conditionalActionCases);

            Bind(conditionalCases);
            Bind(defaultCases);
            Bind(defaultActionCases);
            Bind(conditionalActionCases);
        }

        private void Bind(IUseCase useCase)
        {
            Type bindingType = useCase.GetUseCaseBindingType().IsGenericType ?
                useCase.GetUseCaseBindingType().MakeGenericType(useCase.GetBaseBindingType())
                : useCase.GetUseCaseBindingType();

            if (useCase is IInstanceUseCase)
            {
                var binding = GetInstance<IInstanceUseCaseBinding>(bindingType);
                binding.BindInstance((IInstanceUseCase)useCase, this);
            }
            else
            {
                var binding = GetInstance<IUseCaseBinding>(bindingType);
                binding.Bind(useCase, this);
            }
        }

        public IServiceLocatorStore Store
        {
            get { return this.store; }
        }

        public void Dispose()
        {
            serviceLocator.Dispose();
            this.store.Dispose();
        }

        IGenericFactory IFactoryFetcher.GetFactory(Type type)
        {
            if (!factories.ContainsKey(type))
            {
                lock (factories.SyncRoot)
                {
                    if (!factories.ContainsKey(type))
                    {
                        var factory = new Factory(this);
                        Bind(Given<Factory>.Then("Factory" + type, factory));

                        factories.Add(type, factory);
                    }
                }
            }

            return (Factory) factories[type];
        }
    }
}