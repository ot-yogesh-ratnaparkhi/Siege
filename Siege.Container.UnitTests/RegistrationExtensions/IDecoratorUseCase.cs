using System;
using Siege.ServiceLocation;

namespace Siege.Container.UnitTests.RegistrationExtensions
{
    public interface IDecoratorUseCase<TService> : IConditionalUseCase<TService> {}

    public class DecoratorUseCase<TService> : GenericUseCase<TService>, IDecoratorUseCase<TService>
    {
        public override Type GetUseCaseBindingType()
        {
            return typeof (IConditionalUseCaseBinding<>);
        }

        protected override IActivationStrategy<TService> GetActivationStrategy()
        {
            return new DecoratorActivationStrategy(typeof(TService), GetBoundType());
        }

        protected class DecoratorActivationStrategy : IActivationStrategy<TService>
        {
            private readonly Type decoratorType;
            private readonly Type serviceType;

            public DecoratorActivationStrategy(Type serviceType, Type decoratorType)
            {
                this.decoratorType = decoratorType;
                this.serviceType = serviceType;
            }

            public TService Resolve(IInstanceResolver locator)
            {
                var service = Activator.CreateInstance(serviceType);
                return (TService)Activator.CreateInstance(decoratorType, service);
            }
        }
    }

    public interface IDecoratorUseCaseBinding<TService> : IUseCaseBinding
    {
        
    }
}
