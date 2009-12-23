using System;
using System.Collections.Generic;
using Siege.ServiceLocation;

namespace Siege.ServiceLocation.UnitTests.RegistrationExtensions
{
    public class DecoratorUseCase<TService> : GenericUseCase<TService>, IDecoratorUseCase<TService>
    {
        private IContextualServiceLocator serviceLocator;

        public DecoratorUseCase(IContextualServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public override Type GetUseCaseBindingType()
        {
            return typeof (IDecoratorUseCaseBinding<>);
        }

        public override Type GetBaseBindingType()
        {
            return GetBoundType();
        }

        protected override IActivationStrategy GetActivationStrategy()
        {
            return new DecoratorActivationStrategy(serviceLocator, GetBoundType());
        }

        protected class DecoratorActivationStrategy : IActivationStrategy
        {
            private readonly IContextualServiceLocator serviceLocator;
            private readonly Type decoratedType;

            public DecoratorActivationStrategy(IContextualServiceLocator serviceLocator, Type decoratedType)
            {
                this.serviceLocator = serviceLocator;
                this.decoratedType = decoratedType;
            }

            public object Resolve(IInstanceResolver locator, IList<object> context)
            {
                var useCaseBinding = (IDecoratorUseCaseBinding)locator.GetInstance(typeof(IDecoratorUseCaseBinding<TService>));
                var decorators = new List<Type>();
                var rootObject = locator.GetInstance(decoratedType);

                foreach (IDecoratorUseCase useCase in serviceLocator.GetRegisteredUseCasesForType(decoratedType))
                {
                    if(useCase.IsValid(context)) decorators.Add(useCase.GetDecoratorType());
                }

                return GetInstance(useCaseBinding, rootObject, decoratedType, decorators);
            }

            private object GetInstance(IDecoratorUseCaseBinding binding, object rootObject, Type encapsulatedType, IList<Type> decorators)
            {
                if (decorators.Count == 0) return rootObject;

                Type decorator = decorators[0];
                IList<Type> prunedDecoratorList = new List<Type>(decorators);
                prunedDecoratorList.RemoveAt(0);

                object resolvedObject = binding.Resolve(decorator, encapsulatedType, rootObject);

                return GetInstance(binding, resolvedObject, encapsulatedType, prunedDecoratorList);
            }
        }

        public Type GetDecoratorType()
        {
            return typeof (TService);
        }
    }
}