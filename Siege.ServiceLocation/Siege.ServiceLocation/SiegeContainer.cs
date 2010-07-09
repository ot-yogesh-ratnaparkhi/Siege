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
using Siege.ServiceLocation.ExtensionMethods;
using Siege.ServiceLocation.Policies;
using Siege.ServiceLocation.Resolution;
using Siege.ServiceLocation.Stores;
using Siege.ServiceLocation.Syntax;
using Siege.ServiceLocation.UseCases;
using Siege.ServiceLocation.UseCases.Managers;

namespace Siege.ServiceLocation
{
    public class SiegeContainer : IContextualServiceLocator
    {
        private readonly IServiceLocatorAdapter serviceLocator;
        private readonly IServiceLocatorStore store;
        private readonly IResolver resolver;
        private readonly Foundation foundation;
        private readonly Hashtable factories = new Hashtable();

        public SiegeContainer(IServiceLocatorAdapter serviceLocator, IServiceLocatorStore store)
        {
            this.serviceLocator = serviceLocator;
            this.store = store;
            foundation = new Foundation();
            resolver = new Resolver(serviceLocator, this.store, foundation);

            serviceLocator.RegisterInstance(typeof (IServiceLocatorAdapter), serviceLocator);

            Bind(Given<IResolver>.Then(resolver));

            RegisterPolicy(Given<Transient>.Then<Transient>());
            RegisterPolicy(Given<Singleton>.Then<Singleton>());

            Register<Singleton>(Given<IFactoryFetcher>.Then(this));
            Register<Singleton>(Given<IServiceLocator>.Then(this));
            Register<Singleton>(Given<IContextualServiceLocator>.Then(this));
        }

        public void AddContext(object contextItem)
        {
            store.ContextStore.Add(contextItem);
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
            store.ResolutionStore.Add(new List<IResolutionArgument>(arguments));

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
            store.ResolutionStore.Add(new List<IResolutionArgument>(arguments));

            return serviceLocator.GetInstance(type, key, store.ResolutionStore.Items.OfType<ConstructorParameter, IResolutionArgument>());
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

        public IServiceLocator Register<TRegistrationPolicy>(List<IUseCase> useCases)
            where TRegistrationPolicy : IRegistrationPolicy
        {
            foreach (IUseCase useCase in useCases) Register<TRegistrationPolicy>(useCase);

            return this;
        }

        public IServiceLocator Register<TRegistrationPolicy>(IUseCase useCase)
            where TRegistrationPolicy : IRegistrationPolicy
        {
            IUseCaseManager caseManager = foundation.ContainsUseCaseManagerForBinding(useCase.GetUseCaseBinding())
                                              ? foundation.GetUseCaseManager(useCase.GetUseCaseBinding())
                                              : GetInstance<IUseCaseManager>(new ContextArgument(useCase));

            var policy = GetInstance<TRegistrationPolicy>(new ContextArgument(useCase),
                                                          new ConstructorParameter {Name = "useCase", Value = useCase});

            caseManager.Add(policy);
            Bind(policy);

            return this;
        }

        private void RegisterPolicy(IUseCase useCase)
        {
            IUseCaseManager caseManager = foundation.ContainsUseCaseManagerForBinding(useCase.GetUseCaseBinding())
                                              ? foundation.GetUseCaseManager(useCase.GetUseCaseBinding())
                                              : GetInstance<IUseCaseManager>(new ContextArgument(useCase));

            caseManager.Add(useCase);

            Bind(useCase);
        }

        public IServiceLocator Register(IUseCase useCase)
        {
            return Register<Transient>(useCase);
        }

        private void Bind(IUseCase useCase)
        {
            useCase.GetUseCaseBinding().Bind(serviceLocator, useCase, this);
        }

        public IServiceLocatorStore Store
        {
            get { return store; }
        }

        public void Dispose()
        {
            serviceLocator.Dispose();
            store.Dispose();
        }

        IGenericFactory IFactoryFetcher.GetFactory(Type type)
        {
            if (!factories.ContainsKey(type))
            {
                lock (factories.SyncRoot)
                {
                    if (!factories.ContainsKey(type))
                    {
                        factories.Add(type, new Factory(this, foundation));
                    }
                }
            }

            return (Factory) factories[type];
        }
    }
}