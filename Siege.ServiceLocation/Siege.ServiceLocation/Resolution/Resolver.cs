using System;
using System.Collections.Generic;
using Siege.ServiceLocation.EventHandlers;
using Siege.ServiceLocation.Exceptions;
using Siege.ServiceLocation.ExtensionMethods;
using Siege.ServiceLocation.Stores;
using Siege.ServiceLocation.Stores.UseCases;
using Siege.ServiceLocation.UseCases;
using Siege.ServiceLocation.UseCases.Actions;
using Siege.ServiceLocation.UseCases.Conditional;
using Siege.ServiceLocation.UseCases.Default;

namespace Siege.ServiceLocation.Resolution
{
    public class Resolver : IResolver, ITypeResolver
    {
        private IInstanceResolver serviceLocator;
        private IServiceLocatorStore store;
        private UseCaseStore useCaseStore;
        public event TypeResolvedEventHandler TypeResolved;

        public Resolver(IInstanceResolver serviceLocator, IServiceLocatorStore store, UseCaseStore useCaseStore)
        {
            this.serviceLocator = serviceLocator;
            this.store = store;
            this.useCaseStore = useCaseStore;
            this.store.ExecutionStore.WireEvent(this);
        }

        public object Resolve(Type type)
        {
            IList<IUseCase> conditionalCases = useCaseStore.Conditional.ResolutionCases.GetUseCasesForType(type);
            var defaultCases = useCaseStore.Default.ResolutionCases;
            
            if (conditionalCases != null)
            {
                for (int i = 0; i < conditionalCases.Count; i++)
                {
                    var useCase = (IGenericUseCase)conditionalCases[i];
                    object value = null;

                    if (useCase.IsValid(this.store))
                    {
                        value = Resolve(useCase, new ConditionalResolutionStrategy(serviceLocator, this.store));
                    }

                    if (value != null)
                    {
                        return value;
                    }
                }
            }
            
            if (defaultCases.Contains(type))
            {
                var useCase = (IGenericUseCase)defaultCases.GetUseCaseForType(type);
                var value = Resolve(useCase, new DefaultResolutionStrategy(serviceLocator, this.store));

                if (value != null)
                {
                    return value;
                }
            }

            if (serviceLocator.HasTypeRegistered(type) || type.IsGenericType)
            {
                return serviceLocator.GetInstance(type, this.store.ResolutionStore.Items.OfType<ConstructorParameter, IResolutionArgument>());
            }

            throw new RegistrationNotFoundException(type);
        }

        private object Resolve(IGenericUseCase useCase, IResolutionStrategy strategy)
        {
            var value = useCase.Resolve(strategy, this.store);

            if (value != null)
            {
                this.RaiseTypeResolvedEvent(useCase.GetBoundType());
                ExecutePostConditions(useCaseStore.Default, useCase, actionUseCase => value = actionUseCase.Invoke(value));
                ExecutePostConditions(useCaseStore.Conditional, useCase, actionUseCase =>
                {
                    if (actionUseCase.IsValid(this.store))
                        value = actionUseCase.Invoke(value);
                });
            }

            return value;
        }

        private static void ExecutePostConditions(UseCaseGroup useCaseGroup, IUseCase useCase,
                                           Action<IActionUseCase> action)
        {
            IList<IUseCase> actions = useCaseGroup.PostResolutionCases.GetUseCasesForType(useCase.GetBoundType()) ??
                                      useCaseGroup.PostResolutionCases.GetUseCasesForType(useCase.GetBaseBindingType());

            if (actions != null)
            {
                for (int i = 0; i < actions.Count; i++)
                {
                    var actionUseCase = actions[i];
                    action((IActionUseCase)actionUseCase);
                }
            }
        }

        private void RaiseTypeResolvedEvent(Type type)
        {
            if (this.TypeResolved != null) this.TypeResolved(type);
        }
    }
}