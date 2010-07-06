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
using Siege.ServiceLocation.Bindings.PostResolution;
using Siege.ServiceLocation.Bindings.Conditional;
using Siege.ServiceLocation.Bindings.Default;
using Siege.ServiceLocation.Bindings.Named;
using Siege.ServiceLocation.Bindings.OpenGenerics;
using Siege.ServiceLocation.Bindings.Registration;
using Siege.ServiceLocation.Policies;
using Siege.ServiceLocation.Resolution;
using Siege.ServiceLocation.Stores;
using Siege.ServiceLocation.Stores.UseCases;
using Siege.ServiceLocation.Syntax;
using Siege.ServiceLocation.TypeBuilders;
using Siege.ServiceLocation.UseCases;
using Siege.ServiceLocation.ExtensionMethods;
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
            AddBinding(new ConditionalPostResolutionUseCaseBinding());
            AddBinding(new DefaultPostResolutionUseCaseBinding());

            InitializeUseCaseStore();
            
            Bind(Given<UseCaseStore>.Then(useCaseStore));
            Bind(Given<IResolver>.Then(resolver));
            
            RegisterPolicy(Given<Transient>.Then<Transient>());
            RegisterPolicy(Given<Singleton>.Then<Singleton>());

            Register<Singleton>(Given<IFactoryFetcher>.Then(this));
            Register<Singleton>(Given<IServiceLocator>.Then(this));
            Register<Singleton>(Given<IContextualServiceLocator>.Then(this));
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
            return Register<Transient>(useCases);
        }

        public IServiceLocator Register<TRegistrationPolicy>(List<IUseCase> useCases) where TRegistrationPolicy : IRegistrationPolicy
        {
            foreach (IUseCase useCase in useCases) Register<TRegistrationPolicy>(useCase);

            return this;
        }

        public IServiceLocator Register<TRegistrationPolicy>(IUseCase useCase) where TRegistrationPolicy : IRegistrationPolicy
        {
            var caseStore = GetInstance<IUseCaseManager>(new ContextArgument(useCase));
            var policy = GetInstance<TRegistrationPolicy>(new ContextArgument(useCase), new ConstructorParameter { Name = "useCase", Value = useCase });
            
            caseStore.Add(policy);
            Bind(policy);

            return this;
        }

        private void RegisterPolicy(IUseCase useCase)
        {
            var caseStore = GetInstance<IUseCaseManager>(new ContextArgument(useCase));
            caseStore.Add(useCase);

            Bind(useCase);
        }

        public IServiceLocator Register(IUseCase useCase)
        {
            return Register<Transient>(useCase);
        }

        private void InitializeUseCaseStore()
        {
            var conditionalCases = Given<IUseCaseManager>.Then<ConditionalUseCaseManager>();
            var defaultCases = Given<IUseCaseManager>.When<IUseCase>(useCase => useCase.GetUseCaseBindingType() == typeof(DefaultUseCaseBinding)).Then<DefaultUseCaseManager>();
            var namedCases = Given<IUseCaseManager>.When<IUseCase>(useCase => useCase.GetUseCaseBindingType() == typeof(NamedUseCaseBinding)).Then<ConditionalUseCaseManager>();
            var defaultActionCases = Given<IUseCaseManager>.When<IUseCase>(useCase => useCase.GetUseCaseBindingType() == typeof(DefaultPostResolutionUseCaseBinding)).Then<DefaultPostResolutionUseCaseManager>();
            var conditionalActionCases = Given<IUseCaseManager>.When<IUseCase>(useCase => useCase.GetUseCaseBindingType() == typeof(ConditionalPostResolutionUseCaseBinding)).Then<ConditionalPostResolutionUseCaseManager>();
            var conditionalRegistrationCases = Given<IUseCaseManager>.When<IUseCase>(useCase => useCase.GetUseCaseBindingType() == typeof(ConditionalRegistrationUseCaseBinding)).Then<ConditionalRegistrationUseCaseManager>();
            var defaultRegistrationCases = Given<IUseCaseManager>.When<IUseCase>(useCase => useCase.GetUseCaseBindingType() == typeof(DefaultRegistrationUseCaseBinding)).Then<DefaultRegistrationUseCaseManager>();

            useCaseStore.Conditional.ResolutionCases.Add(typeof(IUseCaseManager), defaultCases);
            useCaseStore.Conditional.ResolutionCases.Add(typeof(IUseCaseManager), namedCases);
            useCaseStore.Default.ResolutionCases.Add(typeof(IUseCaseManager), conditionalCases);
            useCaseStore.Conditional.ResolutionCases.Add(typeof(IUseCaseManager), defaultActionCases);
            useCaseStore.Conditional.ResolutionCases.Add(typeof(IUseCaseManager), conditionalActionCases);
            useCaseStore.Conditional.ResolutionCases.Add(typeof(IUseCaseManager), conditionalRegistrationCases);
            useCaseStore.Conditional.ResolutionCases.Add(typeof(IUseCaseManager), defaultRegistrationCases);

            Bind(conditionalCases);
            Bind(defaultCases);
            Bind(namedCases);
            Bind(defaultActionCases);
            Bind(conditionalActionCases);
            Bind(conditionalRegistrationCases);
            Bind(defaultRegistrationCases);
        }

        private void Bind(IUseCase useCase)
        {
            Type bindingType = useCase.GetUseCaseBindingType().IsGenericType ?
                useCase.GetUseCaseBindingType().MakeGenericType(useCase.GetBaseBindingType())
                : useCase.GetUseCaseBindingType();

            if (useCase.GetBinding() is Type)
            {
                var binding = GetInstance<IUseCaseBinding>(bindingType);
                binding.Bind(useCase, this);
            }
            else
            {
                var binding = GetInstance<IInstanceUseCaseBinding>(bindingType);
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