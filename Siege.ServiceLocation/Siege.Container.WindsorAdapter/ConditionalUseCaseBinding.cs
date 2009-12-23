using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Siege.ServiceLocation;

namespace Siege.SeviceLocation.WindsorAdapter
{
    public class ConditionalUseCaseBinding<TService> : IConditionalUseCaseBinding<TService>
    {
        private IKernel kernel;
        private IServiceLocatorAdapter locator;

        public ConditionalUseCaseBinding(IKernel kernel, IServiceLocatorAdapter locator)
        {
            this.kernel = kernel;
            this.locator = locator;
        }

        public void Bind(IUseCase useCase)
        {
            Bind((IConditionalUseCase<TService>)useCase);
        }

        private void Bind(IConditionalUseCase<TService> useCase)
        {
            var factory = (Factory<TService>)locator.GetFactory<TService>();
            factory.AddCase(useCase);

            kernel.Register(Component.For<TService>().UsingFactoryMethod(() => factory.Build()).LifeStyle.Transient.Unless(Component.ServiceAlreadyRegistered));
            kernel.Register(Component.For(useCase.GetBoundType()).LifeStyle.Transient.Unless(Component.ServiceAlreadyRegistered));
        }
    }
}