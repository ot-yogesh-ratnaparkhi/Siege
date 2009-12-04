using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Siege.ServiceLocation;

namespace Siege.Container.UnitTests.RegistrationExtensions.Castle
{
    public class DecoratorUseCaseBinding<TService> : IDecoratorUseCaseBinding<TService>
    {
        private IKernel kernel;
        private IServiceLocatorAdapter locator;

        public DecoratorUseCaseBinding(IKernel kernel, IServiceLocatorAdapter locator)
        {
            this.kernel = kernel;
            this.locator = locator;
        }

        public void Bind(IUseCase useCase)
        {
            Bind((IDecoratorUseCase<TService>)useCase);
        }

        private void Bind(IDecoratorUseCase<TService> useCase)
        {
            var factory = (Factory<TService>)locator.GetFactory<TService>();
            factory.AddCase(useCase);

            if (typeof(TService) != useCase.GetBoundType()) kernel.Register(Component.For<TService>().UsingFactoryMethod(() => factory.Build(null)).LifeStyle.Transient.Unless(Component.ServiceAlreadyRegistered));
            kernel.Register(Component.For(useCase.GetBoundType()).ImplementedBy(useCase.GetBoundType()).LifeStyle.Transient.Unless(Component.ServiceAlreadyRegistered));
        }
    }
}
