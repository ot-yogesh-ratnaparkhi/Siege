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
using Siege.ServiceLocation.Exceptions;
using Siege.ServiceLocation.Resolution;
using Siege.ServiceLocation.Stores;
using Siege.ServiceLocation.Stores.UseCases;
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
        private IServiceLocatorStore store;
        private UseCaseStore useCaseStore = new UseCaseStore();
        private readonly Hashtable factories = new Hashtable();

        public SiegeContainer(IServiceLocatorAdapter serviceLocator, IServiceLocatorStore store, ITypeBuilder typeBuilder)
        {
            this.serviceLocator = serviceLocator;
            this.store = store;
            TypeHandler.Initialize(typeBuilder);

            AddBinding(typeof (IActionUseCaseBinding<>), typeof (ActionUseCaseBinding<>));
            AddBinding(typeof (IConditionalUseCaseBinding<>), this.serviceLocator.ConditionalUseCaseBinding);
            AddBinding(typeof (IDefaultUseCaseBinding<>), this.serviceLocator.DefaultUseCaseBinding);
            AddBinding(typeof (IKeyBasedUseCaseBinding<>), this.serviceLocator.KeyBasedUseCaseBinding);
            AddBinding(typeof (IOpenGenericUseCaseBinding), this.serviceLocator.OpenGenericUseCaseBinding);

            Register(Given<IFactoryFetcher>.Then(this));
            Register(Given<IServiceLocator>.Then(this));
            Register(Given<IContextualServiceLocator>.Then(this));
            Register(Given<IServiceLocatorAdapter>.Then(serviceLocator));
        }

        public SiegeContainer(IServiceLocatorAdapter serviceLocator, IServiceLocatorStore store) : this(serviceLocator, store, new DefaultTypeBuilder())
        {
            
        }

        public void AddContext(object contextItem)
        {
            store.ContextStore.Add(contextItem);
        }

        public IServiceLocator AddBinding(Type baseBinding, Type targetBinding)
        {
            serviceLocator.RegisterBinding(baseBinding, targetBinding);
            return this;
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

            IList<IUseCase> selectedCase = this.useCaseStore.Conditional.ResolutionCases.GetUseCasesForType(type);

            if (selectedCase != null)
            {
                foreach (IGenericUseCase useCase in selectedCase)
                {
                    var value = Resolve(useCase, new ConditionalResolutionStrategy(serviceLocator, this.store));

                    if (value != null) return value;
                }
            }
            else if (this.useCaseStore.Default.ResolutionCases.Contains(type))
            {
                IGenericUseCase useCase =
                    (IGenericUseCase) this.useCaseStore.Default.ResolutionCases.GetUseCaseForType(type);
                var value = Resolve(useCase, new DefaultResolutionStrategy(serviceLocator, this.store));

                if (value != null) return value;
            }

            if (HasTypeRegistered(type) || type.IsGenericType)
            {
                return serviceLocator.GetInstance(type);
            }

            throw new RegistrationNotFoundException(type);
        }

        public TService GetInstance<TService>(string key)
        {
            return (TService) GetInstance(typeof (TService), key, new IResolutionArgument[] {});
        }

        public TService GetInstance<TService>(string key, params IResolutionArgument[] arguments)
        {
            return (TService) GetInstance(typeof (TService), key, arguments);
        }

        public object GetService(Type serviceType, params IResolutionArgument[] arguments)
        {
            return GetInstance(serviceType, arguments);
        }

        public object GetInstance(Type type, string key, params IResolutionArgument[] arguments)
        {
            this.store.ResolutionStore.Add(new List<IResolutionArgument>(arguments));
            this.store.ExecutionStore = this.store.ExecutionStore.Create();
            return serviceLocator.GetInstance(type, key);
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
            this.store.ExecutionStore = this.store.ExecutionStore.Create();
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

        public IServiceLocator Register(IUseCase useCase)
        {
            if (useCase is IDefaultActionUseCase)
            {
                this.useCaseStore.Default.PostResolutionCases.Add(useCase.GetBoundType(), useCase);
            }
            else if (useCase is IConditionalActionUseCase)
            {
                this.useCaseStore.Conditional.PostResolutionCases.Add(useCase.GetBoundType(), useCase);
            }
            else if (useCase is IDefaultUseCase)
            {
                this.useCaseStore.Default.ResolutionCases.Add(useCase.GetBaseBindingType(), useCase);
            }
            else
            {
                this.useCaseStore.Conditional.ResolutionCases.Add(useCase.GetBaseBindingType(), useCase);
            }

            Type bindingType;

            if (useCase.GetUseCaseBindingType().IsGenericType)
            {
                bindingType = useCase.GetUseCaseBindingType().MakeGenericType(useCase.GetBaseBindingType());
            }
            else
            {
                bindingType = useCase.GetUseCaseBindingType();
            }

            if (useCase is IInstanceUseCase)
            {
                var binding = GetInstance<IInstanceUseCaseBinding>(bindingType);
                binding.BindInstance((IInstanceUseCase) useCase, this);
            }
            else
            {
                var binding = GetInstance<IUseCaseBinding>(bindingType);
                binding.Bind(useCase, this);
            }

            return this;
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

        private object Resolve(IGenericUseCase useCase, IResolutionStrategy strategy)
        {
            var value = useCase.Resolve(strategy, this.store);

            if (value != null)
            {
                this.store.ExecutionStore.Decrement();
                this.store.ExecutionStore = this.store.ExecutionStore.Create();

                ExecutePostConditions(this.useCaseStore.Default, useCase,
                                      actionUseCase => value = actionUseCase.Invoke(value));
                ExecutePostConditions(this.useCaseStore.Conditional, useCase, actionUseCase =>
                                                                                  {
                                                                                      if (
                                                                                          actionUseCase.IsValid(
                                                                                              this.store))
                                                                                          value =
                                                                                              actionUseCase.Invoke(value);
                                                                                  });
            }

            return value;
        }

        private void ExecutePostConditions(UseCaseGroup useCaseGroup, IGenericUseCase useCase,
                                           Action<IActionUseCase> action)
        {
            IList<IUseCase> actions = useCaseGroup.PostResolutionCases.GetUseCasesForType(useCase.GetBoundType());

            if (actions == null)
                actions = useCaseGroup.PostResolutionCases.GetUseCasesForType(useCase.GetBaseBindingType());

            if (actions != null)
            {
                foreach (IActionUseCase actionUseCase in actions)
                {
                    action(actionUseCase);
                }
            }
        }

        IGenericFactory<TBaseService> IFactoryFetcher.GetFactory<TBaseService>()
        {
            if (!factories.ContainsKey(typeof (TBaseService)))
            {
                lock (factories.SyncRoot)
                {
                    if (!factories.ContainsKey(typeof (TBaseService)))
                    {
                        Factory<TBaseService> factory = new Factory<TBaseService>(this);
                        Register(Given<Factory<TBaseService>>.Then("Factory" + typeof (TBaseService), factory));

                        factories.Add(typeof (TBaseService), factory);
                    }
                }
            }

            return (Factory<TBaseService>) factories[typeof (TBaseService)];
        }
    }
}